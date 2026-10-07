using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Sets the effective derby opt-in preference for the matching neighborhood.</summary>
public sealed record SetDerbyParticipationServerCommand(LongId NeighborhoodId, LongId ActorId, bool Enabled) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SetDerbyParticipationServerCommandType;

    /// <summary>Decodes the participation update before its server-command metadata.</summary>
    public static SetDerbyParticipationServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadLongId(), stream.ReadLongId(), stream.ReadBoolean());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(NeighborhoodId);
        stream.WriteLongId(ActorId);
        stream.WriteBoolean(Enabled);
    }
}
