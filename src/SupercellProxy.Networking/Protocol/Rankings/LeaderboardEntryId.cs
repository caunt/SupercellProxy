using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>One leaderboard entry id entry.</summary>
/// <param name="High">The high.</param>
/// <param name="Low">The low.</param>
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
