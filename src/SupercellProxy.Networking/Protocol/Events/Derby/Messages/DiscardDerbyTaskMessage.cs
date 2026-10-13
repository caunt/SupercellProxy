using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests resolution of the player's selected task using its current board status.</summary>
public sealed record DiscardDerbyTaskMessage(int TaskBoardIndex, int TaskStatus, LongId NeighborhoodId, LongId PlayerId, bool Discarded) : CodecMessage<DiscardDerbyTaskMessage>, ICodecProvider<DiscardDerbyTaskMessage>
{
    /// <inheritdoc />
    public static ICodec<DiscardDerbyTaskMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.TaskBoardIndex,
            Codecs.VarInt,
            static message => message.TaskStatus,
            Codecs.LongId,
            static message => message.NeighborhoodId,
            Codecs.LongId,
            static message => message.PlayerId,
            Codecs.Boolean,
            static message => message.Discarded,
            static (taskBoardIndex, taskStatus, neighborhoodId, playerId, discarded) => new DiscardDerbyTaskMessage(taskBoardIndex, taskStatus, neighborhoodId, playerId, discarded)
        ),
        trailingDataMessage: "The derby message has trailing data."
    );

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DiscardDerbyTaskMessage);
    }
}
