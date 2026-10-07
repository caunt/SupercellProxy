using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.Race;

/// <summary>Requests derby rankings, optionally selecting a neighborhood.</summary>
public sealed record RequestDerbyRankingsMessage(LongId? NeighborhoodId) : IMessage
{
    /// <summary>Decodes the optional neighborhood selector.</summary>
    public static RequestDerbyRankingsMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        RequestDerbyRankingsMessage message = new(stream.ReadOptionalLongId());
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the optional neighborhood selector.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteOptionalLongId(NeighborhoodId);
    }

    /// <summary>Omits the neighborhood identifier from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(RequestDerbyRankingsMessage);
    }
}
