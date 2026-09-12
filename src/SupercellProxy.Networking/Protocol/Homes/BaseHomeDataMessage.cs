using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// Represents the base-home snapshot envelope used by clientbound message 28544.
public sealed record BaseHomeDataMessage(OwnHomeDataMessage Data) : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static BaseHomeDataMessage Create(MessageContainer container)
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
