using System.Text.Json.Serialization;

namespace BarbershopDbExplorer.Api.Models;

public sealed record ItemsResponse
{
    [JsonPropertyName("items")]
    public required List<ListItem> Items { get; init; }
}

public sealed record UpsertResponse
{
    [JsonPropertyName("ok")]
    public bool Ok { get; init; } = true;

    [JsonPropertyName("item")]
    public required object Item { get; init; }
}

public sealed record OkResponse
{
    [JsonPropertyName("ok")]
    public bool Ok { get; init; } = true;
}

public sealed record OrphansResponse
{
    [JsonPropertyName("orphans")]
    public required List<SlotLockOrphan> Orphans { get; init; }
}

public sealed record ErrorResponse
{
    [JsonPropertyName("error")]
    public required string Error { get; init; }
}
