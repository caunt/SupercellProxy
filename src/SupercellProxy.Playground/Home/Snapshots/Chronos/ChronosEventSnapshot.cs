using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>ChronosEventSnapshot</c> home data.
/// </summary>
public sealed record ChronosEventSnapshot
{
    /// <summary>
    /// Gets or sets the <c>Type</c> value.
    /// </summary>
    [JsonPropertyName("type")]
    public int Type { get; init; }

    /// <summary>
    /// Gets or sets the <c>StartTime</c> value.
    /// </summary>
    [JsonPropertyName("startTime")]
    public long StartTime { get; init; }

    /// <summary>
    /// Gets or sets the <c>EndTime</c> value.
    /// </summary>
    [JsonPropertyName("endTime")]
    public long EndTime { get; init; }
}
