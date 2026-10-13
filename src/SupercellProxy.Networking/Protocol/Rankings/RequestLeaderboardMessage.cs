using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>Requests a leaderboard.</summary>
/// <param name="Entries">The entries.</param>
/// <param name="NeighborhoodId">The neighborhood id.</param>
public sealed record RequestLeaderboardMessage(ReadOnlyMemory<LeaderboardEntryId> Entries, LongId? NeighborhoodId) : CodecMessage<RequestLeaderboardMessage>, ICodecProvider<RequestLeaderboardMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestLeaderboardMessage> Codec { get; } = Codecs.Composite(
        Codecs.Memory(LeaderboardEntryId.Codec),
        static message => message.Entries,
        Codecs.OptionalLongId,
        static message => message.NeighborhoodId,
        static (entries, neighborhoodId) => new RequestLeaderboardMessage(entries, neighborhoodId)
    );
}
