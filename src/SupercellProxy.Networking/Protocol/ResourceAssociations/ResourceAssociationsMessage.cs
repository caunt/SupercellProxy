using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ResourceAssociations;

/// Replaces or adds ordered resource-association records.
public sealed record ResourceAssociationsMessage : IMessage
{
    /// Gets the decoded association records in wire order.
    public ResourceAssociationRecord[] Records { get; init; } = [];

    /// Decodes clientbound message 27398.
    public static ResourceAssociationsMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        ResourceAssociationsMessage message = new()
        {
            Records = stream.ReadArray(DecodeRecord),
        };

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "Clientbound message 27398 has trailing data.")
            : message;
    }

    /// Encodes clientbound message 27398.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(Records.Length);

        foreach (ResourceAssociationRecord record in Records)
        {
            stream.WriteUInt32(record.Key.First);
            stream.WriteUInt32(record.Key.Second);
            stream.WriteOptionalString(record.SecondaryLabel);
            stream.WriteOptionalString(record.TertiaryLabel);
            stream.WriteOptionalString(record.QuaternaryLabel);
            stream.WriteOptionalString(record.DisplayName);
        }
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
