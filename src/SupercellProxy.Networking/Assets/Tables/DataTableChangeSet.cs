using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Assets.Tables;

/// <summary>The JSON payload of a live table-change event: the CSV edits that apply while the event runs.</summary>
public sealed record DataTableChangeSet
{
    /// <summary>Gets or sets the edits in the order they are applied.</summary>
    /// <value>The event's <c>Changes</c> array, where later edits see the result of earlier ones; <see langword="null"/> when the event
    /// carries none.</value>
    [JsonPropertyName("Changes")]
    public DataTableChange[]? Changes { get; init; }
}
