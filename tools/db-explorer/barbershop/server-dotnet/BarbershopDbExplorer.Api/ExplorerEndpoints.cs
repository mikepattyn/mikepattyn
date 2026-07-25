using System.Text.Json;
using Amazon.DynamoDBv2.Model;
using BarbershopDbExplorer.Api.Models;
using DeleteRequest = BarbershopDbExplorer.Api.Models.DeleteRequest;

namespace BarbershopDbExplorer.Api;

public static class ExplorerEndpoints
{
    private static readonly HashSet<string> ValidEnvs = ["dev", "acc", "prod"];

    public static async Task<IResult> GetMetaAsync(
        IAwsClientFactory clientFactory,
        TableNameResolver tableNameResolver,
        string env,
        string tenantId,
        string region,
        string? profile,
        CancellationToken cancellationToken
    )
    {
        try
        {
            ValidateEnv(env);
            var ssm = clientFactory.CreateSsmClient(profile, region);
            var tableName = await tableNameResolver.ResolveTableNameAsync(ssm, env, profile, region, cancellationToken);

            return Results.Ok(
                new MetaResponse
                {
                    TableName = tableName,
                    DeploymentEnvironment = DynamoKeys.DeploymentEnv(env),
                    Region = region,
                }
            );
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    public static async Task<IResult> GetItemsAsync(
        IAwsClientFactory clientFactory,
        TableNameResolver tableNameResolver,
        string env,
        string tenantId,
        string region,
        string? profile,
        string entityType,
        CancellationToken cancellationToken
    )
    {
        try
        {
            ValidateEnv(env);
            var dynamo = clientFactory.CreateDynamoClient(profile, region);
            var ssm = clientFactory.CreateSsmClient(profile, region);
            var tableName = await tableNameResolver.ResolveTableNameAsync(ssm, env, profile, region, cancellationToken);

            var (pk, skPrefix, exactSk) = EntityMapper.ListQuery(entityType, tenantId);
            var rawItems = await DynamoQueryOperations.QueryItemsAsync(
                dynamo,
                tableName,
                pk,
                skPrefix,
                exactSk,
                cancellationToken
            );
            var items = rawItems.Select(item => EntityMapper.ToListItem(entityType, item)).ToList();

            return Results.Ok(new ItemsResponse { Items = items });
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    public static async Task<IResult> PutItemsAsync(
        IAwsClientFactory clientFactory,
        TableNameResolver tableNameResolver,
        UpsertRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            ValidateEnv(request.Aws.Env);
            var dynamo = clientFactory.CreateDynamoClient(request.Aws.Profile, request.Aws.Region);
            var ssm = clientFactory.CreateSsmClient(request.Aws.Profile, request.Aws.Region);
            var tableName = await tableNameResolver.ResolveTableNameAsync(
                ssm,
                request.Aws.Env,
                request.Aws.Profile,
                request.Aws.Region,
                cancellationToken
            );

            object savedData;

            if (request.EntityType == "Appointment")
            {
                var appointment = Deserialize<AppointmentData>(request.Data);
                var slotLock = EntityMapper.MapSlotLockFromAppointment(appointment);
                var item = EntityMapper.MapToItem(appointment);

                if (request.IsNew)
                {
                    await DynamoQueryOperations.TransactWriteAsync(
                        dynamo,
                        tableName,
                        [
                            new TransactWriteItem
                            {
                                Put = new Put
                                {
                                    Item = slotLock,
                                    ConditionExpression = "attribute_not_exists(PK) AND attribute_not_exists(SK)",
                                },
                            },
                            new TransactWriteItem
                            {
                                Put = new Put
                                {
                                    Item = item,
                                    ConditionExpression = "attribute_not_exists(PK) AND attribute_not_exists(SK)",
                                },
                            },
                        ],
                        cancellationToken
                    );
                }
                else
                {
                    await DynamoQueryOperations.PutItemAsync(dynamo, tableName, item, cancellationToken);
                }

                savedData = appointment;
            }
            else if (request.EntityType == "Customer")
            {
                var customer = Deserialize<CustomerData>(request.Data);
                var now = DateTimeOffset.UtcNow.ToString("O");
                customer = request.IsNew
                    ? customer with
                    {
                        CreatedAt = string.IsNullOrWhiteSpace(customer.CreatedAt) ? now : customer.CreatedAt,
                        UpdatedAt = now,
                    }
                    : customer with
                    {
                        UpdatedAt = now,
                    };

                await DynamoQueryOperations.PutItemAsync(
                    dynamo,
                    tableName,
                    EntityMapper.MapToItem(customer),
                    cancellationToken
                );
                savedData = customer;
            }
            else
            {
                savedData = request.EntityType switch
                {
                    "TenantProfile" => await PutTyped<TenantProfileData>(dynamo, tableName, request.Data, cancellationToken),
                    "StaffMember" => await PutTyped<StaffMemberData>(dynamo, tableName, request.Data, cancellationToken),
                    "Service" => await PutTyped<ServiceData>(dynamo, tableName, request.Data, cancellationToken),
                    _ => throw new ArgumentException($"Unknown entity type: {request.EntityType}"),
                };
            }

            return Results.Ok(new UpsertResponse { Item = savedData });
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    public static async Task<IResult> DeleteItemsAsync(
        IAwsClientFactory clientFactory,
        TableNameResolver tableNameResolver,
        DeleteRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            ValidateEnv(request.Aws.Env);
            var dynamo = clientFactory.CreateDynamoClient(request.Aws.Profile, request.Aws.Region);
            var ssm = clientFactory.CreateSsmClient(request.Aws.Profile, request.Aws.Region);
            var tableName = await tableNameResolver.ResolveTableNameAsync(
                ssm,
                request.Aws.Env,
                request.Aws.Profile,
                request.Aws.Region,
                cancellationToken
            );

            if (request.EntityType == "Appointment")
            {
                var appointment =
                    request.Appointment is { } inline
                        ? Deserialize<AppointmentData>(inline)
                        : await LoadExistingAppointment(dynamo, tableName, request.PK, request.SK, cancellationToken);

                if (appointment is null)
                {
                    return Results.Json(new ErrorResponse { Error = "Appointment not found" }, statusCode: 404);
                }

                var slotLockKey = EntityMapper.MapSlotLockFromAppointment(appointment)["SK"].S;

                await DynamoQueryOperations.TransactWriteAsync(
                    dynamo,
                    tableName,
                    [
                        new TransactWriteItem
                        {
                            Delete = new Delete
                            {
                                Key = new Dictionary<string, AttributeValue>
                                {
                                    ["PK"] = new(request.PK),
                                    ["SK"] = new(slotLockKey),
                                },
                            },
                        },
                        new TransactWriteItem
                        {
                            Delete = new Delete
                            {
                                Key = new Dictionary<string, AttributeValue>
                                {
                                    ["PK"] = new(request.PK),
                                    ["SK"] = new(request.SK),
                                },
                            },
                        },
                    ],
                    cancellationToken
                );
            }
            else
            {
                await DynamoQueryOperations.DeleteItemAsync(dynamo, tableName, request.PK, request.SK, cancellationToken);
            }

            return Results.Ok(new OkResponse());
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    public static async Task<IResult> GetOrphanSlotLocksAsync(
        IAwsClientFactory clientFactory,
        TableNameResolver tableNameResolver,
        string env,
        string tenantId,
        string region,
        string? profile,
        CancellationToken cancellationToken
    )
    {
        try
        {
            ValidateEnv(env);
            var dynamo = clientFactory.CreateDynamoClient(profile, region);
            var ssm = clientFactory.CreateSsmClient(profile, region);
            var tableName = await tableNameResolver.ResolveTableNameAsync(ssm, env, profile, region, cancellationToken);

            var (pk, _, _) = EntityMapper.ListQuery("Appointment", tenantId);
            var slotLocks = await DynamoQueryOperations.QueryItemsAsync(
                dynamo,
                tableName,
                pk,
                "SLOT#",
                exactSk: false,
                cancellationToken
            );

            var orphans = new List<SlotLockOrphan>();
            foreach (var lockItem in slotLocks)
            {
                var orphan = EntityMapper.MapSlotLockOrphan(lockItem);
                var existsTenantId = string.IsNullOrEmpty(orphan.TenantId) ? tenantId : orphan.TenantId;
                var (existsPk, existsSk) = EntityMapper.AppointmentExistsKey(existsTenantId, orphan.AppointmentId);
                var appointment = await DynamoQueryOperations.GetItemAsync(
                    dynamo,
                    tableName,
                    existsPk,
                    existsSk,
                    cancellationToken
                );

                if (appointment is null)
                {
                    orphans.Add(orphan);
                }
            }

            return Results.Ok(new OrphansResponse { Orphans = orphans });
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    public static async Task<IResult> DeleteOrphanSlotLockAsync(
        IAwsClientFactory clientFactory,
        TableNameResolver tableNameResolver,
        DeleteOrphanSlotLockRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var dynamo = clientFactory.CreateDynamoClient(request.Aws.Profile, request.Aws.Region);
            var ssm = clientFactory.CreateSsmClient(request.Aws.Profile, request.Aws.Region);
            var tableName = await tableNameResolver.ResolveTableNameAsync(
                ssm,
                request.Aws.Env,
                request.Aws.Profile,
                request.Aws.Region,
                cancellationToken
            );

            await DynamoQueryOperations.DeleteItemAsync(dynamo, tableName, request.PK, request.SK, cancellationToken);

            return Results.Ok(new OkResponse());
        }
        catch (Exception ex)
        {
            return Error(ex);
        }
    }

    private static async Task<AppointmentData?> LoadExistingAppointment(
        Amazon.DynamoDBv2.IAmazonDynamoDB dynamo,
        string tableName,
        string pk,
        string sk,
        CancellationToken cancellationToken
    )
    {
        var existing = await DynamoQueryOperations.GetItemAsync(dynamo, tableName, pk, sk, cancellationToken);
        return existing is null ? null : EntityMapper.MapAppointment(existing);
    }

    private static async Task<T> PutTyped<T>(
        Amazon.DynamoDBv2.IAmazonDynamoDB dynamo,
        string tableName,
        JsonElement data,
        CancellationToken cancellationToken
    )
        where T : notnull
    {
        var typed = Deserialize<T>(data);
        var item = typed switch
        {
            TenantProfileData profile => EntityMapper.MapToItem(profile),
            StaffMemberData staff => EntityMapper.MapToItem(staff),
            ServiceData service => EntityMapper.MapToItem(service),
            _ => throw new ArgumentException($"Unsupported type: {typeof(T)}"),
        };

        await DynamoQueryOperations.PutItemAsync(dynamo, tableName, item, cancellationToken);
        return typed;
    }

    private static T Deserialize<T>(JsonElement data) =>
        data.Deserialize<T>() ?? throw new ArgumentException("Request body data was empty or malformed.");

    private static void ValidateEnv(string env)
    {
        if (!ValidEnvs.Contains(env))
        {
            throw new ArgumentException("Invalid env. Use dev, acc, or prod.");
        }
    }

    private static IResult Error(Exception ex)
    {
        var statusCode = ex switch
        {
            TransactionCanceledException => 409,
            _ when ex.Message.Contains("TransactionCanceled", StringComparison.OrdinalIgnoreCase) => 409,
            _ => 500,
        };

        return Results.Json(new ErrorResponse { Error = ex.Message }, statusCode: statusCode);
    }
}
