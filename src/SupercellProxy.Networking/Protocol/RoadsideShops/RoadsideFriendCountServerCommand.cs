using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// <para>Updates the owner's friend count and unlocks eligible roadside stands.</para>
/// </summary>
public sealed record RoadsideFriendCountServerCommand : CodecServerCommand<RoadsideFriendCountServerCommand>, ICommandCodecProvider<RoadsideFriendCountServerCommand>
{
    /// <summary>
    /// Initializes a new <see cref="RoadsideFriendCountServerCommand"/> instance.
    /// </summary>
    public RoadsideFriendCountServerCommand(int friendCount, LongId homeOwnerId)
    {
        FriendCount = friendCount;
        HomeOwnerId = homeOwnerId;
    }

    /// <inheritdoc />
    public static CommandCodec<RoadsideFriendCountServerCommand> Codec { get; } = Codecs.Command(
        type: 210,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.FriendCount,
            Codecs.LongId,
            static command => command.HomeOwnerId,
            static (friendCount, homeOwnerId) => new RoadsideFriendCountServerCommand(friendCount, homeOwnerId)
        )
    );

    /// <summary>
    /// Gets the <c language="csharp">FriendCount</c> value.
    /// </summary>
    public int FriendCount { get; }

    /// <summary>
    /// Gets the <c language="csharp">HomeOwnerId</c> value.
    /// </summary>
    public LongId HomeOwnerId { get; }
}
