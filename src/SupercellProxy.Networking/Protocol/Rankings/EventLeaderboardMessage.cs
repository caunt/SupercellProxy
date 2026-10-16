using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>Delivers an event leaderboard to the event popup that requested it.</summary>
/// <param name="EventId">Event the leaderboard belongs to.</param>
/// <param name="Entries">The board's lines in rank order; null when the list is absent.</param>
/// <param name="Value0">64-bit number sent after the lines; its meaning is not identified.</param>
/// <param name="Value1">Number that closes the message; not identified.</param>
public sealed record EventLeaderboardMessage(int EventId, ReadOnlyMemory<RankingEntry>? Entries, long Value0, int Value1) : CodecMessage<EventLeaderboardMessage>, ICodecProvider<EventLeaderboardMessage>
{
    /// <inheritdoc />
    public static ICodec<EventLeaderboardMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.EventId,
        Codecs.NullableMemory(RankingEntry.Codec),
        static message => message.Entries,
        Codecs.VarLong,
        static message => message.Value0,
        Codecs.VarInt,
        static message => message.Value1,
        static (eventId, entries, value0, value1) => new EventLeaderboardMessage(eventId, entries, value0, value1)
    );
}
