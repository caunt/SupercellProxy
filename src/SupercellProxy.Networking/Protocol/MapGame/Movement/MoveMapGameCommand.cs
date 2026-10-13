using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Movement;

/// <summary>Requests movement of the local Valley pawn from its current node toward another map node.</summary>
public sealed record MoveMapGameCommand(int SourceNodeId, int TargetNodeId) : CodecCommand<MoveMapGameCommand>, ICommandCodecProvider<MoveMapGameCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MoveMapGameCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.SourceNodeId,
            Codecs.VarInt,
            static command => command.TargetNodeId,
            static (sourceNodeId, targetNodeId) => new MoveMapGameCommand(sourceNodeId, targetNodeId)
        ),
        CommandLayout.MetadataFirst
    );
}
