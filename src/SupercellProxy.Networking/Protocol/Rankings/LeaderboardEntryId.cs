using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>An account on a leaderboard request, sent as its two id words.</summary>
/// <param name="High">First (shard) word of the account id.</param>
/// <param name="Low">Second (row) word of the account id.</param>
public sealed record LeaderboardEntryId(int High, int Low) : ICodecProvider<LeaderboardEntryId>
{
    /// <inheritdoc />
    public static ICodec<LeaderboardEntryId> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static value => value.High,
        Codecs.Int32,
        static value => value.Low,
        static (high, low) => new LeaderboardEntryId(high, low)
    );
}
