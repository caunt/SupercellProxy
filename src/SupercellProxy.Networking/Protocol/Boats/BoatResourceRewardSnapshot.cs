using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Retains the native resource id and quantity of a crate reward.</summary>
public sealed record BoatResourceRewardSnapshot
{
    /// <summary>Gets the reward quantity.</summary>
    [JsonPropertyName("Value")]
    public int Amount { get; init; }
    /// <summary>Gets the resource's global data id.</summary>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }
}
