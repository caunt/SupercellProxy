using SupercellProxy.Playground.Network.Streams;

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
        var maximumDataLength = 20;
        var actualDataLength = Data.Length;
        var lengthToConvert = Math.Min(actualDataLength, maximumDataLength);

        var hexDataString = Convert.ToHexString(Data.Span[..lengthToConvert]);
        var truncationSuffix = actualDataLength > maximumDataLength ? "..." : string.Empty;
        var hintSuffix = string.IsNullOrWhiteSpace(Hint) ? string.Empty : $", Hint = {Hint}";

        return $"{nameof(PassthroughMessage)} {{ Id = {Id}, Version = {Version}, DataLength = {actualDataLength}, Data = {hexDataString}{truncationSuffix}{hintSuffix} }}";
    }
}
