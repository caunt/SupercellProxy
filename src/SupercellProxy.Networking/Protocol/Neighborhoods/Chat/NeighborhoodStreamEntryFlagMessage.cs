using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Sets the second state flag on a Neighborhood stream entry selected by its long id.</summary>
public sealed record NeighborhoodStreamEntryFlagMessage(LongId EntryId) : IMessage
{
    /// <summary>Decodes the selected stream-entry id.</summary>
    public static NeighborhoodStreamEntryFlagMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId id = stream.ReadLongId();

        return stream.Position == stream.Length
            ? new NeighborhoodStreamEntryFlagMessage(id)
            : throw new InvalidDataException(message: "The Neighborhood stream-entry flag message has trailing data.");
    }

    /// <summary>Encodes the selected stream-entry id.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteLongId(EntryId);
    }

    /// <summary>Omits the stream-entry id from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(NeighborhoodStreamEntryFlagMessage);
    }
}
