using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>
/// Saved state of the player's decision boxes: gifts that let the player pick one of several rewards.
/// </summary>
public sealed record DecisionBoxManagerSnapshot
{
    /// <summary>
    /// Decision boxes the player has received but not chosen from yet.
    /// </summary>
    [JsonPropertyName("PendingBoxes")]
    public PendingDecisionBoxSnapshot[] PendingBoxes { get; init; } = [];
    /// <summary>
    /// Saved random seed the next box's reward options are drawn from.
    /// </summary>
    public int RandSeed { get; init; }
}
