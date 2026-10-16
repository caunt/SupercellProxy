using System.Text.Json.Serialization;

using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// A message the code does not decode: its raw payload is kept so it can be forwarded or recorded byte for byte.
/// </summary>
public sealed record PassthroughMessage : IMessage
{
    /// <summary>
    /// The undecoded payload bytes, written back unchanged when the message is encoded.
    /// </summary>
    public required Memory<byte> Data { get; set; }

    /// <summary>
    /// Wire id from the frame header, kept so the frame goes out under the same id.
    /// </summary>
    [JsonPropertyName("Id")]
    public required ushort Id { get; init; }

    /// <summary>
    /// Version field from the frame header, kept so the frame goes out with the same version.
    /// </summary>
    public required ushort Version { get; init; }

    /// <summary>
    /// Takes the rest of a frame's payload as raw bytes, together with the frame's id and version.
    /// </summary>
    /// <param name="container">A decrypted frame.</param>
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
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.Write(Data.Span);
    }

    /// <summary>
    /// Shows the id, version, payload size and the first 20 payload bytes in hex, enough to recognise a frame in a log.
    /// </summary>
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
