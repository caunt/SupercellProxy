using SupercellProxy.Networking.Protocol.FarmPass.Rewards;

using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Saved state of one Farm Pass level: the points it takes and its rewards with their collected flags.</summary>
public sealed record FarmPassLevelSnapshot
{
    /// <summary>Gets or sets the season points this level takes on top of the previous level.</summary>
    /// <value>The difference between this level's and the previous level's point totals; summing it over the levels up to one gives that level's point total.</value>
    [JsonPropertyName("farmPoints")]
    public int FarmPoints { get; init; }

    /// <summary>Gets or sets the rewards every player gets at this level.</summary>
    /// <value>The reward groups saved for the free track; it can be empty.</value>
    [JsonPropertyName("freeRewards")]
    public FarmPassRewardGroup[] FreeRewards { get; init; } = [];

    /// <summary>Gets or sets the rewards only players who bought the premium pass get at this level.</summary>
    /// <value>The reward groups saved for the premium track; it can be empty.</value>
    [JsonPropertyName("premiumRewards")]
    public FarmPassRewardGroup[] PremiumRewards { get; init; } = [];

    /// <summary>Gets or sets the position of the level in the season, starting at 0.</summary>
    /// <value>The level's index in the season's level list, which is also the index the baby pet claim command sends.</value>
    [JsonPropertyName("level")]
    public int Level { get; init; }

    /// <summary>Gets or sets the total season points at which the level is reached, which also identifies it.</summary>
    /// <value>The tier's <c>farmPoints</c> from the event definition, saved as <c>levelId</c>.</value>
    [JsonPropertyName("levelId")]
    public int LevelId { get; init; }

    /// <summary>Gets or sets whether the player has seen the level's unlock.</summary>
    /// <value><see langword="true"/> if the client has shown the player that this level was unlocked; otherwise, <see langword="false"/>.</value>
    [JsonPropertyName("levelUnlockSeen")]
    public bool LevelUnlockSeen { get; init; }

    /// <summary>Gets or sets the baby pets every player gets at this level.</summary>
    /// <value>The free baby pet reward groups as the event names them; empty when the level gives no free baby pet.</value>
    [JsonPropertyName("freeBabyPetRewards")]
    public FarmPassBabyPetRewardGroup[] FreeBabyPetRewards { get; init; } = [];

    /// <summary>Gets or sets the baby pets only players who bought the premium pass get at this level.</summary>
    /// <value>The premium baby pet reward groups as the event names them; empty when the level gives no premium baby pet.</value>
    [JsonPropertyName("premiumBabyPetRewards")]
    public FarmPassBabyPetRewardGroup[] PremiumBabyPetRewards { get; init; } = [];
}
