using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>
/// Represents <c language="csharp">MessageContainer</c>.
/// </summary>
public sealed record MessageContainer([property: System.Text.Json.Serialization.JsonPropertyName("Id")] ushort Identifier, ushort Version, MessageStream Payload)
{
    /// <summary>Frames an encoded message with its registered identifier and resolved or recorded header version.</summary>
    public static MessageContainer Create(IMessage message, ushort? version = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        ushort headerVersion = version ?? (message is PassthroughMessage passthrough ? passthrough.Version : (ushort)0);

        return new MessageContainer(MessageRegistry.GetIdentifier(message), headerVersion, message.ToStream());
    }
}
