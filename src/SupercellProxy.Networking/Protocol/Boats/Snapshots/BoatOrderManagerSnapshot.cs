using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.Snapshots;

/// <summary>Retains the pool used to balance boat difficulties across successive orders.</summary>
public sealed record BoatOrderManagerSnapshot
{
    /// <summary>Gets the UTC day boundary of the last pool reset.</summary>
    [JsonPropertyName("dailyReset")]
    public int DailyResetTimestamp { get; init; }

    /// <summary>Gets the ordered pool of difficulty data ids, including remaining duplicates.</summary>
    [JsonPropertyName("pool")]
    public int[] Pool { get; init; } = [];
}
