using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.Events.Derby.Messages;
using SupercellProxy.Networking.Protocol.Events.Derby.Race;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams;

/// <summary>Clientbound league-member list with two native selection values.</summary>
public sealed record LeagueMemberListMessage : CodecMessage<LeagueMemberListMessage>, ICodecProvider<LeagueMemberListMessage>
{
    /// <inheritdoc />
    public static ICodec<LeagueMemberListMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.VarInt,
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
            }),
        trailingDataMessage: "The avatar-stream page has trailing data.");

    /// <summary>Gets or sets the optional neighborhood race entries.</summary>
    /// <value>
    /// One entry per neighbourhood in the derby race the request named, in the order the server sent them, or <see langword="null"/> when
    /// the server writes the list as absent.
    /// </value>
    public ImmutableArray<DerbyRaceEntry>? Entries { get; init; }

    /// <summary>Gets the optional entry count, including existing empty-page callers.</summary>
    /// <value>The length of <see cref="Entries"/>, or <see langword="null"/> when the list is absent.</value>
    public int? EntryCount => Entries?.Length;

    /// <summary>Gets or sets the first native stream selector.</summary>
    /// <value>The second VarInt of the message, after <see cref="TimestampSeconds"/>; its meaning is not identified.</value>
    public int FirstSelector { get; init; }

    /// <summary>Gets or sets the second native stream selector.</summary>
    /// <value>The third VarInt of the message, before the entry list; its meaning is not identified.</value>
    public int SecondSelector { get; init; }

    /// <summary>Gets or sets the optional native derby race supplement.</summary>
    /// <value>The optional block that ends the message, or <see langword="null"/> when the server leaves it out.</value>
    public DerbyBoardSupplement? Supplement { get; init; }

    /// <summary>Gets or sets the seconds timestamp carried by this stream page.</summary>
    /// <value>The first VarInt of the message, a Unix time in seconds.</value>
    public int TimestampSeconds { get; init; }

    /// <summary>Omits stream contents from diagnostic text.</summary>
    /// <returns>Always <c>LeagueMemberListMessage</c>, so logs never show neighbourhood names or scores.</returns>
    public override string ToString()
    {
        return nameof(LeagueMemberListMessage);
    }
}
