using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Represents the decoded MovieTicketCycleDefinition JSON contract.</summary>
public sealed record MovieTicketCycleDefinition
{
    /// <summary>Gets the CycleDurationMinutes value.</summary>
    [JsonPropertyName("CycleDurationMinutes")]
    public int CycleDurationMinutes { get; init; }

    /// <summary>Gets the MoviesPerCycle value.</summary>
    [JsonPropertyName("MoviesPerCycle")]
    public int MoviesPerCycle { get; init; }

}
