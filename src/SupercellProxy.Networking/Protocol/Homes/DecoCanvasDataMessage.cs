using SupercellProxy.Networking.Transport;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// Represents the deco-canvas home snapshot carried by clientbound message 28544, loaded in native game mode 9.
public sealed record DecoCanvasDataMessage(OwnHomeDataMessage Data) : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static DecoCanvasDataMessage Decode(MessageStream stream)
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
