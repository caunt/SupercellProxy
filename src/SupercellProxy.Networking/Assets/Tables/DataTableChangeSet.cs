using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>The JSON payload of a live table-change event: the CSV edits that apply while the event runs.</summary>
public sealed record DataTableChangeSet
{
    /// <summary>Edits in the order they are applied; null when the event carries none.</summary>
    [JsonPropertyName("Changes")]
    public DataTableChange[]? Changes { get; init; }
}
