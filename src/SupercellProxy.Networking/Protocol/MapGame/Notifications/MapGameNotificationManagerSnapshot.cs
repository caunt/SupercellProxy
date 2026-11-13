using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Notifications;

/// <summary>Represents retained pending Valley notifications.</summary>
public sealed record MapGameNotificationManagerSnapshot
{
    /// <summary>Gets or sets the pending notifications in native presentation order.</summary>
    /// <value>At most one entry per notification row, oldest first. Empty by default.</value>
    [JsonPropertyName("Notifications")]
    public ImmutableArray<MapGameNotificationEntrySnapshot> Notifications { get; init; } = [];
}
