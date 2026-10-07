using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports successful help that advances the requesting player's derby task.</summary>
public sealed record RecordDerbyHelpServerCommand(LongId NeighborhoodId, LongId PlayerId, int Status) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.RecordDerbyHelpServerCommandType;

    /// <summary>Decodes the help result before server-command metadata.</summary>
    public static RecordDerbyHelpServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadLongId(), stream.ReadLongId(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(NeighborhoodId);
        stream.WriteLongId(PlayerId);
        stream.WriteVarInt(Status);
    }
}
