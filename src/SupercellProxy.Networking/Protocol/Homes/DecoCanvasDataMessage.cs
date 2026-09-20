using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// Represents the deco-canvas home snapshot carried by clientbound message 28544, loaded in native game mode 9.
public sealed record DecoCanvasDataMessage(OwnHomeDataMessage Data) : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static DecoCanvasDataMessage Create(MessageContainer container)
    {
        return new(OwnHomeDataMessage.Create(container));
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        return Data.ToContainer(identifier, version);
    }
}
