using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.CountyFair;

/// <summary>Retains the County Fair's opening acknowledgement independently of ordinary task events.</summary>
public sealed record CountyFairSnapshot
{
    /// <summary>Gets whether the player has opened this County Fair.</summary>
    [JsonPropertyName("openedByPlayer")]
    public bool OpenedByPlayer { get; init; }
}
