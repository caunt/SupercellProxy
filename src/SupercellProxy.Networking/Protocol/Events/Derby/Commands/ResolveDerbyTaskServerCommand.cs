using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports the result of completing or discarding a player's derby task.</summary>
public sealed record ResolveDerbyTaskServerCommand(LongId NeighborhoodId, LongId PlayerId, int TaskBoardIndex, int Status, bool Discarded, int ResultValue, bool SeenExpiredTask) : CodecServerCommand<ResolveDerbyTaskServerCommand>, ICommandCodecProvider<ResolveDerbyTaskServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ResolveDerbyTaskServerCommand> Codec { get; } = Codecs.Command(
        type: 171,
        Codecs.Composite(
            Codecs.LongId,
            static command => command.NeighborhoodId,
            Codecs.LongId,
            static command => command.PlayerId,
            Codecs.VarInt,
            static command => command.TaskBoardIndex,
            Codecs.VarInt,
            static command => command.Status,
            Codecs.Boolean,
            static command => command.Discarded,
            Codecs.VarInt,
            static command => command.ResultValue,
            Codecs.Boolean,
            static command => command.SeenExpiredTask,
            static (neighborhoodId, playerId, taskBoardIndex, status, discarded, resultValue, seenExpiredTask) => new ResolveDerbyTaskServerCommand(neighborhoodId, playerId, taskBoardIndex, status, discarded, resultValue, seenExpiredTask)
        )
    );
}
