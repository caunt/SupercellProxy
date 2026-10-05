using SupercellProxy.Networking.Transport;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// Represents the own-fishing-home snapshot carried by clientbound message 24222.
public sealed record FishingDataMessage(OwnHomeDataMessage Data) : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static FishingDataMessage Decode(MessageStream stream)
    {
        return new(OwnHomeDataMessage.Decode(stream));
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        Data.Encode(stream);
    }
}
