using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>One play-limit window of a movie ticket event: how many videos fit in it and how long it lasts.</summary>
public sealed record MovieTicketCycleDefinition
{
    /// <summary>Length of one play-limit window in minutes; when it ends, the window's video count starts over.</summary>
    [JsonPropertyName("CycleDurationMinutes")]
    public int CycleDurationMinutes { get; init; }

    /// <summary>How many videos the player may watch within one window.</summary>
    [JsonPropertyName("MoviesPerCycle")]
    public int MoviesPerCycle { get; init; }
}
