using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Notifications;

/// <summary>Represents one retained Valley notification and its presentation count.</summary>
public sealed record MapGameNotificationEntrySnapshot([property: JsonPropertyName("ID")] int NotificationGlobalIdentifier, [property: JsonPropertyName("Value")] int Count);
