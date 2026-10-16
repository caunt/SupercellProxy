using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration;

/// <summary>Clientbound featured decoration designs for one event group.</summary>
public sealed record FeaturingDesignListMessage : CodecMessage<FeaturingDesignListMessage>, ICodecProvider<FeaturingDesignListMessage>
{
    /// <inheritdoc />
    public static ICodec<FeaturingDesignListMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.Optional(Codecs.Array(Codecs.Optional(FeaturingDesignEntry.Codec), CollectionCount.AtMost(maximum: 1024))),
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
            }
        ),
        trailingDataMessage: "The featured-design list has trailing data."
    );

    /// <summary>Unix milliseconds when the server picked this set of featured designs; a newer set replaces an older one for the group.</summary>
    public long BatchTimestampMilliseconds { get; init; }

    /// <summary>Gets the optional featured-design list.</summary>
    public FeaturingDesignEntry?[]? Entries { get; init; }

    /// <summary>Gets the decoration event id.</summary>
    public int EventId { get; init; }

    /// <summary>Gets the decoration event variant id.</summary>
    public int EventVariantId { get; init; }

    /// <summary>Gets the featuring group id.</summary>
    public int GroupId { get; init; }
}
