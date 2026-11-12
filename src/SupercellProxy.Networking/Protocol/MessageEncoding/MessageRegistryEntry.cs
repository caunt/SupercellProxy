using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>A registered message type and the codec function that reads its payload.</summary>
/// <param name="Type">The message record type.</param>
/// <param name="Factory">Reads the message payload.</param>
public sealed record MessageRegistryEntry(Type Type, Func<MessageStream, IMessage> Factory)
{
    /// <summary>Gets or sets the name captures use for the message.</summary>
    /// <value>
    /// The message name written into capture file names; defaults to the record type's name, and a release registration overrides it
    /// when older captures stored the message under another name.
    /// </value>
    public string CaptureName { get; init; } = Type.Name;

    /// <summary>Creates the entry of a message type from its codec.</summary>
    /// <typeparam name="TMessage">The message record type; its static <c>Codec</c> reads the payload.</typeparam>
    /// <returns>An entry for <typeparamref name="TMessage"/> whose factory decodes a payload with that codec, using the type name as capture name.</returns>
    public static MessageRegistryEntry Create<TMessage>() where TMessage : class, IMessage, ICodecProvider<TMessage>
    {
        return new MessageRegistryEntry(typeof(TMessage), static stream => TMessage.Codec.Read(stream));
    }
}
