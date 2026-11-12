using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Notifications;

/// <summary>Represents one retained Valley notification and its presentation count.</summary>
/// <param name="NotificationGlobalId">Global id of the notification's row in the Valley notifications table, saved as <c>ID</c>.</param>
/// <param name="Count">The notification's accumulated amount, saved as <c>Value</c>.</param>
public sealed record MapGameNotificationEntrySnapshot([property: JsonPropertyName("ID")] int NotificationGlobalId, [property: JsonPropertyName("Value")] int Count);
