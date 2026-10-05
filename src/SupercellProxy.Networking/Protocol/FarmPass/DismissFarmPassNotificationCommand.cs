using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.FarmPass;

/// <summary>
/// Defines the Dismiss Farm Pass Notification Command contract.
/// </summary>
/// <summary>
/// Defines the Notification Global Id contract.
/// </summary>
public sealed record DismissFarmPassNotificationCommand([property: System.Text.Json.Serialization.JsonPropertyName("NotificationGlobalId")] int NotificationGlobalId) : Command
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.DismissFarmPassNotificationCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static DismissFarmPassNotificationCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int notification = stream.ReadVarInt();

        return new DismissFarmPassNotificationCommand(notification);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(NotificationGlobalId);
    }
}
