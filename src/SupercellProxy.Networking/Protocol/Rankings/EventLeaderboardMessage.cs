using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>Delivers an event leaderboard to the event popup that requested it.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="Entries">The entries.</param>
/// <param name="Value0">The value0.</param>
/// <param name="Value1">The value1.</param>
public sealed record EventLeaderboardMessage(int EventId, ReadOnlyMemory<SupercellProxy.Networking.Protocol.Rankings.RankingEntry>? Entries, long Value0, int Value1) : CodecMessage<EventLeaderboardMessage>, ICodecProvider<EventLeaderboardMessage>
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
