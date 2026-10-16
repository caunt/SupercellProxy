using System.Text.Json.Serialization;
namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Saved state of the player's part in the neighbourhood event: which event it belongs to and its progress.</summary>
public sealed record NeighborhoodObjectManagerSnapshot
{
    /// <summary>Live event the saved progress belongs to; progress of another event starts over.</summary>
    [JsonPropertyName("ActiveEventId")]
    public int ActiveEventId { get; init; }
    /// <summary>Gets the last neighborhood used by this event manager.</summary>
    public long LastKnownNeighborhoodId { get; init; }

    /// <summary>The player's tasks, points, rewards and perks in that event.</summary>
    public NeighborhoodObjectStateSnapshot State { get; init; } = new();
}
