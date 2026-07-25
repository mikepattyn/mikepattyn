using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using BarbershopDbExplorer.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Xunit;
using DeleteRequest = BarbershopDbExplorer.Api.Models.DeleteRequest;

namespace BarbershopDbExplorer.Api.Tests;

public class ExplorerEndpointsTests
{
    private const string TableName = "Kapsalon-Test";
    private const string TenantId = "sabunandsteel";

    private static (Mock<IAmazonDynamoDB> Dynamo, Mock<IAwsClientFactory> Factory, TableNameResolver Resolver) MakeFixture()
    {
        var dynamo = new Mock<IAmazonDynamoDB>();
        var ssm = new Mock<IAmazonSimpleSystemsManagement>();
        ssm.Setup(s => s.GetParameterAsync(It.IsAny<GetParameterRequest>(), default))
            .ReturnsAsync(new GetParameterResponse { Parameter = new Parameter { Value = TableName } });

        var factory = new Mock<IAwsClientFactory>();
        factory.Setup(f => f.CreateDynamoClient(It.IsAny<string?>(), It.IsAny<string>())).Returns(dynamo.Object);
        factory.Setup(f => f.CreateSsmClient(It.IsAny<string?>(), It.IsAny<string>())).Returns(ssm.Object);

        return (dynamo, factory, new TableNameResolver());
    }

    [Fact]
    public async Task GetMetaAsync_ReturnsTableNameAndDeploymentEnvironment()
    {
        var (_, factory, resolver) = MakeFixture();

        var result = await ExplorerEndpoints.GetMetaAsync(
            factory.Object,
            resolver,
            env: "dev",
            tenantId: TenantId,
            region: "eu-central-1",
            profile: null,
            CancellationToken.None
        );

        var ok = Assert.IsType<Ok<MetaResponse>>(result);
        Assert.Equal(TableName, ok.Value!.TableName);
        Assert.Equal("Development", ok.Value.DeploymentEnvironment);
    }

