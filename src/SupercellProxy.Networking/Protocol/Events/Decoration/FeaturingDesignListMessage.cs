using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Clientbound featured decoration designs for one event group.</summary>
public sealed record FeaturingDesignListMessage : CodecMessage<FeaturingDesignListMessage>, ICodecProvider<FeaturingDesignListMessage>
{
    /// <inheritdoc />
    public static ICodec<FeaturingDesignListMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.Optional(Codecs.Array(Codecs.Optional(FeaturingDesignEntry.Codec), CollectionCount.AtMost(maximum: 1024))),
            static message => message.Entries,
            Codecs.VarLong,
            static message => message.BatchTimestampMilliseconds,
            Codecs.VarInt,
            static message => message.GroupId,
            Codecs.VarInt,
            static message => message.EventId,
            Codecs.VarInt,
            static message => message.EventVariantId,
            static (entries, batchTimestampMilliseconds, groupId, eventId, eventVariantId) => new FeaturingDesignListMessage
            {
                Entries = entries,
                BatchTimestampMilliseconds = batchTimestampMilliseconds,
                GroupId = groupId,
                EventId = eventId,
                EventVariantId = eventVariantId,
            }),
        trailingDataMessage: "The featured-design list has trailing data.");

    /// <summary>Gets or sets the Unix milliseconds when the server picked this set of featured designs.</summary>
    /// <value>A millisecond timestamp sent as a variable-length long right after the entries.</value>
    public long BatchTimestampMilliseconds { get; init; }

    /// <summary>Gets or sets the optional featured-design list.</summary>
    /// <value>Up to 1024 entries, any of which may be <see langword="null"/>; the whole list is <see langword="null"/> when the server sends none.</value>
    public FeaturingDesignEntry?[]? Entries { get; init; }

    /// <summary>Gets or sets the decoration event id.</summary>
    /// <value>The live event id of the decoration event the featured designs belong to.</value>
    public int EventId { get; init; }

    /// <summary>Gets or sets the decoration event variant id.</summary>
    /// <value>The variant id of that decoration event, the last field of the message.</value>
    public int EventVariantId { get; init; }

    /// <summary>Gets or sets the featuring group id.</summary>
    /// <value>The featuring group the list is for, 0 to 3, sent as a VarInt after the timestamp.</value>
    public int GroupId { get; init; }
}
