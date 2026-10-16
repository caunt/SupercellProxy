using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Reports how many friends the farm has; each friend-count gate of the roadside shop the count reaches unlocks one more stand.
/// </summary>
public sealed record RoadsideFriendCountServerCommand : CodecServerCommand<RoadsideFriendCountServerCommand>, ICommandCodecProvider<RoadsideFriendCountServerCommand>
{
    /// <summary>Creates the command for one farm.</summary>
    /// <param name="friendCount">How many friends the farm has.</param>
    /// <param name="homeOwnerId">Home the count belongs to.</param>
    public RoadsideFriendCountServerCommand(int friendCount, LongId homeOwnerId)
    {
        FriendCount = friendCount;
        HomeOwnerId = homeOwnerId;
    }

    /// <inheritdoc />
    public static CommandCodec<RoadsideFriendCountServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.FriendCount,
            Codecs.LongId,
            static command => command.HomeOwnerId,
            static (friendCount, homeOwnerId) => new RoadsideFriendCountServerCommand(friendCount, homeOwnerId)
        )
    );

    /// <summary>How many friends the farm has; a negative count changes nothing.</summary>
    public int FriendCount { get; }

    /// <summary>Home the count belongs to; other homes ignore the command.</summary>
    public LongId HomeOwnerId { get; }
}
