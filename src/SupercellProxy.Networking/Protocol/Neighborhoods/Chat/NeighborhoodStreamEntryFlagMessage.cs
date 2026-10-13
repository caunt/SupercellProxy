using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Sets the second state flag on a Neighborhood stream entry selected by its long id.</summary>
public sealed record NeighborhoodStreamEntryFlagMessage(LongId EntryId) : CodecMessage<NeighborhoodStreamEntryFlagMessage>, ICodecProvider<NeighborhoodStreamEntryFlagMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodStreamEntryFlagMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(Codecs.LongId, static message => message.EntryId, static (entryId) => new NeighborhoodStreamEntryFlagMessage(entryId)),
        trailingDataMessage: "The Neighborhood stream-entry flag message has trailing data."
    );

    /// <summary>Omits the stream-entry id from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(NeighborhoodStreamEntryFlagMessage);
    }
}
