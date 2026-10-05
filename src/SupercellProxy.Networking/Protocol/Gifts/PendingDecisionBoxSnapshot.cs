using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Gifts;

/// <summary>A saved reward box awaiting the player's choice.</summary>
public sealed record PendingDecisionBoxSnapshot
{
    /// <summary>Gets the source's saved reward metadata.</summary>
    [JsonPropertyName("details")]
    public DecisionBoxSourceDetail[] Details { get; init; } = [];

    /// <summary>Gets the decision-box data identifier.</summary>
    [JsonPropertyName("GlobalId")]
    public int GlobalIdentifier { get; init; }

    /// <summary>Gets the native acquisition reason carried by this reward.</summary>
    public int SourceTag { get; init; }
}
