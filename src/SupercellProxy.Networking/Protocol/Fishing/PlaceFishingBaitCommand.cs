using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>
/// Places the selected bait on one fishing-area spot.
/// The native execute resolves the spot in table 96 and the bait in table 98, rejects a negative
/// <see cref="BiteDelayMilliseconds"/>, moves the spot's attached fish to the baited state, and
/// hands the delay to the spot's bait setter together with the resolved bait row.
/// </summary>
/// <param name="FishingSpotGlobalIdentifier">The baited spot's table-96 game-object global identifier.</param>
/// <param name="BiteDelayMilliseconds">
/// The client-chosen delay before a fish bites. The native bait setter converts it into bite-timer
/// updates as <c language="csharp">value * 30 / 1000</c>; only non-negative values are accepted.
/// </param>
/// <param name="BaitGlobalIdentifier">The bait's data row in the baits table.</param>
/// <param name="ExecutionPhaseCounter">The execution phase counter.</param>
/// <param name="DebugData0">The first optional debug command data.</param>
/// <param name="DebugData1">The second optional debug command data.</param>
public sealed record PlaceFishingBaitCommand(
    int FishingSpotGlobalIdentifier,
    int BiteDelayMilliseconds,
    int BaitGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.PlaceFishingBaitCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static PlaceFishingBaitCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int fishingSpotGlobalIdentifier = stream.ReadVariableInt();
        int biteDelayMilliseconds = stream.ReadVariableInt();
        int baitGlobalIdentifier = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new PlaceFishingBaitCommand(
            fishingSpotGlobalIdentifier,
            biteDelayMilliseconds,
            baitGlobalIdentifier,
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(FishingSpotGlobalIdentifier);
        stream.WriteVariableInt(BiteDelayMilliseconds);
        stream.WriteVariableInt(BaitGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
