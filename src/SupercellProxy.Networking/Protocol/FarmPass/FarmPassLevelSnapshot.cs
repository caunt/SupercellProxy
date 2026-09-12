using SupercellProxy.Networking.Protocol.FarmPass.Rewards;

using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>
/// Defines the Farm Pass Level Snapshot contract.
/// </summary>
public sealed record FarmPassLevelSnapshot
{

    /// <summary>
    /// Gets the Farm Points value.
    /// </summary>
    [JsonPropertyName("farmPoints")]
    public int FarmPoints { get; init; }

    /// <summary>
    /// Gets the Free Rewards value.
    /// </summary>
    [JsonPropertyName("freeRewards")]
    public FarmPassRewardGroup[] FreeRewards { get; init; } = [];

    /// <summary>
    /// Gets the Premium Rewards value.
    /// </summary>
    [JsonPropertyName("premiumRewards")]
    public FarmPassRewardGroup[] PremiumRewards { get; init; } = [];
    /// <summary>
    /// Gets the Level value.
    /// </summary>
    [JsonPropertyName("level")]
    public int Level { get; init; }

    /// <summary>
    /// Gets the Level Id value.
    /// </summary>
    [JsonPropertyName("levelId")]
    public int LevelIdentifier { get; init; }

    /// <summary>
    /// Gets the Level Unlock Seen value.
    /// </summary>
    [JsonPropertyName("levelUnlockSeen")]
    public bool LevelUnlockSeen { get; init; }

    /// <summary>
    /// Gets the Free Baby Pet Rewards value.
    /// </summary>
    [JsonPropertyName("freeBabyPetRewards")]
    public FarmPassBabyPetRewardGroup[] FreeBabyPetRewards { get; init; } = [];

    /// <summary>
    /// Gets the Premium Baby Pet Rewards value.
    /// </summary>
    [JsonPropertyName("premiumBabyPetRewards")]
    public FarmPassBabyPetRewardGroup[] PremiumBabyPetRewards { get; init; } = [];
}
