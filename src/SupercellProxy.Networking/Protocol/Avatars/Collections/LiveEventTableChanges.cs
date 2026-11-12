using System.Text.Json.Serialization;

using SupercellProxy.Networking.Assets.Tables;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>The table edits a live event document carries besides its own settings, such as a patch, County Fair or Valley event.</summary>
public sealed record LiveEventTableChanges
{
    /// <summary>Gets or sets the CSV edits that apply to the data tables while the event runs, read from the event document's <c>csvChange</c> object.</summary>
    /// <value>The change set whose edits are applied in order on top of the base tables, or <see langword="null"/> when the event document has no <c>csvChange</c> object.</value>
    [JsonPropertyName("csvChange")]
    public DataTableChangeSet? TableChanges { get; init; }
}
