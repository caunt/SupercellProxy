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
    public RoadsideFriendCountServerCommand(
        int friendCount,
        LongIdentifier homeOwnerIdentifier,
        int serverCommandIdentifier,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandIdentifier, executionPhaseCounter, debugData0, debugData1)
    {
        FriendCount = friendCount;
        HomeOwnerIdentifier = homeOwnerIdentifier;
    }

    /// <summary>
    /// Gets the <c language="csharp">FriendCount</c> value.
    /// </summary>
    public int FriendCount { get; }

    /// <summary>
    /// Gets the <c language="csharp">HomeOwnerIdentifier</c> value.
    /// </summary>
    public LongIdentifier HomeOwnerIdentifier { get; }

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
        int friendCount = stream.ReadVariableInt();
        LongIdentifier homeOwnerIdentifier = stream.ReadLongIdentifier();
        (int serverCommandIdentifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields) = DecodeServerCommand(stream, environment);

        return new RoadsideFriendCountServerCommand(
            friendCount,
            homeOwnerIdentifier,
            serverCommandIdentifier,
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(FriendCount);
        stream.WriteLongIdentifier(HomeOwnerIdentifier);
        EncodeServerCommand(stream, environment);
    }
}
