using SupercellProxy.Networking.Transport;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>The player's town home snapshot carried by clientbound message 28543.</summary>
public sealed record OwnTownDataMessage(OwnHomeDataMessage Data) : IMessage
{
    /// <summary>Decodes the shared own-home snapshot wire format.</summary>
    public static OwnTownDataMessage Decode(MessageStream stream)
    {
        return new(OwnHomeDataMessage.Decode(stream));
    }

    /// <summary>Encodes the shared own-home snapshot wire format.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        Data.Encode(stream);
    }
}
