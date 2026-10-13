using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>A registered message type and the codec function that reads its payload.</summary>
/// <param name="Type">The message record type.</param>
/// <param name="Factory">Reads the message payload.</param>
public sealed record MessageRegistryEntry(Type Type, Func<MessageStream, IMessage> Factory)
{
    /// <summary>Gets the name captures use for the message.</summary>
    public string CaptureName { get; init; } = Type.Name;

    /// <summary>Creates the entry of a message type from its codec.</summary>
    public static MessageRegistryEntry Create<TMessage>() where TMessage : class, IMessage, ICodecProvider<TMessage>
    {
        return new MessageRegistryEntry(typeof(TMessage), static stream => TMessage.Codec.Read(stream));
    }
}
