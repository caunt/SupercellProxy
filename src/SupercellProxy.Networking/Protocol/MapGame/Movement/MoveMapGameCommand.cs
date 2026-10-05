using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Movement;

/// <summary>Requests movement of the local Valley pawn from its current node toward another map node.</summary>
public sealed record MoveMapGameCommand(int SourceNodeId, int TargetNodeId) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.MoveMapGameCommandType;

    /// <summary>Decodes a command from its native wire representation.</summary>
    public static MoveMapGameCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MoveMapGameCommand(stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(SourceNodeId);
        stream.WriteVarInt(TargetNodeId);
    }
}
