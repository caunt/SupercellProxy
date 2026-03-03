using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

public record OtherHomeDataMessage : IMessage
{
    // LogicClientAvatar
    // VarInt
    // LogicClientAvatar
    // Length-prefixed byte array - Home Data
    // Length-prefixed byte array - Alliance Data
    // Length-prefixed byte array - Global Eevnt / Calendar Data
    public required Memory<byte> UnknownData { get; init; }

    public static OtherHomeDataMessage Create(MessageContainer container)
    {
        return new OtherHomeDataMessage
        {
            UnknownData = container.Payload.ReadToEnd()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.Write(UnknownData.Span);

        return new MessageContainer(id, version, supercellStream);
    }
}
