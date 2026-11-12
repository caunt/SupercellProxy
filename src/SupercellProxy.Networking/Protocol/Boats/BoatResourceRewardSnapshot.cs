using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Retains the native resource id and quantity of a crate reward.</summary>
public sealed record BoatResourceRewardSnapshot
{
    /// <summary>Gets or sets the reward quantity.</summary>
    /// <value>How many units of <see cref="DataGlobalId"/> the reward grants, saved as <c>Value</c>.</value>
    [JsonPropertyName("Value")]
    public int Amount { get; init; }
    /// <summary>Gets or sets the resource's global data id.</summary>
    /// <value>The data global id of the granted resource, saved as <c>ID</c>.</value>
    [JsonPropertyName("ID")]
    public int DataGlobalId { get; init; }
}
