using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages.Race;

/// <summary>Requests the selected derby instance's race page.</summary>
public sealed record RequestDerbyRaceMessage(LongId? InstanceId, LongId? NeighborhoodId, LongId? PlayerId) : IMessage
{
    /// <summary>Decodes the three independently optional native identifiers.</summary>
    public static RequestDerbyRaceMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        RequestDerbyRaceMessage message = new(stream.ReadOptionalLongId(), stream.ReadOptionalLongId(), stream.ReadOptionalLongId());
        DerbyMessageCodec.RequireEnd(stream);

        return message;
    }

    /// <summary>Encodes the instance, neighborhood and requesting player in native order.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteOptionalLongId(InstanceId);
        stream.WriteOptionalLongId(NeighborhoodId);
        stream.WriteOptionalLongId(PlayerId);
    }

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(RequestDerbyRaceMessage);
    }
}
