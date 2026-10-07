using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.History;

/// <summary>Requests the neighborhood's derby task-log tab.</summary>
public sealed record RequestDerbyTaskLogMessage(LongId PlayerId, LongId NeighborhoodId) : IMessage
{
    /// <summary>Decodes the requesting player and neighborhood.</summary>
    public static RequestDerbyTaskLogMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        RequestDerbyTaskLogMessage message = new(stream.ReadLongId(), stream.ReadLongId());
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the requesting player before the neighborhood.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteLongId(PlayerId);
        stream.WriteLongId(NeighborhoodId);
    }

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(RequestDerbyTaskLogMessage);
    }
}
