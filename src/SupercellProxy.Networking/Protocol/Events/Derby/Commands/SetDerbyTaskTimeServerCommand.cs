using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Replaces the remaining seconds of the active derby task timer.</summary>
public sealed record SetDerbyTaskTimeServerCommand(int RemainingSeconds) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SetDerbyTaskTimeServerCommandType;

    /// <summary>Decodes the remaining seconds before server-command metadata.</summary>
    public static SetDerbyTaskTimeServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(RemainingSeconds);
    }
}
