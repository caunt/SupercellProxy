using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Timing;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>
/// Represents decoded <c language="csharp">BoosterSnapshot</c> home data.
/// </summary>
public sealed record BoosterSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">BoosterDataGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("LogicBoosterDataGlobalID")]
    public int BoosterDataGlobalIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Timer</c> value.
    /// </summary>
    public TimerSnapshot Timer { get; init; }
}
