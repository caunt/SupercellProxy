using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Expansion;

/// <summary>
/// Represents decoded <c language="csharp">ExpansionReadyDataSnapshot</c> home data.
/// </summary>
public sealed record ExpansionReadyDataSnapshot
{
    /// <summary>
    /// Gets or sets the global id of the land expansion whose handed-in materials this entry records.
    /// </summary>
    /// <value>A row of the expansions table, or of the train expansions table for town land, saved as <c>LogicExpansionDataGlobalID</c>.</value>
    [JsonPropertyName("LogicExpansionDataGlobalID")]
    public int ExpansionDataGlobalId { get; init; }

    /// <summary>
    /// Gets or sets which of the expansion's required tools have already been handed in; bit i is set once the i-th listed tool has been handed in.
    /// </summary>
    /// <value>A bit mask over the expansion's <c>CollectionTool</c> list; 0 means nothing has been handed in yet.</value>
    public int ReadyBits { get; init; }
}
