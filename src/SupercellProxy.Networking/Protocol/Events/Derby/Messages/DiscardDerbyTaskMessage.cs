using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests resolution of the player's selected task using its current board status.</summary>
public sealed record DiscardDerbyTaskMessage(int TaskBoardIndex, int TaskStatus, LongId NeighborhoodId, LongId PlayerId, bool Discarded) : IMessage
{
    /// <summary>Decodes the selected-task request.</summary>
    public static DiscardDerbyTaskMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        DiscardDerbyTaskMessage message = new(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadLongId(), stream.ReadLongId(), stream.ReadBoolean());
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the selected task and its discard classification.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(TaskBoardIndex);
        stream.WriteVarInt(TaskStatus);
        stream.WriteLongId(NeighborhoodId);
        stream.WriteLongId(PlayerId);
        stream.WriteBoolean(Discarded);
    }

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DiscardDerbyTaskMessage);
    }
}
