using System.Text.Json.Serialization;

using SupercellProxy.Networking.Json;

namespace SupercellProxy.Networking.Protocol.FarmPass.Rewards;

/// <summary>Represents decoded FarmPassBabyPetReward state.</summary>
public sealed record FarmPassBabyPetReward : ExtensibleDocument
{
    /// <summary>Gets the Amount value.</summary>
    [JsonPropertyName("rewardAmount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Amount { get; init; }

    /// <summary>Gets the Count value.</summary>
    [JsonPropertyName("Value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Count { get; init; }

    /// <summary>Gets the DataGlobalIdentifier value.</summary>
    [JsonPropertyName("ID")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DataGlobalIdentifier { get; init; }

    /// <summary>Gets the Name value.</summary>
    [JsonPropertyName("reward")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; init; }

    /// <summary>Gets the ShopDisplayGroup value.</summary>
    [JsonPropertyName("shopDisplayGroup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ShopDisplayGroup { get; init; }

}
