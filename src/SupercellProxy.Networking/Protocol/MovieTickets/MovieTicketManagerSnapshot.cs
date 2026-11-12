using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MovieTickets;

/// <summary>Saved movie ticket state of a home: one entry per movie ticket placement.</summary>
public sealed record MovieTicketManagerSnapshot
{
    /// <summary>Gets or sets the per-placement ticket state, indexed by placement; <see langword="null"/> for placements the home has no state for.</summary>
    /// <value>Saved as <c>tickets</c>; index <c>i</c> belongs to row <c>i</c> of the movie ticket table, which is also the ticket index commands name. Empty by default.</value>
    [JsonPropertyName("tickets")]
    public MovieTicketSnapshot?[] Tickets { get; init; } = [];
}
