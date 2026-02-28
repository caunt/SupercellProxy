using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages;

public record PassthroughMessage : IMessage
{
    public required ushort Id { get; init; }
    public required ushort Version { get; init; }
    public required Memory<byte> Data { get; init; }

    public string? Hint => MessageRegistry.GetHint(Id);

    public static PassthroughMessage Create(MessageContainer container)
    {
        return new PassthroughMessage
        {
            Id = container.Id,
            Version = container.Version,
            Data = container.Payload.ReadToEnd()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.Write(Data.Span);

        return new MessageContainer(id, version, supercellStream);
    }
}
