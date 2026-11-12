using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved identity and progress for the current Mini Pass.</summary>
public sealed record MiniPassInstanceSnapshot
{
    /// <summary>Gets or sets whether the player has seen the pass's completion celebration.</summary>
    /// <value><see langword="true"/> if a <see cref="SetMiniPassStateFlagsCommand"/> has set flag bit 1 (value 2); otherwise, <see langword="false"/>.</value>
    public bool CelebrationSeen { get; init; }

    /// <summary>Gets or sets the Mini Pass data row.</summary>
    /// <value>Saved as <c>DataId</c>; the global id of the pass's row in the Mini Pass table, or 0 when unset.</value>
    [JsonPropertyName("DataId")]
    public int DataGlobalId { get; init; }

    /// <summary>Gets or sets whether the player has finished the pass's end screen.</summary>
    /// <value><see langword="true"/> if a <see cref="SetMiniPassStateFlagsCommand"/> has set flag bit 2 (value 4); otherwise, <see langword="false"/>.</value>
    public bool EndScreenFinished { get; init; }

    /// <summary>Gets or sets the event configuration the pass was started from.</summary>
    /// <value>Saved as <c>EventConfig</c>; the pass type and title copied from the live event, or <see langword="null"/> when the save has none.</value>
    [JsonPropertyName("EventConfig")]
    public MiniPassEventConfigurationSnapshot? EventConfiguration { get; init; }

    /// <summary>Gets or sets the Mini Pass event id.</summary>
    /// <value>The id of the live event that started this pass, or 0 when unset.</value>
    [JsonPropertyName("EventId")]
    public int EventId { get; init; }

    /// <summary>Gets or sets the free reward track.</summary>
    /// <value>The rewards of the free track, with which of them are claimed; <see langword="null"/> means no free rewards.</value>
    public MiniPassRewardTrackSnapshot? FreeRewards { get; init; }

    /// <summary>Gets or sets the pass duration in seconds, counted from <see cref="StartTime" />.</summary>
    /// <value>Length of this player's pass in seconds; the pass ends at <c>StartTime + PersonalDuration</c>.</value>
    public int PersonalDuration { get; init; }

    /// <summary>Gets or sets accumulated pass points.</summary>
    /// <value>Points earned from completed pass tasks.</value>
    public int Points { get; init; }

    /// <summary>Gets or sets the points each reward index requires.</summary>
    /// <value>The cumulative points needed for reward index <c>i</c> at position <c>i</c>; the last entry is the points that complete the pass.</value>
    public int[] PointsRequired { get; init; } = [];

    /// <summary>Gets or sets the premium reward track.</summary>
    /// <value>The rewards of the premium track, with which of them are claimed; <see langword="null"/> means no premium rewards.</value>
    public MiniPassRewardTrackSnapshot? PremiumRewards { get; init; }

    /// <summary>Gets or sets whether the premium track is unlocked.</summary>
    /// <value><see langword="true"/> if the player bought the premium track; otherwise, <see langword="false"/>.</value>
    public bool PremiumUnlocked { get; init; }

    /// <summary>Gets or sets the last points amount acknowledged by the player.</summary>
    /// <value>The points the finale animation last played up to, set by <see cref="PlayMiniPassFinaleCommand"/>; 0 before it first plays.</value>
    public int SeenPoints { get; init; }

    /// <summary>Gets or sets the pass start time in Unix seconds.</summary>
    /// <value>Server time, in seconds since 1970-01-01 UTC, at which the pass began; <see cref="PersonalDuration"/> counts from it.</value>
    public int StartTime { get; init; }

    /// <summary>Gets or sets whether the player has seen the pass's introduction.</summary>
    /// <value><see langword="true"/> if a <see cref="SetMiniPassStateFlagsCommand"/> has set flag bit 0 (value 1); otherwise, <see langword="false"/>.</value>
    public bool TutorialSeen { get; init; }
}
