using SupercellProxy.Networking.Protocol.FarmPass.Rewards;

using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Saved state of one Farm Pass level: the points it takes and its rewards with their collected flags.</summary>
public sealed record FarmPassLevelSnapshot
{
    /// <summary>Season points this level takes on top of the previous level.</summary>
    [JsonPropertyName("farmPoints")]
    public int FarmPoints { get; init; }

    /// <summary>Rewards every player gets at this level.</summary>
    [JsonPropertyName("freeRewards")]
    public FarmPassRewardGroup[] FreeRewards { get; init; } = [];

    /// <summary>Rewards only players who bought the premium pass get at this level.</summary>
    [JsonPropertyName("premiumRewards")]
    public FarmPassRewardGroup[] PremiumRewards { get; init; } = [];

    /// <summary>Position of the level in the season, starting at 0.</summary>
    [JsonPropertyName("level")]
    public int Level { get; init; }

    /// <summary>Total season points at which the level is reached, which also identifies it.</summary>
    [JsonPropertyName("levelId")]
    public int LevelId { get; init; }

    /// <summary>Whether the player has seen the level's unlock.</summary>
    [JsonPropertyName("levelUnlockSeen")]
    public bool LevelUnlockSeen { get; init; }

    /// <summary>Baby pets every player gets at this level.</summary>
    [JsonPropertyName("freeBabyPetRewards")]
    public FarmPassBabyPetRewardGroup[] FreeBabyPetRewards { get; init; } = [];

    /// <summary>Baby pets only players who bought the premium pass get at this level.</summary>
    [JsonPropertyName("premiumBabyPetRewards")]
    public FarmPassBabyPetRewardGroup[] PremiumBabyPetRewards { get; init; } = [];
}
