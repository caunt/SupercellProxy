using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Reserves a board task for the player; the server command starts the accepted task.</summary>
/// <param name="TaskBoardIndex">The zero-based board slot of the task to take.</param>
/// <param name="RestoreActiveTask"><see langword="true"/> if the request restores the task the player already has active rather than taking a new one; otherwise, <see langword="false"/>.</param>
/// <param name="NeighborhoodId">The identifier of the neighborhood whose board holds the task.</param>
/// <param name="PlayerId">The account identifier of the player taking the task.</param>
public sealed record TakeDerbyTaskMessage(int TaskBoardIndex, bool RestoreActiveTask, LongId NeighborhoodId, LongId PlayerId) : CodecMessage<TakeDerbyTaskMessage>, ICodecProvider<TakeDerbyTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<TakeDerbyTaskMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.VarInt,
            static message => message.TaskBoardIndex,
            Codecs.Boolean,
            static message => message.RestoreActiveTask,
            Codecs.LongId,
            static message => message.NeighborhoodId,
            Codecs.LongId,
            static message => message.PlayerId,
            static (taskBoardIndex, restoreActiveTask, neighborhoodId, playerId) => new TakeDerbyTaskMessage(taskBoardIndex, restoreActiveTask, neighborhoodId, playerId)),
        trailingDataMessage: "The derby message has trailing data.");

    /// <summary>Omits identifiers from diagnostic text.</summary>
    /// <returns>The type name <c>TakeDerbyTaskMessage</c> alone, so the neighborhood and player identifiers never reach logs.</returns>
    public override string ToString()
    {
        return nameof(TakeDerbyTaskMessage);
    }
}
