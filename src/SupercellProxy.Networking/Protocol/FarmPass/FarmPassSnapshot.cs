using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>Saved Farm Pass state of a home: its daily and weekly tasks, the joined season, and claimed perks.</summary>
public sealed record FarmPassSnapshot
{
    /// <summary>Gets or sets today's Farm Pass tasks.</summary>
    /// <value>The daily tasks of the current day, saved as <c>FarmPassDailyTasks</c>; empty by default.</value>
    [JsonPropertyName("FarmPassDailyTasks")]
    public ImmutableArray<FarmPassTaskSnapshot> DailyTasks { get; init; } = [];

    /// <summary>Gets or sets the id of the Farm Pass live event the manager runs; 0 when none was saved.</summary>
    /// <value>A Farm Pass live event id.</value>
    public int Event { get; init; }

    /// <summary>Gets or sets this season's weekly Farm Pass tasks, listed group after group.</summary>
    /// <value>Every weekly group since the event started, each holding its weekly tasks plus any extra diamond tasks.</value>
    [JsonPropertyName("FarmPassWeeklyTasks")]
    public ImmutableArray<FarmPassTaskSnapshot> WeeklyTasks { get; init; } = [];

    /// <summary>Gets or sets the hour index (hours since 1970) of the next daily task reset, as the daily reset timer saved it.</summary>
    /// <value>A UTC hour count whose hour of day should be the configured reset hour; 0 means no timer was saved.</value>
    public int NextExpireHourIndex { get; init; }

    /// <summary>Gets or sets yesterday's daily tasks.</summary>
    /// <value>The daily tasks that were current before the last reset; empty by default.</value>
    [JsonPropertyName("FarmPassDailyTasksExpired")]
    public ImmutableArray<FarmPassTaskSnapshot> ExpiredDailyTasks { get; init; } = [];

    /// <summary>Gets or sets the season the player joined; <see langword="null"/> before the first season.</summary>
    /// <value>The joined season's levels, points and purchases, saved as <c>FarmPassSeason</c>.</value>
    [JsonPropertyName("FarmPassSeason")]
    public FarmPassSeasonSnapshot? Season { get; init; }

    /// <summary>Gets or sets the id of the last live event whose season start the player has seen; 0 when none.</summary>
    /// <value>A Farm Pass live event id; -1 while a running event's season start is unseen.</value>
    public int SeenSeasonStart { get; init; }

    /// <summary>Gets or sets the daily tasks the player threw away today.</summary>
    /// <value>The discarded daily tasks since the last reset; empty by default.</value>
    [JsonPropertyName("FarmPassDailyTasksTrashed")]
    public ImmutableArray<FarmPassTaskSnapshot> TrashedDailyTasks { get; init; } = [];

    /// <summary>Gets or sets the perks the player claimed from the pass, active or not.</summary>
    /// <value>One entry per claimed perk, saved as <c>FarmPassPerks_v1</c>; empty by default.</value>
    [JsonPropertyName("FarmPassPerks_v1")]
    public ImmutableArray<FarmPassPerkSnapshot> Perks { get; init; } = [];
}
