using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests a paid refresh of a board slot with a regeneration timer.</summary>
public sealed record SpeedUpDerbyBoardTaskMessage(int TaskBoardIndex, int DiamondCost, LongId NeighborhoodId, LongId PlayerId) : IMessage
{
    /// <summary>Decodes the selected slot and quoted diamond price.</summary>
    public static SpeedUpDerbyBoardTaskMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        SpeedUpDerbyBoardTaskMessage message = new(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadLongId(), stream.ReadLongId());
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the paid refresh request.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(TaskBoardIndex);
        stream.WriteVarInt(DiamondCost);
        stream.WriteLongId(NeighborhoodId);
        stream.WriteLongId(PlayerId);
    }

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(SpeedUpDerbyBoardTaskMessage);
    }
}
