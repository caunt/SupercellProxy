using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MiniPass;

/// <summary>Saved identity and progress for the current Mini Pass.</summary>
public sealed record MiniPassInstanceSnapshot
{
    /// <summary>Gets the Mini Pass data row.</summary>
    [JsonPropertyName("DataId")]
    public int DataGlobalId { get; init; }

    /// <summary>Gets the event configuration the pass was started from.</summary>
    [JsonPropertyName("EventConfig")]
    public MiniPassEventConfigurationSnapshot? EventConfiguration { get; init; }

    /// <summary>Gets the Mini Pass event id.</summary>
    [JsonPropertyName("EventId")]
    public int EventId { get; init; }

    /// <summary>Gets the free reward track.</summary>
    public MiniPassRewardTrackSnapshot? FreeRewards { get; init; }

    /// <summary>Gets the pass duration in seconds, counted from <see cref="StartTime" />.</summary>
    public int PersonalDuration { get; init; }

    /// <summary>Gets accumulated pass points.</summary>
    public int Points { get; init; }

    /// <summary>Gets the points each reward index requires.</summary>
    public int[] PointsRequired { get; init; } = [];

    /// <summary>Gets the premium reward track.</summary>
    public MiniPassRewardTrackSnapshot? PremiumRewards { get; init; }

    /// <summary>Gets whether the premium track is unlocked.</summary>
    public bool PremiumUnlocked { get; init; }

    /// <summary>Gets the last points amount acknowledged by the player.</summary>
    public int SeenPoints { get; init; }

    /// <summary>Gets the pass start time in Unix seconds.</summary>
    public int StartTime { get; init; }
}
