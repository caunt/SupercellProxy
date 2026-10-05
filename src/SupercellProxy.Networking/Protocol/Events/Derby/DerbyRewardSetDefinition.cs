using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Names one reward set in the current Derby assets.</summary>
public sealed record DerbyRewardSetDefinition
{
    /// <summary>Gets the case-sensitive asset row name.</summary>
    [JsonPropertyName("rewardSet")]
    public string? Name { get; init; }
}
