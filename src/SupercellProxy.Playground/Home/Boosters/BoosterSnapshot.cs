using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">BoosterSnapshot</c> home data.
/// </summary>
internal sealed record BoosterSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">BoosterDataGlobalId</c> value.
    /// </summary>
    [JsonPropertyName("LogicBoosterDataGlobalID")]
    public int BoosterDataGlobalId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Timer</c> value.
    /// </summary>
    public TimerSnapshot Timer { get; init; }
}
