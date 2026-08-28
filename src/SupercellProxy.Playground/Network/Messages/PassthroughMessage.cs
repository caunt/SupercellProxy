using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages;

/// <summary>
/// Represents the <c language="csharp">PassthroughMessage</c> protocol message.
/// </summary>
internal sealed record PassthroughMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Id</c> value.
    /// </summary>
    public required ushort Id { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Version</c> value.
    /// </summary>
    public required ushort Version { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Data</c> value.
    /// </summary>
    public required Memory<byte> Data { get; set; }

    /// <summary>
    /// Gets the <c language="csharp">Hint</c> value.
    /// </summary>
    public string? Hint => MessageRegistry.GetHint(Id);

    /// <summary>
    /// Creates a <c language="csharp">PassthroughMessage</c> from the supplied data.
    /// </summary>
    public static PassthroughMessage Create(MessageContainer container)
    {
        return new PassthroughMessage
        {
            Id = container.Id,
            Version = container.Version,
            Data = container.Payload.ReadToEnd(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = MessageStream.Create();

        supercellStream.Write(Data.Span);

        return new MessageContainer(id, version, supercellStream);
    }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
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
