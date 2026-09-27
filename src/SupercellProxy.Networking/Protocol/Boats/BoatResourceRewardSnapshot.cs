using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Retains the native resource identifier and quantity of a crate reward.</summary>
public sealed record BoatResourceRewardSnapshot
{
    /// <summary>Gets the reward quantity.</summary>
    [JsonPropertyName("Value")]
    public int Amount { get; init; }
    /// <summary>Gets the resource's global data identifier.</summary>
    [JsonPropertyName("ID")]
    public int DataGlobalIdentifier { get; init; }
}
