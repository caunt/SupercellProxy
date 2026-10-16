using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.FarmPass.Rewards;

/// <summary>
/// One baby pet reward of a Farm Pass level: the event's name and amount, and the resolved global id and count once they are
/// filled in.
/// </summary>
public sealed record FarmPassBabyPetReward : ExtensibleDocument
{
    /// <summary>How many baby pets the event's definition gives (saved as <c>rewardAmount</c>).</summary>
    [JsonPropertyName("rewardAmount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Amount { get; init; }

    /// <summary>How many baby pets the claim gives (saved as <c>Value</c>); null until resolved.</summary>
    [JsonPropertyName("Value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Count { get; init; }

    /// <summary>Global id of the baby pet's table entry (saved as <c>ID</c>); null until resolved.</summary>
    [JsonPropertyName("ID")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DataGlobalId { get; init; }

    /// <summary>Name of the baby pet's table entry as the event names it (saved as <c>reward</c>).</summary>
    [JsonPropertyName("reward")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; init; }

    /// <summary>Shop group the reward is shown with; null when not saved.</summary>
    [JsonPropertyName("shopDisplayGroup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ShopDisplayGroup { get; init; }
}
