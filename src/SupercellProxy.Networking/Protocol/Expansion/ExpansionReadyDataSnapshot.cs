using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Expansion;

/// <summary>
/// Represents decoded <c language="csharp">ExpansionReadyDataSnapshot</c> home data.
/// </summary>
public sealed record ExpansionReadyDataSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">ExpansionDataGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("LogicExpansionDataGlobalID")]
    public int ExpansionDataGlobalIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ReadyBits</c> value.
    /// </summary>
    public int ReadyBits { get; init; }
}
