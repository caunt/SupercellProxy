using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>One play-limit window of a movie ticket event: how many videos fit in it and how long it lasts.</summary>
public sealed record MovieTicketCycleDefinition
{
    /// <summary>Gets or sets the length of one play-limit window in minutes.</summary>
    /// <value>A length in whole minutes, read from <c>CycleDurationMinutes</c>.</value>
    [JsonPropertyName("CycleDurationMinutes")]
    public int CycleDurationMinutes { get; init; }

    /// <summary>Gets or sets how many videos the player may watch within one window.</summary>
    /// <value>A video count, read from <c>MoviesPerCycle</c>.</value>
    [JsonPropertyName("MoviesPerCycle")]
    public int MoviesPerCycle { get; init; }
}
