using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// <para>Updates the owner's friend count and unlocks eligible roadside stands.</para>
/// </summary>
public sealed record RoadsideFriendCountServerCommand : ServerCommand
{
    /// <summary>
    /// Initializes a new <see cref="RoadsideFriendCountServerCommand"/> instance.
    /// </summary>
    public RoadsideFriendCountServerCommand(int friendCount, LongId homeOwnerId)
    {
        FriendCount = friendCount;
        HomeOwnerId = homeOwnerId;
    }

    /// <summary>
    /// Gets the <c language="csharp">FriendCount</c> value.
    /// </summary>
    public int FriendCount { get; }

    /// <summary>
    /// Gets the <c language="csharp">HomeOwnerId</c> value.
    /// </summary>
    public LongId HomeOwnerId { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public override int Type => CommandRegistry.RoadsideFriendCountServerCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsideFriendCountServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int friendCount = stream.ReadVarInt();
        LongId homeOwnerId = stream.ReadLongId();


        return new RoadsideFriendCountServerCommand(friendCount, homeOwnerId);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(FriendCount);
        stream.WriteLongId(HomeOwnerId);
    }
}
