using SupercellProxy.Playground.Network.Streams;
using System.Text;

namespace SupercellProxy.Playground.Network.Messages;

public record PassthroughMessage : IMessage
{
    public required ushort Id { get; init; }
    public required ushort Version { get; init; }
    public required Memory<byte> Data { get; set; }

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

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(nameof(PassthroughMessage));
        stringBuilder.Append(" { ");
        stringBuilder.Append("Id = ").Append(Id).Append(", ");
        stringBuilder.Append("Version = ").Append(Version).Append(", ");
        stringBuilder.Append("DataLength = ").Append(Data.Length).Append(", ");
        stringBuilder.Append("Data = ").Append(Convert.ToHexString(Data.Span[..Math.Min(Data.Length, 20)]));

        if (!string.IsNullOrWhiteSpace(Hint))
            stringBuilder.Append(", ").Append("Hint = ").Append(Hint);

        stringBuilder.Append(" }");

        return stringBuilder.ToString();
    }
}
