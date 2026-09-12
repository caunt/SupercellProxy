using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Defines the Order Track Snapshot contract.
/// </summary>
public sealed record OrderTrackSnapshot
{
    /// <summary>
    /// Gets the Completed value.
    /// </summary>
    [JsonPropertyName("completed")]
    public int Completed { get; init; }

    /// <summary>
    /// Gets the Index value.
    /// </summary>
    [JsonPropertyName("idx")]
    public int Index { get; init; }

    /// <summary>
    /// Gets the Pending Completion Points value.
    /// </summary>
    [JsonPropertyName("pendingCompletionPoints")]
    public int PendingCompletionPoints { get; init; }

    /// <summary>
    /// Gets the Reset Timestamp value.
    /// </summary>
    [JsonPropertyName("reset")]
    public int ResetTimestamp { get; init; }

    /// <summary>
    /// Gets the Rewards value.
    /// </summary>
    [JsonPropertyName("rewards")]
    public OrderTrackReward[][] Rewards { get; init; } = [];

    /// <summary>
    /// Gets the Target value.
    /// </summary>
    [JsonPropertyName("target")]
    public int Target { get; init; }

    /// <summary>
    /// Gets the Track Id value.
    /// </summary>
    [JsonPropertyName("track")]
    public int TrackIdentifier { get; init; }
}
