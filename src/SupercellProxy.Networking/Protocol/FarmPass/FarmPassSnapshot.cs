using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>
/// Represents decoded <c language="csharp">FarmPassSnapshot</c> home data.
/// </summary>
public sealed record FarmPassSnapshot
{

    /// <summary>
    /// Gets the Daily Tasks value.
    /// </summary>
    [JsonPropertyName("FarmPassDailyTasks")]
    public FarmPassTaskSnapshot[] DailyTasks { get; init; } = [];

    /// <summary>
    /// Gets the Weekly Tasks value.
    /// </summary>
    [JsonPropertyName("FarmPassWeeklyTasks")]
    public FarmPassTaskSnapshot[] WeeklyTasks { get; init; } = [];

    /// <summary>
    /// Gets the Expired Daily Tasks value.
    /// </summary>
    [JsonPropertyName("FarmPassDailyTasksExpired")]
    public FarmPassTaskSnapshot[] ExpiredDailyTasks { get; init; } = [];

    /// <summary>
    /// Gets the Season value.
    /// </summary>
    [JsonPropertyName("FarmPassSeason")]
    public FarmPassSeasonSnapshot? Season { get; init; }
    /// <summary>
    /// Gets the Seen Season Start value.
    /// </summary>
    public int SeenSeasonStart { get; init; }

    /// <summary>
    /// Gets the Trashed Daily Tasks value.
    /// </summary>
    [JsonPropertyName("FarmPassDailyTasksTrashed")]
    public FarmPassTaskSnapshot[] TrashedDailyTasks { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Perks</c> value.
    /// </summary>
    [JsonPropertyName("FarmPassPerks_v1")]
    public FarmPassPerkSnapshot[] Perks { get; init; } = [];
}
