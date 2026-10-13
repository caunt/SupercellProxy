using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Reserves a board task for the player; the server command starts the accepted task.</summary>
public sealed record TakeDerbyTaskMessage(int TaskBoardIndex, bool RestoreActiveTask, LongId NeighborhoodId, LongId PlayerId) : CodecMessage<TakeDerbyTaskMessage>, ICodecProvider<TakeDerbyTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<TakeDerbyTaskMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.TaskBoardIndex,
            Codecs.Boolean,
            static message => message.RestoreActiveTask,
            Codecs.LongId,
            static message => message.NeighborhoodId,
            Codecs.LongId,
            static message => message.PlayerId,
            static (taskBoardIndex, restoreActiveTask, neighborhoodId, playerId) => new TakeDerbyTaskMessage(taskBoardIndex, restoreActiveTask, neighborhoodId, playerId)
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(TakeDerbyTaskMessage);
    }
}
