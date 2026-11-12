using System.Text.Json.Serialization;

using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// A message the code does not decode: its raw payload is kept so it can be forwarded or recorded byte for byte.
/// </summary>
public sealed record PassthroughMessage : IMessage
{
    /// <summary>
    /// Gets or sets the undecoded payload bytes, written back unchanged when the message is encoded.
    /// </summary>
    /// <value>The decrypted payload from the read position to the end of the frame, without the 7-byte header; may be empty.</value>
    public required Memory<byte> Data { get; set; }

    /// <summary>
    /// Gets or sets the wire id from the frame header, kept so the frame goes out under the same id.
    /// </summary>
    /// <value>The 16-bit message id as received; no release lookup is done on it when the message is sent again.</value>
    [JsonPropertyName("Id")]
    public required ushort Id { get; init; }

    /// <summary>
    /// Gets or sets the version field from the frame header, kept so the frame goes out with the same version.
    /// </summary>
    /// <value>The 16-bit header version as received; it is used when the message is framed without an explicit version.</value>
    public required ushort Version { get; init; }

    /// <summary>
    /// Takes the rest of a frame's payload as raw bytes, together with the frame's id and version.
    /// </summary>
    /// <param name="container">A decrypted frame.</param>
    /// <returns>A new message holding the frame's id and version and a copy of the payload bytes from the current position to the end.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null"/>.</exception>
    public static PassthroughMessage Decode(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new PassthroughMessage
        {
            Id = container.Id,
            Version = container.Version,
            Data = container.Payload.ReadToEnd(),
        };
    }

    /// <summary>
    /// Writes the kept payload bytes exactly as they were received.
    /// </summary>
    /// <param name="stream">Payload stream of the outgoing frame.</param>
    /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null"/>.</exception>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.Write(Data.Span);
    }

    /// <summary>
    /// Shows the id, version, payload size and the first 20 payload bytes in hex, enough to recognise a frame in a log.
    /// </summary>
    /// <returns>
    /// A string such as <c>PassthroughMessage { Id = 10100, Version = 0, DataLength = 3, Data = 0A0B0C }</c>, with <c>...</c> appended to the
    /// hex when the payload is longer than 20 bytes.
    /// </returns>
    public override string ToString()
    {
        int maximumDataLength = 20;
        int actualDataLength = Data.Length;
        int lengthToConvert = Math.Min(actualDataLength, maximumDataLength);

        string hexDataString = Convert.ToHexString(Data.Span[..lengthToConvert]);
        string truncationSuffix = actualDataLength > maximumDataLength ? "..." : string.Empty;

        return $"{nameof(PassthroughMessage)} {{ Id = {Id}, Version = {Version}, DataLength = {actualDataLength}, Data = {hexDataString}{truncationSuffix} }}";
    }
}
