using System.Text.Json;
using System.Text.Json.Serialization;

namespace BarbershopDbExplorer.Api.Models;

public sealed record AwsConfig
{
    [JsonPropertyName("env")]
    public required string Env { get; init; }

    [JsonPropertyName("tenantId")]
    public required string TenantId { get; init; }

    [JsonPropertyName("profile")]
    public string? Profile { get; init; }

    [JsonPropertyName("region")]
    public required string Region { get; init; }
}

public sealed record MetaResponse
{
    [JsonPropertyName("tableName")]
    public required string TableName { get; init; }

    [JsonPropertyName("deploymentEnvironment")]
    public required string DeploymentEnvironment { get; init; }

    [JsonPropertyName("region")]
    public required string Region { get; init; }
}

public sealed record ListItem
{
    public required string PK { get; init; }

    public required string SK { get; init; }

    [JsonPropertyName("entityType")]
    public required string EntityType { get; init; }

    [JsonPropertyName("label")]
    public required string Label { get; init; }

    [JsonPropertyName("data")]
    public required object Data { get; init; }
}

public sealed record UpsertRequest
{
    [JsonPropertyName("aws")]
    public required AwsConfig Aws { get; init; }

    [JsonPropertyName("entityType")]
    public required string EntityType { get; init; }

    [JsonPropertyName("isNew")]
    public required bool IsNew { get; init; }

    [JsonPropertyName("data")]
    public required JsonElement Data { get; init; }
}

public sealed record DeleteRequest
{
    [JsonPropertyName("aws")]
    public required AwsConfig Aws { get; init; }

    [JsonPropertyName("entityType")]
    public required string EntityType { get; init; }

    public required string PK { get; init; }

    public required string SK { get; init; }

    [JsonPropertyName("appointment")]
    public JsonElement? Appointment { get; init; }
}

public sealed record DeleteOrphanSlotLockRequest
{
    [JsonPropertyName("aws")]
    public required AwsConfig Aws { get; init; }

    public required string PK { get; init; }

    public required string SK { get; init; }
}
