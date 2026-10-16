using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Chronos;

/// <summary>
/// Record that the player looked at one showing (impression) of a targeted event, so the same showing is not announced again.
/// </summary>
public sealed record SeenEventSnapshot
{
    /// <summary>
    /// How many times this showing was looked at (saved as <c>cnt</c>); a new record starts at 1.
    /// </summary>
    [JsonPropertyName("cnt")]
    public int Count { get; init; } = 1;

    /// <summary>
    /// Unix seconds when the event ends (saved as <c>et</c>), after which the record can be dropped.
    /// </summary>
    [JsonPropertyName("et")]
    public int EndTime { get; init; }
    /// <summary>
    /// The targeted event that was looked at (saved as <c>id</c>).
    /// </summary>
    [JsonPropertyName("id")]
    public int EventId { get; init; }

    /// <summary>
    /// Which showing of the event it was (saved as <c>imp</c>); the same event can be shown to the player several times.
    /// </summary>
    [JsonPropertyName("imp")]
    public int ImpressionId { get; init; }

    /// <summary>
    /// Kind of record (saved as <c>st</c>); records for targeted events use 1.
    /// </summary>
    [JsonPropertyName("st")]
    public int Kind { get; init; }
}
