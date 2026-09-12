using System.Globalization;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// Carries the retained empty entry collection from clientbound message 21945.
public sealed record Clientbound21945Message : IMessage
{
    private const int MaximumEntryCount = 1000;

    /// Decodes clientbound message 21945.
    public static Clientbound21945Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        int entryCount = container.Payload.ReadVariableInt();

        return uint.CreateTruncating(entryCount) > MaximumEntryCount
            ? throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid message-21945 entry count: {entryCount}."))
            : entryCount is not 0
            ? throw new NotSupportedException(message: "Nonempty clientbound message 21945 entries are not implemented yet.")
            : container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "Clientbound message 21945 has trailing data.")
            : new Clientbound21945Message();
    }

    /// Encodes clientbound message 21945.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(valueToWrite: 0);

        return new MessageContainer(identifier, version, stream);
    }
}
