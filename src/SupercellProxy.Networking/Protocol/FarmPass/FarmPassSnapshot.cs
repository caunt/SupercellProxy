using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Saved Farm Pass state of a home: its daily and weekly tasks, the joined season, and claimed perks.</summary>
public sealed record FarmPassSnapshot
{
    /// <summary>Today's Farm Pass tasks.</summary>
    [JsonPropertyName("FarmPassDailyTasks")]
    public FarmPassTaskSnapshot[] DailyTasks { get; init; } = [];

    /// <summary>This season's weekly Farm Pass tasks, listed group after group.</summary>
    [JsonPropertyName("FarmPassWeeklyTasks")]
    public FarmPassTaskSnapshot[] WeeklyTasks { get; init; } = [];

    /// <summary>Gets the hour index (hours since 1970) of the next daily task reset, as the daily reset timer saved it.</summary>
    public int NextExpireHourIndex { get; init; }

    /// <summary>Yesterday's daily tasks, which new daily tasks avoid repeating.</summary>
    [JsonPropertyName("FarmPassDailyTasksExpired")]
    public FarmPassTaskSnapshot[] ExpiredDailyTasks { get; init; } = [];

    /// <summary>The season the player joined; null before the first season.</summary>
    [JsonPropertyName("FarmPassSeason")]
    public FarmPassSeasonSnapshot? Season { get; init; }

    /// <summary>Id of the last live event whose season start the player has seen; 0 when none.</summary>
    public int SeenSeasonStart { get; init; }

    /// <summary>Daily tasks the player threw away today, which their replacements avoid repeating.</summary>
    [JsonPropertyName("FarmPassDailyTasksTrashed")]
    public FarmPassTaskSnapshot[] TrashedDailyTasks { get; init; } = [];

    /// <summary>Perks the player claimed from the pass, active or not.</summary>
    [JsonPropertyName("FarmPassPerks_v1")]
    public FarmPassPerkSnapshot[] Perks { get; init; } = [];
}
