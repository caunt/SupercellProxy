using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests removal of an available task from the neighborhood board.</summary>
public sealed record TrashDerbyBoardTaskMessage(LongId NeighborhoodId, LongId PlayerId, int TaskBoardIndex) : IMessage
{
    /// <summary>Decodes the board-task removal request.</summary>
    public static TrashDerbyBoardTaskMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        TrashDerbyBoardTaskMessage message = new(stream.ReadLongId(), stream.ReadLongId(), stream.ReadVarInt());
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the board-task removal request.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteLongId(NeighborhoodId);
        stream.WriteLongId(PlayerId);
        stream.WriteVarInt(TaskBoardIndex);
    }

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(TrashDerbyBoardTaskMessage);
    }
}
