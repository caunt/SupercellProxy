using SupercellProxy.Networking.Protocol.Timing;

using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Defines the Newspaper Snapshot contract.
/// </summary>
public sealed record NewspaperSnapshot
{

    /// <summary>
    /// Gets the Bot Reset Time value.
    /// </summary>
    [JsonPropertyName("botResetTime")]
    public int BotResetTime { get; init; }

    /// <summary>
    /// Gets the Bought Collection Tool Excluding Town From Rss Today value.
    /// </summary>
    [JsonPropertyName("boughtCollectionToolExcludingTownFromRssToday")]
    public int BoughtCollectionToolExcludingTownFromRssToday { get; init; }

    /// <summary>
    /// Gets the Bought From Bot Today value.
    /// </summary>
    [JsonPropertyName("boughtFromBotToday")]
    public int BoughtFromBotToday { get; init; }
    /// <summary>
    /// Gets the Timer value.
    /// </summary>
    [JsonPropertyName("timer")]
    public TimerSnapshot? Timer { get; init; }
}
