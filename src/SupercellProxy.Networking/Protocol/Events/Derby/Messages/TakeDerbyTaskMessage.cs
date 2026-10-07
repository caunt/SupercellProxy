using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Reserves a board task for the player; the server command starts the accepted task.</summary>
public sealed record TakeDerbyTaskMessage(int TaskBoardIndex, bool RestoreActiveTask, LongId NeighborhoodId, LongId PlayerId) : IMessage
{
    /// <summary>Decodes the reservation request.</summary>
    public static TakeDerbyTaskMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        TakeDerbyTaskMessage message = new(stream.ReadVarInt(), stream.ReadBoolean(), stream.ReadLongId(), stream.ReadLongId());
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the reservation request in native order.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(TaskBoardIndex);
        stream.WriteBoolean(RestoreActiveTask);
        stream.WriteLongId(NeighborhoodId);
        stream.WriteLongId(PlayerId);
    }

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(TakeDerbyTaskMessage);
    }
}
