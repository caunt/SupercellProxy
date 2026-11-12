using System.Text.Json.Serialization;

using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// One protocol frame after decryption: the 7-byte header's message id and version, plus the payload ready to decode.
/// </summary>
/// <param name="Id">Wire id that tells which message type the payload holds; ids differ between game releases.</param>
/// <param name="Version">Version field from the frame header, echoed back unchanged when the frame is forwarded.</param>
/// <param name="Payload">The decrypted payload, positioned at its start.</param>
public sealed record MessageContainer([property: JsonPropertyName("Id")] ushort Id, ushort Version, MessageStream Payload)
{
    /// <summary>Frames an encoded message with its registered id and resolved or recorded header version.</summary>
    /// <param name="message">The message to encode; a <see cref="PassthroughMessage"/> keeps its own id and raw bytes.</param>
    /// <param name="version">
    /// The header version to write, or <see langword="null"/> to use the version a <see cref="PassthroughMessage"/> was received with, and 0
    /// for any other message.
    /// </param>
    /// <param name="gameVersion">
    /// The game release whose id table and payload layouts apply; it is also set on the new payload stream. With <see langword="null"/>, the
    /// message's id must be the same in every release.
    /// </param>
    /// <returns>A new container with the message's wire id, the chosen header version and an in-memory payload holding the encoded bytes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
    public static MessageContainer Create(IMessage message, ushort? version = null, Version? gameVersion = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        ushort headerVersion = version ?? (message is PassthroughMessage passthrough ? passthrough.Version : (ushort)0);

        ushort id = MessageRegistry.GetId(message, gameVersion);

        using MessageStream payload = MessageStream.Create();

        payload.GameVersion = gameVersion;

        message.Encode(payload);
        payload.FlushWriteBoolean();

        return new MessageContainer(id, headerVersion, payload);
    }
}
