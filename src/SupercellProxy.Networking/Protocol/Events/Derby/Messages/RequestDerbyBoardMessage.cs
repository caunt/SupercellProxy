using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Requests the neighborhood's current derby task board.</summary>
public sealed record RequestDerbyBoardMessage(LongId PlayerId, LongId NeighborhoodId) : IMessage
{
    /// <summary>Decodes the player and neighborhood identifiers in native order.</summary>
    public static RequestDerbyBoardMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        RequestDerbyBoardMessage message = new(stream.ReadLongId(), stream.ReadLongId());
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the player before the neighborhood.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteLongId(PlayerId);
        stream.WriteLongId(NeighborhoodId);
    }

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(RequestDerbyBoardMessage);
    }
}
