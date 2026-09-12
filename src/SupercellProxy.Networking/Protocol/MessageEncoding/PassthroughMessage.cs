using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// Represents the <c language="csharp">PassthroughMessage</c> protocol message.
/// </summary>
public sealed record PassthroughMessage : IMessage
{

    /// <summary>
    /// Gets or sets the <c language="csharp">Data</c> value.
    /// </summary>
    public required Memory<byte> Data { get; set; }

    /// <summary>
    /// Gets the <c language="csharp">Hint</c> value.
    /// </summary>
    public string? Hint => MessageRegistry.GetHint(Identifier);
    /// <summary>
    /// Gets or sets the <c language="csharp">Id</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Id")]
    public required ushort Identifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Version</c> value.
    /// </summary>
    public required ushort Version { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">PassthroughMessage</c> from the supplied data.
    /// </summary>
    public static PassthroughMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new PassthroughMessage
        {
            Identifier = container.Identifier,
            Version = container.Version,
            Data = container.Payload.ReadToEnd(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream supercellStream = MessageStream.Create();

        supercellStream.Write(Data.Span);

        return new MessageContainer(identifier, version, supercellStream);
    }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
    public override string ToString()
    {
        int maximumDataLength = 20;
        int actualDataLength = Data.Length;
        int lengthToConvert = Math.Min(actualDataLength, maximumDataLength);

        string hexDataString = Convert.ToHexString(Data.Span[..lengthToConvert]);
        string truncationSuffix = actualDataLength > maximumDataLength ? "..." : string.Empty;
        string hintSuffix = string.IsNullOrWhiteSpace(Hint) ? string.Empty : $", Hint = {Hint}";

        return $"{nameof(PassthroughMessage)} {{ Id = {Identifier}, Version = {Version}, DataLength = {actualDataLength}, Data = {hexDataString}{truncationSuffix}{hintSuffix} }}";
    }
}
