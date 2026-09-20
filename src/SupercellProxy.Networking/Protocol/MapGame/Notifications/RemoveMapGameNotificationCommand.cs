using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Notifications;

/// <summary>Removes one pending Valley notification after it has been presented.</summary>
public sealed record RemoveMapGameNotificationCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("NotificationGlobalId")] int NotificationGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.RemoveMapGameNotificationCommandType;

    /// <summary>Decodes a command from its native wire representation.</summary>
    public static RemoveMapGameNotificationCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int notificationGlobalIdentifier = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields = DecodeCommand(stream, environment);

        return new RemoveMapGameNotificationCommand(notificationGlobalIdentifier, commandFields.ExecutionPhaseCounter, commandFields.DebugData0, commandFields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(NotificationGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
