using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Boards;

/// <summary>Toggles the liked state shared by events of the selected event's type.</summary>
public sealed record ToggleEventTypeLikeCommand(int EventId) : Command
{
    /// <summary>Gets the version-independent command identity.</summary>
    public override int Type => CommandRegistry.ToggleEventTypeLikeCommandType;

    /// <summary>Decodes the event instance following base command metadata.</summary>
    public static ToggleEventTypeLikeCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new ToggleEventTypeLikeCommand(stream.ReadVarInt());
    }

    /// <summary>Encodes the event instance identifier.</summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(EventId);
    }
}
