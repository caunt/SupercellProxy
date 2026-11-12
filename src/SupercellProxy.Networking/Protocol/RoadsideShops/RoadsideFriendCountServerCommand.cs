using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Reports how many friends the farm has; each friend-count gate of the roadside shop the count reaches unlocks one more stand.
/// </summary>
public sealed record RoadsideFriendCountServerCommand : CodecServerCommand<RoadsideFriendCountServerCommand>, ICommandCodecProvider<RoadsideFriendCountServerCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RoadsideFriendCountServerCommand"/> record. Creates the command for one farm.</summary>
    /// <param name="friendCount">How many friends the farm has.</param>
    /// <param name="homeOwnerId">Home the count belongs to.</param>
    public RoadsideFriendCountServerCommand(int friendCount, LongId homeOwnerId)
    {
        FriendCount = friendCount;
        HomeOwnerId = homeOwnerId;
    }

    /// <inheritdoc />
    public static CommandCodec<RoadsideFriendCountServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.FriendCount,
        Codecs.LongId,
        static command => command.HomeOwnerId,
        static (friendCount, homeOwnerId) => new RoadsideFriendCountServerCommand(friendCount, homeOwnerId)));

    /// <summary>Gets how many friends the farm has.</summary>
    /// <value>The friend count, written as a var-int before the home id.</value>
    public int FriendCount { get; }

    /// <summary>Gets the home the count belongs to.</summary>
    /// <value>The farm's home id, written as a high/low <see cref="LongId"/> after the count.</value>
    public LongId HomeOwnerId { get; }
}
