using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>
/// Saved state of the player's decision boxes: gifts that let the player pick one of several rewards.
/// </summary>
public sealed record DecisionBoxManagerSnapshot
{
    /// <summary>
    /// Gets or sets the decision boxes the player has received but not chosen from yet.
    /// </summary>
    /// <value>Boxes in the order they were received. Empty by default.</value>
    [JsonPropertyName("PendingBoxes")]
    public PendingDecisionBoxSnapshot[] PendingBoxes { get; init; } = [];
    /// <summary>
    /// Gets or sets the saved random seed of the decision boxes.
    /// </summary>
    /// <value>An integer counter; 0 by default.</value>
    public int RandSeed { get; init; }
}
