using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports the result of completing or discarding a player's derby task.</summary>
/// <param name="NeighborhoodId">The neighbourhood of the task, the first field.</param>
/// <param name="PlayerId">The avatar id of the player whose task was resolved.</param>
/// <param name="TaskBoardIndex">The task-board slot of the resolved task.</param>
/// <param name="Status">The result code of the request; 1 marks success.</param>
/// <param name="Discarded"><see langword="true"/> if the task was discarded; <see langword="false"/> if it was finished.</param>
/// <param name="ResultValue">An unnamed integer sent after <paramref name="Discarded"/>; its meaning is not yet identified.</param>
/// <param name="SeenExpiredTask"><see langword="true"/> if the player already saw the task expire; otherwise, <see langword="false"/>.</param>
public sealed record ResolveDerbyTaskServerCommand(LongId NeighborhoodId, LongId PlayerId, int TaskBoardIndex, int Status, bool Discarded, int ResultValue, bool SeenExpiredTask) : CodecServerCommand<ResolveDerbyTaskServerCommand>, ICommandCodecProvider<ResolveDerbyTaskServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ResolveDerbyTaskServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.LongId,
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
        static (neighborhoodId, playerId, taskBoardIndex, status, discarded, resultValue, seenExpiredTask) => new ResolveDerbyTaskServerCommand(neighborhoodId, playerId, taskBoardIndex, status, discarded, resultValue, seenExpiredTask)));
}
