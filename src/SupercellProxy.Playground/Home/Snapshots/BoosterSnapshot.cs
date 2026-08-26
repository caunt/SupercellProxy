using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>BoosterSnapshot</c> home data.
/// </summary>
public sealed record BoosterSnapshot
{
    /// <summary>
    /// Gets or sets the <c>BoosterDataGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("LogicBoosterDataGlobalID")]
    public int BoosterDataGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c>Timer</c> value.
    /// </summary>
    public TimerSnapshot Timer { get; init; }
}
