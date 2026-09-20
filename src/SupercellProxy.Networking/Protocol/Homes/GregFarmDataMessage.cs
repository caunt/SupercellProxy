using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// Represents Greg's-farm snapshot carried by clientbound message 20699, loaded in native game mode 3.
public sealed record GregFarmDataMessage(OwnHomeDataMessage Data) : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static GregFarmDataMessage Create(MessageContainer container)
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
