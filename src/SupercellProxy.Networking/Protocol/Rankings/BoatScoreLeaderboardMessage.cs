using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>Delivers the global boat scores leaderboard.</summary>
/// <param name="Entries">The board's lines in rank order; null when the list is absent.</param>
public sealed record BoatScoreLeaderboardMessage(ReadOnlyMemory<RankingEntry>? Entries) : CodecMessage<BoatScoreLeaderboardMessage>, ICodecProvider<BoatScoreLeaderboardMessage>
{
    /// <inheritdoc />
    public static ICodec<BoatScoreLeaderboardMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(RankingEntry.Codec),
        static message => message.Entries,
        static entries => new BoatScoreLeaderboardMessage(entries)
    );
}
