using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Fills an own-farm boat crate using goods in the player's inventories.</summary>
/// <param name="UseDirectRewards">Selects direct inventory grants instead of per-resource presentation callbacks. Both paths update state synchronously.</param>
/// <param name="CrateIndex">The zero-based crate within the selected order.</param>
/// <param name="AllowCompletedOrder">Allows filling an unpaid crate after helpers completed the order.</param>
/// <param name="ExecutionPhaseCounter"></param>
/// <param name="DebugData0"></param>
/// <param name="DebugData1"></param>
public sealed record FillBoatCrateCommand(
    bool UseDirectRewards,
    int CrateIndex,
    bool AllowCompletedOrder,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.FillBoatCrateCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static FillBoatCrateCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new FillBoatCrateCommand(
            stream.ReadBoolean(),
            stream.ReadVariableInt(),
            stream.ReadBoolean(),
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteBoolean(UseDirectRewards);
        stream.WriteVariableInt(CrateIndex);
        stream.WriteBoolean(AllowCompletedOrder);
    }
}
