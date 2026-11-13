using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Saved state of the player's part in the neighbourhood event: which event it belongs to and its progress.</summary>
public sealed record NeighborhoodObjectManagerSnapshot
{
    /// <summary>Gets or sets the live event the saved progress belongs to.</summary>
    /// <value>The id of the neighbourhood live event (type 55) the player joined, or 0 when no event has started for them.</value>
    [JsonPropertyName("ActiveEventId")]
    public int ActiveEventId { get; init; }

    /// <summary>Gets or sets the last neighborhood used by this event manager.</summary>
    /// <value>The neighbourhood id as a 64-bit number, or 0 when the player is in none.</value>
    public long LastKnownNeighborhoodId { get; init; }

    /// <summary>Gets or sets the player's tasks, points, rewards and perks in that event.</summary>
    /// <value>Never <see langword="null"/>; a new manager starts with an empty state.</value>
    public NeighborhoodObjectStateSnapshot State { get; init; } = new();
}