    [Fact]
    public async Task GetMetaAsync_InvalidEnv_ReturnsErrorWithoutCallingAws()
    {
        var (_, factory, resolver) = MakeFixture();

        var result = await ExplorerEndpoints.GetMetaAsync(
            factory.Object,
            resolver,
            env: "staging",
            tenantId: TenantId,
            region: "eu-central-1",
            profile: null,
            CancellationToken.None
        );

        Assert.IsType<JsonHttpResult<ErrorResponse>>(result);
        factory.Verify(f => f.CreateSsmClient(It.IsAny<string?>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetItemsAsync_StaffMember_ReturnsPopulatedListItems()
    {
        var (dynamo, factory, resolver) = MakeFixture();
        dynamo
            .Setup(d => d.QueryAsync(It.IsAny<QueryRequest>(), default))
            .ReturnsAsync(
                new QueryResponse
                {
                    Items =
                    [
                        new Dictionary<string, AttributeValue>
                        {
                            ["PK"] = new("TENANT#sabunandsteel"),
                            ["SK"] = new("STAFF#marcus"),
                            ["StaffId"] = new("marcus"),
                            ["TenantId"] = new(TenantId),
                            ["Name"] = new("Marcus"),
                        },
                    ],
                }
            );

        var result = await ExplorerEndpoints.GetItemsAsync(
            factory.Object,
            resolver,
            env: "dev",
            tenantId: TenantId,
            region: "eu-central-1",
            profile: null,
            entityType: "StaffMember",
            CancellationToken.None
        );

        var ok = Assert.IsType<Ok<ItemsResponse>>(result);
        var item = Assert.Single(ok.Value!.Items);
        var staff = Assert.IsType<StaffMemberData>(item.Data);
        Assert.Equal("Marcus", staff.Name);
        Assert.Equal("Marcus (marcus)", item.Label);
    }

    [Fact]
    public async Task PutItemsAsync_NewAppointment_UsesTransactWriteForSlotLockAndAppointment()
    {
        var (dynamo, factory, resolver) = MakeFixture();
        TransactWriteItemsRequest? captured = null;
        dynamo
            .Setup(d => d.TransactWriteItemsAsync(It.IsAny<TransactWriteItemsRequest>(), default))
            .Callback<TransactWriteItemsRequest, CancellationToken>((request, _) => captured = request)
            .ReturnsAsync(new TransactWriteItemsResponse());

        var request = BuildUpsertRequest(
            "Appointment",
            isNew: true,
            new
            {
                AppointmentId = "appt-1",
                TenantId,
                CustomerId = "cust-1",
                ServiceId = "cut",
                StaffId = "marcus",
                Date = "2026-07-25",
                TimeSlot = "09:00",
                CustomerDisplayName = "Jane Doe",
                CreatedAt = "2026-07-01T00:00:00.000Z",
            }
        );

        var result = await ExplorerEndpoints.PutItemsAsync(factory.Object, resolver, request, CancellationToken.None);

        Assert.IsType<Ok<UpsertResponse>>(result);
        Assert.NotNull(captured);
        Assert.Equal(2, captured!.TransactItems.Count);
        dynamo.Verify(d => d.PutItemAsync(It.IsAny<PutItemRequest>(), default), Times.Never);
    }

    [Fact]
    public async Task PutItemsAsync_ExistingAppointment_UsesPlainPutItem()
    {
        var (dynamo, factory, resolver) = MakeFixture();
        dynamo.Setup(d => d.PutItemAsync(It.IsAny<PutItemRequest>(), default)).ReturnsAsync(new PutItemResponse());

        var request = BuildUpsertRequest(
            "Appointment",
            isNew: false,
            new
            {
                AppointmentId = "appt-1",
                TenantId,
                CustomerId = "cust-1",
                ServiceId = "cut",
                StaffId = "marcus",
                Date = "2026-07-25",
                TimeSlot = "09:00",
                CustomerDisplayName = "Jane Doe",
                CreatedAt = "2026-07-01T00:00:00.000Z",
            }
        );

        await ExplorerEndpoints.PutItemsAsync(factory.Object, resolver, request, CancellationToken.None);

        dynamo.Verify(d => d.PutItemAsync(It.IsAny<PutItemRequest>(), default), Times.Once);
        dynamo.Verify(d => d.TransactWriteItemsAsync(It.IsAny<TransactWriteItemsRequest>(), default), Times.Never);
    }

    [Fact]
    public async Task PutItemsAsync_NewCustomer_SetsCreatedAndUpdatedAtServerSide()
    {
        var (dynamo, factory, resolver) = MakeFixture();
        PutItemRequest? captured = null;
        dynamo
            .Setup(d => d.PutItemAsync(It.IsAny<PutItemRequest>(), default))
            .Callback<PutItemRequest, CancellationToken>((request, _) => captured = request)
            .ReturnsAsync(new PutItemResponse());

        var request = BuildUpsertRequest(
            "Customer",
            isNew: true,
            new
            {
                PrincipalId = "principal-1",
                Email = "a@b.com",
                DisplayName = "Jane Doe",
                GoogleName = "",
                CreatedAt = "",
                UpdatedAt = "",
            }
        );

        await ExplorerEndpoints.PutItemsAsync(factory.Object, resolver, request, CancellationToken.None);

        Assert.NotNull(captured);
        Assert.False(string.IsNullOrWhiteSpace(captured!.Item["CreatedAt"].S));
        Assert.False(string.IsNullOrWhiteSpace(captured.Item["UpdatedAt"].S));
    }

    [Fact]
    public async Task PutItemsAsync_TransactionCanceled_ReturnsConflict()
    {
        var (dynamo, factory, resolver) = MakeFixture();
        dynamo
            .Setup(d => d.TransactWriteItemsAsync(It.IsAny<TransactWriteItemsRequest>(), default))
            .ThrowsAsync(new TransactionCanceledException("TransactionCanceledException: conditions not met"));

        var request = BuildUpsertRequest(
            "Appointment",
            isNew: true,
            new
            {
                AppointmentId = "appt-1",
                TenantId,
                CustomerId = "cust-1",
                ServiceId = "cut",
                StaffId = "marcus",
                Date = "2026-07-25",
                TimeSlot = "09:00",
                CustomerDisplayName = "Jane Doe",
                CreatedAt = "2026-07-01T00:00:00.000Z",
            }
        );

        var result = await ExplorerEndpoints.PutItemsAsync(factory.Object, resolver, request, CancellationToken.None);

        var json = Assert.IsType<JsonHttpResult<ErrorResponse>>(result);
        Assert.Equal(409, json.StatusCode);
    }

    [Fact]
    public async Task DeleteItemsAsync_Appointment_WithInlinePayload_DeletesSlotLockAndAppointment()
    {
        var (dynamo, factory, resolver) = MakeFixture();
        TransactWriteItemsRequest? captured = null;
        dynamo
            .Setup(d => d.TransactWriteItemsAsync(It.IsAny<TransactWriteItemsRequest>(), default))
            .Callback<TransactWriteItemsRequest, CancellationToken>((request, _) => captured = request)
            .ReturnsAsync(new TransactWriteItemsResponse());

        var appointmentJson = JsonSerializer.SerializeToElement(
            new
            {
                AppointmentId = "appt-1",
                TenantId,
                CustomerId = "cust-1",
                ServiceId = "cut",
                StaffId = "marcus",
                Date = "2026-07-25",
                TimeSlot = "09:00",
                CustomerDisplayName = "Jane Doe",
                CreatedAt = "2026-07-01T00:00:00.000Z",
            }
        );

        var request = new DeleteRequest
        {
            Aws = new AwsConfig { Env = "dev", TenantId = TenantId, Region = "eu-central-1" },
            EntityType = "Appointment",
            PK = "TENANT#sabunandsteel",
            SK = "APPOINTMENT#appt-1",
            Appointment = appointmentJson,
        };

        var result = await ExplorerEndpoints.DeleteItemsAsync(factory.Object, resolver, request, CancellationToken.None);

        Assert.IsType<Ok<OkResponse>>(result);
        Assert.NotNull(captured);
        Assert.Equal(2, captured!.TransactItems.Count);
        dynamo.Verify(d => d.GetItemAsync(It.IsAny<GetItemRequest>(), default), Times.Never);
    }

    [Fact]
    public async Task DeleteItemsAsync_Appointment_WithoutInlinePayload_FetchesThenDeletesBoth()
    {
        var (dynamo, factory, resolver) = MakeFixture();
        dynamo
            .Setup(d => d.GetItemAsync(It.IsAny<GetItemRequest>(), default))
            .ReturnsAsync(
                new GetItemResponse
                {
                    Item = new Dictionary<string, AttributeValue>
                    {
                        ["AppointmentId"] = new("appt-1"),
                        ["TenantId"] = new(TenantId),
                        ["CustomerId"] = new("cust-1"),
                        ["ServiceId"] = new("cut"),
                        ["StaffId"] = new("marcus"),
                        ["Date"] = new("2026-07-25"),
                        ["TimeSlot"] = new("09:00"),
                        ["CustomerDisplayName"] = new("Jane Doe"),
                        ["CreatedAt"] = new("2026-07-01T00:00:00.000Z"),
                    },
                }
            );
        dynamo
            .Setup(d => d.TransactWriteItemsAsync(It.IsAny<TransactWriteItemsRequest>(), default))
            .ReturnsAsync(new TransactWriteItemsResponse());

        var request = new DeleteRequest
        {
            Aws = new AwsConfig { Env = "dev", TenantId = TenantId, Region = "eu-central-1" },
            EntityType = "Appointment",
            PK = "TENANT#sabunandsteel",
            SK = "APPOINTMENT#appt-1",
            Appointment = null,
        };

        var result = await ExplorerEndpoints.DeleteItemsAsync(factory.Object, resolver, request, CancellationToken.None);

        Assert.IsType<Ok<OkResponse>>(result);
        dynamo.Verify(d => d.GetItemAsync(It.IsAny<GetItemRequest>(), default), Times.Once);
        dynamo.Verify(d => d.TransactWriteItemsAsync(It.IsAny<TransactWriteItemsRequest>(), default), Times.Once);
    }

    [Fact]
    public async Task DeleteItemsAsync_NonAppointmentEntity_UsesPlainDelete()
    {
        var (dynamo, factory, resolver) = MakeFixture();
        dynamo.Setup(d => d.DeleteItemAsync(It.IsAny<DeleteItemRequest>(), default)).ReturnsAsync(new DeleteItemResponse());

        var request = new DeleteRequest
        {
            Aws = new AwsConfig { Env = "dev", TenantId = TenantId, Region = "eu-central-1" },
            EntityType = "StaffMember",
            PK = "TENANT#sabunandsteel",
            SK = "STAFF#marcus",
        };

        var result = await ExplorerEndpoints.DeleteItemsAsync(factory.Object, resolver, request, CancellationToken.None);

        Assert.IsType<Ok<OkResponse>>(result);
        dynamo.Verify(d => d.DeleteItemAsync(It.IsAny<DeleteItemRequest>(), default), Times.Once);
    }

    [Fact]
    public async Task GetOrphanSlotLocksAsync_ExcludesLocksWithExistingAppointment()
    {
        var (dynamo, factory, resolver) = MakeFixture();
        dynamo
            .Setup(d => d.QueryAsync(It.IsAny<QueryRequest>(), default))
            .ReturnsAsync(
                new QueryResponse
                {
                    Items =
                    [
                        new Dictionary<string, AttributeValue>
                        {
                            ["PK"] = new("TENANT#sabunandsteel"),
                            ["SK"] = new("SLOT#2026-07-25#09:00#marcus"),
                            ["TenantId"] = new(TenantId),
                            ["StaffId"] = new("marcus"),
                            ["Date"] = new("2026-07-25"),
                            ["TimeSlot"] = new("09:00"),
                            ["AppointmentId"] = new("orphan-appt"),
                        },
                        new Dictionary<string, AttributeValue>
                        {
                            ["PK"] = new("TENANT#sabunandsteel"),
                            ["SK"] = new("SLOT#2026-07-26#10:00#marcus"),
                            ["TenantId"] = new(TenantId),
                            ["StaffId"] = new("marcus"),
                            ["Date"] = new("2026-07-26"),
                            ["TimeSlot"] = new("10:00"),
                            ["AppointmentId"] = new("live-appt"),
                        },
                    ],
                }
            );
        dynamo
            .Setup(d => d.GetItemAsync(It.IsAny<GetItemRequest>(), default))
            .ReturnsAsync(
                (GetItemRequest request, CancellationToken _) =>
                    request.Key["SK"].S == "APPOINTMENT#live-appt"
                        ? new GetItemResponse
                        {
                            Item = new Dictionary<string, AttributeValue> { ["AppointmentId"] = new("live-appt") },
                        }
                        : new GetItemResponse { Item = null }
            );

        var result = await ExplorerEndpoints.GetOrphanSlotLocksAsync(
            factory.Object,
            resolver,
            env: "dev",
            tenantId: TenantId,
            region: "eu-central-1",
            profile: null,
            CancellationToken.None
        );

        var ok = Assert.IsType<Ok<OrphansResponse>>(result);
        var orphan = Assert.Single(ok.Value!.Orphans);
        Assert.Equal("orphan-appt", orphan.AppointmentId);
    }

    private static UpsertRequest BuildUpsertRequest(string entityType, bool isNew, object data) =>
        new()
        {
            Aws = new AwsConfig { Env = "dev", TenantId = TenantId, Region = "eu-central-1" },
            EntityType = entityType,
            IsNew = isNew,
            Data = JsonSerializer.SerializeToElement(data),
        };
}
