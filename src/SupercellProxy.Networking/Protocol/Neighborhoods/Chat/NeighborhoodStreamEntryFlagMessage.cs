using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Sets the second state flag on a Neighborhood stream entry selected by its long id.</summary>
/// <param name="EntryId">
/// Id of the Neighborhood stream entry, such as a chat message or help request, whose flag is set; the message carries nothing else and
/// fails to decode when bytes follow the id.
/// </param>
public sealed record NeighborhoodStreamEntryFlagMessage(LongId EntryId) : CodecMessage<NeighborhoodStreamEntryFlagMessage>, ICodecProvider<NeighborhoodStreamEntryFlagMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodStreamEntryFlagMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.LongId, static message => message.EntryId, static entryId => new NeighborhoodStreamEntryFlagMessage(entryId)),
        trailingDataMessage: "The Neighborhood stream-entry flag message has trailing data.");

    /// <summary>Omits the stream-entry id from diagnostic text.</summary>
    /// <returns>Always the type name <c>NeighborhoodStreamEntryFlagMessage</c>, whatever the entry id.</returns>
    public override string ToString()
    {
        return nameof(NeighborhoodStreamEntryFlagMessage);
    }
}
