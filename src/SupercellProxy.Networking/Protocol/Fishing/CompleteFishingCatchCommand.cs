using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>
/// Completes a hooked catch on one fishing-area spot. The native execute accepts the command only in
/// the fishing game mode, resolves the spot in table 96, the fish in table 97 and the bait in table
/// 98, and then takes one of two branches: <see cref="Caught"/> keeps the catch, while a cleared
/// <see cref="Caught"/> releases it and returns every attached fish to the swimming state.
/// </summary>
/// <param name="Caught">
/// Whether the hooked catch is kept. The native execute requires both a resolved fish and a resolved
/// catch reference while it is set, and takes the releasing branch while it is cleared.
/// </param>
/// <param name="FishingSpotGlobalIdentifier">The spot's table-96 game-object global identifier.</param>
/// <param name="FishGlobalIdentifier">The caught fish's table-97 game-object global identifier.</param>
/// <param name="BaitGlobalIdentifier">The placed bait's data row in the baits table.</param>
/// <param name="ExecutionPhaseCounter">The execution phase counter.</param>
/// <param name="DebugData0">The first optional debug command data.</param>
/// <param name="DebugData1">The second optional debug command data.</param>
public sealed record CompleteFishingCatchCommand(
    bool Caught,
    int FishingSpotGlobalIdentifier,
    int FishGlobalIdentifier,
    int BaitGlobalIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.CompleteFishingCatchCommandType;

    /// <summary>Decodes a value from the supplied protocol payload.</summary>
    public static CompleteFishingCatchCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        bool caught = stream.ReadBoolean();
        int fishingSpotGlobalIdentifier = stream.ReadVariableInt();
        int fishGlobalIdentifier = stream.ReadVariableInt();
        int baitGlobalIdentifier = stream.ReadVariableInt();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new CompleteFishingCatchCommand(
            caught,
            fishingSpotGlobalIdentifier,
            fishGlobalIdentifier,
            baitGlobalIdentifier,
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteBoolean(Caught);
        stream.WriteVariableInt(FishingSpotGlobalIdentifier);
        stream.WriteVariableInt(FishGlobalIdentifier);
        stream.WriteVariableInt(BaitGlobalIdentifier);
        EncodeCommand(stream, environment);
    }
}
