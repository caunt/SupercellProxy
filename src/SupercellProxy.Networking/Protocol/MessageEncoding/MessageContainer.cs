using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// Represents <c language="csharp">MessageContainer</c>.
/// </summary>
public sealed record MessageContainer([property: System.Text.Json.Serialization.JsonPropertyName("Id")] ushort Id, ushort Version, MessageStream Payload)
{
    /// <summary>Frames an encoded message with its registered id and resolved or recorded header version.</summary>
    public static MessageContainer Create(IMessage message, ushort? version = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        ushort headerVersion = version ?? (message is PassthroughMessage passthrough ? passthrough.Version : (ushort)0);

        ushort id = MessageRegistry.GetId(message);

        using MessageStream payload = MessageStream.Create();

        message.Encode(payload);
        payload.FlushWriteBoolean();

        return new MessageContainer(id, headerVersion, payload);
    }
}
