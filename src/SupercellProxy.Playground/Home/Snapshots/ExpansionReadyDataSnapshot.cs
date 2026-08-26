using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>ExpansionReadyDataSnapshot</c> home data.
/// </summary>
public sealed record ExpansionReadyDataSnapshot
{
    /// <summary>
    /// Gets or sets the <c>ExpansionDataGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("LogicExpansionDataGlobalID")]
    public int ExpansionDataGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c>ReadyBits</c> value.
    /// </summary>
    public int ReadyBits { get; init; }
}
