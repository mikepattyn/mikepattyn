using BarbershopDbExplorer.Api;
using BarbershopDbExplorer.Api.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://127.0.0.1:3847");

builder.Services.AddSingleton<IAwsClientFactory, AwsClientFactory>();
builder.Services.AddSingleton<TableNameResolver>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Entity data fields (TenantId, DisplayName, ...) must stay PascalCase to match the
    // existing Vue frontend's shared/types.ts contract; envelope fields override this via
    // explicit [JsonPropertyName] attributes where the contract is camelCase.
    options.SerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();

app.MapGet(
    "/api/meta",
    (
        IAwsClientFactory clientFactory,
        TableNameResolver tableNameResolver,
        CancellationToken cancellationToken,
        string env = "dev",
        string tenantId = "sabunandsteel",
        string region = "eu-central-1",
        string? profile = null
    ) => ExplorerEndpoints.GetMetaAsync(clientFactory, tableNameResolver, env, tenantId, region, profile, cancellationToken)
);

app.MapGet(
    "/api/items",
    (
        IAwsClientFactory clientFactory,
        TableNameResolver tableNameResolver,
        CancellationToken cancellationToken,
        string entityType,
        string env = "dev",
        string tenantId = "sabunandsteel",
        string region = "eu-central-1",
        string? profile = null
    ) =>
        ExplorerEndpoints.GetItemsAsync(
            clientFactory,
            tableNameResolver,
            env,
            tenantId,
            region,
            profile,
            entityType,
            cancellationToken
        )
);

app.MapPut(
    "/api/items",
    (IAwsClientFactory clientFactory, TableNameResolver tableNameResolver, UpsertRequest request, CancellationToken cancellationToken) =>
        ExplorerEndpoints.PutItemsAsync(clientFactory, tableNameResolver, request, cancellationToken)
);

app.MapDelete(
    "/api/items",
    (
        IAwsClientFactory clientFactory,
        TableNameResolver tableNameResolver,
        [FromBody] DeleteRequest request,
        CancellationToken cancellationToken
    ) => ExplorerEndpoints.DeleteItemsAsync(clientFactory, tableNameResolver, request, cancellationToken)
);

app.MapGet(
    "/api/orphans/slot-locks",
    (
        IAwsClientFactory clientFactory,
        TableNameResolver tableNameResolver,
        CancellationToken cancellationToken,
        string env = "dev",
        string tenantId = "sabunandsteel",
        string region = "eu-central-1",
        string? profile = null
    ) =>
        ExplorerEndpoints.GetOrphanSlotLocksAsync(
            clientFactory,
            tableNameResolver,
            env,
            tenantId,
            region,
            profile,
            cancellationToken
        )
);

app.MapDelete(
    "/api/orphans/slot-locks",
    (
        IAwsClientFactory clientFactory,
        TableNameResolver tableNameResolver,
        [FromBody] DeleteOrphanSlotLockRequest request,
        CancellationToken cancellationToken
    ) => ExplorerEndpoints.DeleteOrphanSlotLockAsync(clientFactory, tableNameResolver, request, cancellationToken)
);

app.Run();

public partial class Program { }
