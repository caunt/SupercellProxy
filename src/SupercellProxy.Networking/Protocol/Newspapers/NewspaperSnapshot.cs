using SupercellProxy.Networking.Protocol.Timing;

using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Saved newspaper state: today's purchase counters and the refresh timer.
/// </summary>
public sealed record NewspaperSnapshot
{
    /// <summary>
    /// Gets or sets when the counter of purchases from the newspaper bot resets, as a saved time value.
    /// </summary>
    /// <value>Saved as <c>botResetTime</c>; the unit of the stored time is not confirmed, so treat it as an opaque value to carry over unchanged.</value>
    [JsonPropertyName("botResetTime")]
    public int BotResetTime { get; init; }

    /// <summary>
    /// Gets or sets how many collection tools, town tools excluded, the player bought through roadside shops today.
    /// </summary>
    /// <value>Saved as <c>boughtCollectionToolExcludingTownFromRssToday</c>; a daily count that starts at 0.</value>
    [JsonPropertyName("boughtCollectionToolExcludingTownFromRssToday")]
    public int BoughtCollectionToolExcludingTownFromRssToday { get; init; }

    /// <summary>
    /// Gets or sets how many purchases the player made from the newspaper bot today.
    /// </summary>
    /// <value>Saved as <c>boughtFromBotToday</c>; a daily count that starts at 0.</value>
    [JsonPropertyName("boughtFromBotToday")]
    public int BoughtFromBotToday { get; init; }
    /// <summary>
    /// Gets or sets the countdown until the next free newspaper refresh.
    /// </summary>
    /// <value>
    /// Saved as <c>timer</c>; it counts down in timer ticks.
    /// <see langword="null"/> or a finished timer means a free refresh is available.
    /// </value>
    [JsonPropertyName("timer")]
    public TimerSnapshot? Timer { get; init; }
}
