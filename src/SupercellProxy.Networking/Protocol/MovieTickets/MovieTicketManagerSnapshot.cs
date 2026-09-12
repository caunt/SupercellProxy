using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>
/// Defines the Movie Ticket Manager Snapshot contract.
/// </summary>
public sealed record MovieTicketManagerSnapshot
{
    /// <summary>
    /// Gets the Tickets value.
    /// </summary>
    [JsonPropertyName("tickets")]
    public MovieTicketSnapshot?[] Tickets { get; init; } = [];
}
