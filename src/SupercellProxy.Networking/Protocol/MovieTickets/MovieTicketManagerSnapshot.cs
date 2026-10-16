using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Saved movie ticket state of a home: one entry per movie ticket placement.</summary>
public sealed record MovieTicketManagerSnapshot
{
    /// <summary>Per-placement ticket state, indexed by placement; null for placements the home has no state for.</summary>
    [JsonPropertyName("tickets")]
    public MovieTicketSnapshot?[] Tickets { get; init; } = [];
}
