using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Stores the hash of the friend leaderboard last opened on the own farm.</summary>
/// <param name="LeaderboardHash">The opened leaderboard hash.</param>
public sealed record SetLastOpenedFriendLeaderboardCommand(int LeaderboardHash) : CodecCommand<SetLastOpenedFriendLeaderboardCommand>, ICommandCodecProvider<SetLastOpenedFriendLeaderboardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetLastOpenedFriendLeaderboardCommand> Codec { get; } = Codecs.Command(
        type: 101,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.LeaderboardHash,
            static (leaderboardHash) => new SetLastOpenedFriendLeaderboardCommand(leaderboardHash)
        )
    );
}
