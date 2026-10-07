using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Returns a board-slot speedup result and its actual diamond charge.</summary>
public sealed record SpeedUpDerbyBoardTaskResponseMessage(LongId NeighborhoodId, LongId PlayerId, int TaskBoardIndex, int DiamondCost, int Status) : IMessage
{
    /// <summary>Decodes the paid refresh result.</summary>
    public static SpeedUpDerbyBoardTaskResponseMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        SpeedUpDerbyBoardTaskResponseMessage message = new(stream.ReadLongId(), stream.ReadLongId(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the paid refresh result.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteLongId(NeighborhoodId);
        stream.WriteLongId(PlayerId);
        stream.WriteVarInt(TaskBoardIndex);
        stream.WriteVarInt(DiamondCost);
        stream.WriteVarInt(Status);
    }

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(SpeedUpDerbyBoardTaskResponseMessage);
    }
}
