using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ResourceAssociations;

/// Replaces or adds ordered resource-association records.
public sealed record ResourceAssociationsMessage : IMessage
{
    /// Gets the decoded association records in wire order.
    public ResourceAssociationRecord[] Records { get; init; } = [];

    /// Decodes clientbound message 27398.
    public static ResourceAssociationsMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        ResourceAssociationsMessage message = new()
        {
            Records = container.Payload.ReadArray(DecodeRecord),
        };

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "Clientbound message 27398 has trailing data.")
            : message;
    }

    /// Encodes clientbound message 27398.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Records.Length);

        foreach (ResourceAssociationRecord record in Records)
        {
            stream.WriteUInt32(record.Key.First);
            stream.WriteUInt32(record.Key.Second);
            stream.WriteOptionalString(record.SecondaryLabel);
            stream.WriteOptionalString(record.TertiaryLabel);
            stream.WriteOptionalString(record.QuaternaryLabel);
            stream.WriteOptionalString(record.DisplayName);
        }

        return new MessageContainer(identifier, version, stream);
    }

    private static ResourceAssociationRecord DecodeRecord(MessageStream stream)
    {
        return new ResourceAssociationRecord(
            new ResourceAssociationKey(stream.ReadUInt32(), stream.ReadUInt32()),
            stream.ReadOptionalString(),
            stream.ReadOptionalString(),
            stream.ReadOptionalString(),
            stream.ReadOptionalString()
        );
    }
}
