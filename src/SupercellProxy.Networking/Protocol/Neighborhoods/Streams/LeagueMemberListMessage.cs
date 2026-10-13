using SupercellProxy.Networking.Protocol.Events.Derby.Messages;
using SupercellProxy.Networking.Protocol.Events.Derby.Race;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams;

/// <summary>Clientbound league-member list with two native selection values.</summary>
public sealed record LeagueMemberListMessage : CodecMessage<LeagueMemberListMessage>, ICodecProvider<LeagueMemberListMessage>
{
    /// <inheritdoc />
    public static ICodec<LeagueMemberListMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.TimestampSeconds,
            Codecs.VarInt,
            static message => message.FirstSelector,
            Codecs.VarInt,
            static message => message.SecondSelector,
            Codecs.NullableArray(DerbyRaceEntry.Codec, CollectionCount.VarInt.WithStrictAbsence()),
            static message => message.Entries,
            Codecs.Optional(DerbyBoardSupplement.Codec),
            static message => message.Supplement,
            static (timestampSeconds, firstSelector, secondSelector, entries, supplement) => new LeagueMemberListMessage
            {
                TimestampSeconds = timestampSeconds,
                FirstSelector = firstSelector,
                SecondSelector = secondSelector,
                Entries = entries,
                Supplement = supplement,
            }
        ),
        trailingDataMessage: "The avatar-stream page has trailing data."
    );

    /// <summary>Gets the optional neighborhood race entries.</summary>
    public DerbyRaceEntry[]? Entries { get; init; }

    /// <summary>Gets the optional entry count, including existing empty-page callers.</summary>
    public int? EntryCount => Entries?.Length;

    /// <summary>Gets the first native stream selector.</summary>
    public int FirstSelector { get; init; }

    /// <summary>Gets the second native stream selector.</summary>
    public int SecondSelector { get; init; }

    /// <summary>Gets the optional native derby race supplement.</summary>
    public DerbyBoardSupplement? Supplement { get; init; }

    /// <summary>Gets the seconds timestamp carried by this stream page.</summary>
    public int TimestampSeconds { get; init; }

    /// <summary>Omits stream contents from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(LeagueMemberListMessage);
    }
}
