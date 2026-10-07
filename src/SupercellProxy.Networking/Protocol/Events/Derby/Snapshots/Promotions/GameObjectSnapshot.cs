using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Boats;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets the promotion reward attached to a mystery box or reward-bearing object.</summary>
    [JsonPropertyName("PopPromoBox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoatPromotionRewardSnapshot? PromotionReward { get; init; }
}
