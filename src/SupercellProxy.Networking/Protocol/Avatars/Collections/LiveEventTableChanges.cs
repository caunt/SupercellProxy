using System.Text.Json.Serialization;

using SupercellProxy.Networking.Assets.Tables;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>The table edits a live event document carries besides its own settings, such as a patch, County Fair or Valley event.</summary>
public sealed record LiveEventTableChanges
{
    /// <summary>Gets the CSV edits that apply while the event runs; null when the event carries none.</summary>
    [JsonPropertyName("csvChange")]
    public DataTableChangeSet? TableChanges { get; init; }
}
