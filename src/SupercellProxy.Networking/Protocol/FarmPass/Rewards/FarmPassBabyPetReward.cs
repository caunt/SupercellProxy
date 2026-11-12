using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.FarmPass.Rewards;

/// <summary>
/// One baby pet reward of a Farm Pass level: the event's name and amount, and the resolved global id and count once they are
/// filled in.
/// </summary>
public sealed record FarmPassBabyPetReward : ExtensibleDocument
{
    /// <summary>Gets or sets how many baby pets the event's definition gives (saved as <c>rewardAmount</c>).</summary>
    /// <value>The count from the event's reward entry; <see langword="null"/> when the save omits it.</value>
    [JsonPropertyName("rewardAmount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Amount { get; init; }

    /// <summary>Gets or sets how many baby pets the claim gives (saved as <c>Value</c>); <see langword="null"/> until resolved.</summary>
    /// <value>The resolved baby pet count, written only when present.</value>
    [JsonPropertyName("Value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Count { get; init; }

    /// <summary>Gets or sets the global id of the baby pet's table entry (saved as <c>ID</c>); <see langword="null"/> until resolved.</summary>
    /// <value>A row of the baby pets table, written only when present.</value>
    [JsonPropertyName("ID")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DataGlobalId { get; init; }

    /// <summary>Gets or sets the name of the baby pet's table entry as the event names it (saved as <c>reward</c>).</summary>
    /// <value>A baby pets table row name from the event's reward entry; many saves carry only this name and <see cref="Amount"/>.</value>
    [JsonPropertyName("reward")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; init; }

    /// <summary>Gets or sets the shop group the reward is shown with; <see langword="null"/> when not saved.</summary>
    /// <value>The group index saved as <c>shopDisplayGroup</c>, kept as read and written back only when present.</value>
    [JsonPropertyName("shopDisplayGroup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ShopDisplayGroup { get; init; }
}
