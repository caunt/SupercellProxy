using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">ExpansionReadyDataSnapshot</c> home data.
/// </summary>
internal sealed record ExpansionReadyDataSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">ExpansionDataGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("LogicExpansionDataGlobalID")]
    public int ExpansionDataGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ReadyBits</c> value.
    /// </summary>
    public int ReadyBits { get; init; }
}
