using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Names one reward set in the current Derby assets.</summary>
public sealed record DerbyRewardSetDefinition
{
    /// <summary>Gets or sets the case-sensitive asset row name.</summary>
    /// <value>The <c>rewardSet</c> string naming a reward-set row in the Derby assets, matched exactly, or <see langword="null"/> when the JSON omits it.</value>
    [JsonPropertyName("rewardSet")]
    public string? Name { get; init; }
}
