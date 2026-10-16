using SupercellProxy.Networking.Protocol.Timing;

using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Saved newspaper state: today's purchase counters and the refresh timer.
/// </summary>
public sealed record NewspaperSnapshot
{
    /// <summary>
    /// When the counter of purchases from the newspaper bot resets, as a saved time value.
    /// </summary>
    [JsonPropertyName("botResetTime")]
    public int BotResetTime { get; init; }

    /// <summary>
    /// How many collection tools, town tools excluded, the player bought through roadside shops today, as native counts them.
    /// </summary>
    [JsonPropertyName("boughtCollectionToolExcludingTownFromRssToday")]
    public int BoughtCollectionToolExcludingTownFromRssToday { get; init; }

    /// <summary>
    /// How many purchases the player made from the newspaper bot today.
    /// </summary>
    [JsonPropertyName("boughtFromBotToday")]
    public int BoughtFromBotToday { get; init; }
    /// <summary>
    /// Countdown until the next free newspaper refresh.
    /// </summary>
    [JsonPropertyName("timer")]
    public TimerSnapshot? Timer { get; init; }
}
