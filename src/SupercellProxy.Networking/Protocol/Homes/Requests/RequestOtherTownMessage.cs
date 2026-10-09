using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Homes.Requests;

/// <summary>Requests the Town belonging to the selected home.</summary>
public sealed record RequestOtherTownMessage(LongId HomeId) : IMessage
{
    /// <summary>Decodes the target home's identifier.</summary>
    public static RequestOtherTownMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new RequestOtherTownMessage(stream.ReadLongId());
    }

    /// <summary>Encodes the target home's identifier.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteLongId(HomeId);
    }
}
