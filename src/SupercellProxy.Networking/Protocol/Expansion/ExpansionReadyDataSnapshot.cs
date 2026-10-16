using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Expansion;

/// <summary>
/// Represents decoded <c language="csharp">ExpansionReadyDataSnapshot</c> home data.
/// </summary>
public sealed record ExpansionReadyDataSnapshot
{
    /// <summary>
    /// Global id of the land expansion in the expansions table.
    /// </summary>
    [JsonPropertyName("LogicExpansionDataGlobalID")]
    public int ExpansionDataGlobalId { get; init; }

    /// <summary>
    /// One bit per tool the expansion needs; bit i is set once the i-th listed tool has been handed in.
    /// </summary>
    public int ReadyBits { get; init; }
}
