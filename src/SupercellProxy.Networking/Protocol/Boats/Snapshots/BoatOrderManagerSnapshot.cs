using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.Snapshots;

/// <summary>Retains the pool used to balance boat difficulties across successive orders.</summary>
public sealed record BoatOrderManagerSnapshot
{
    /// <summary>Gets or sets the UTC day boundary of the last pool reset.</summary>
    /// <value>A Unix timestamp in seconds, saved as <c>dailyReset</c>; 0 when the pool was never reset.</value>
    [JsonPropertyName("dailyReset")]
    public int DailyResetTimestamp { get; init; }

    /// <summary>Gets or sets the ordered pool of difficulty data ids, including remaining duplicates.</summary>
    /// <value>The difficulty global ids still to be handed out, saved as <c>pool</c>; empty by default.</value>
    [JsonPropertyName("pool")]
    public int[] Pool { get; init; } = [];
}
