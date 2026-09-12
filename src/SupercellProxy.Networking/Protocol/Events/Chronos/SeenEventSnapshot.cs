using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Defines the Seen Event Snapshot contract.
/// </summary>
public sealed record SeenEventSnapshot
{

    /// <summary>
    /// Gets the Count value.
    /// </summary>
    [JsonPropertyName("cnt")]
    public int Count { get; init; } = 1;

    /// <summary>
    /// Gets the End Time value.
    /// </summary>
    [JsonPropertyName("et")]
    public int EndTime { get; init; }
    /// <summary>
    /// Gets the Event Id value.
    /// </summary>
    [JsonPropertyName("id")]
    public int EventIdentifier { get; init; }

    /// <summary>
    /// Gets the Impression Id value.
    /// </summary>
    [JsonPropertyName("imp")]
    public int ImpressionIdentifier { get; init; }

    /// <summary>
    /// Gets the Kind value.
    /// </summary>
    [JsonPropertyName("st")]
    public int Kind { get; init; }
}
