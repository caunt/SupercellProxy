using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Resolves a timed task only when the reply still describes the active task.</summary>
public sealed record ExpireDerbyTaskServerCommand(
    LongId NeighborhoodId,
    LongId PlayerId,
    int TaskBoardIndex,
    int Status,
    bool Discarded,
    int Points,
    int DurationMinutes,
    int TaskGlobalId
) : CodecServerCommand<ExpireDerbyTaskServerCommand>, ICommandCodecProvider<ExpireDerbyTaskServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ExpireDerbyTaskServerCommand> Codec { get; } = Codecs.Command(
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
            static command => command.Points,
            Codecs.VarInt,
            static command => command.DurationMinutes,
            Codecs.VarInt,
            static command => command.TaskGlobalId,
            static (neighborhoodId, playerId, taskBoardIndex, status, discarded, points, durationMinutes, taskGlobalId) => new ExpireDerbyTaskServerCommand(neighborhoodId, playerId, taskBoardIndex, status, discarded, points, durationMinutes, taskGlobalId)
        )
    );
}
