using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Boats;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets or sets the promotion reward attached to a mystery box or reward-bearing object.</summary>
    /// <value>The Pop box reward and its collected marker saved under <c>PopPromoBox</c>, or <see langword="null"/> when the object carries no box; a <see langword="null"/> value is left out when writing JSON.</value>
    [JsonPropertyName("PopPromoBox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoatPromotionRewardSnapshot? PromotionReward { get; init; }
}
