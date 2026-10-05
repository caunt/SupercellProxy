using SupercellProxy.Networking.Transport;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// Represents Greg's-farm snapshot carried by clientbound message 20699, loaded in native game mode 3.
public sealed record GregFarmDataMessage(OwnHomeDataMessage Data) : IMessage
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static GregFarmDataMessage Decode(MessageStream stream)
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
