using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Notifications;

/// <summary>Represents retained pending Valley notifications.</summary>
public sealed record MapGameNotificationManagerSnapshot
{
    /// <summary>Gets the pending notifications in native presentation order.</summary>
    [JsonPropertyName("Notifications")]
    public MapGameNotificationEntrySnapshot[] Notifications { get; init; } = [];
}
