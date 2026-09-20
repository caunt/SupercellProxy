using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>
/// Exchanges one booster held in the player's booster storage for another booster.
/// The native purpose of the trailing modifier field is unestablished; every retained input uses
/// <c>false</c>.
/// </summary>
/// <param name="RemovedBoosterDataGlobalIdentifier">The consumed booster's data row in the boosters table.</param>
/// <param name="AddedBoosterDataGlobalIdentifier">The granted booster's data row in the boosters table.</param>
/// <param name="Unknown0">Retained native modifier; only false is proven.</param>
/// <param name="ExecutionPhaseCounter">The execution phase counter.</param>
/// <param name="DebugData0">The first optional debug command data.</param>
/// <param name="DebugData1">The second optional debug command data.</param>
public sealed record ExchangeBoosterCommand(
    int RemovedBoosterDataGlobalIdentifier,
    int AddedBoosterDataGlobalIdentifier,
    bool Unknown0,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ExchangeBoosterCommandType;

    /// <summary>Decodes a booster exchange command.</summary>
    public static ExchangeBoosterCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int removedBoosterDataGlobalIdentifier = stream.ReadVariableInt();
        int addedBoosterDataGlobalIdentifier = stream.ReadInt32();
        bool unknown0 = stream.ReadBoolean();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new ExchangeBoosterCommand(
            removedBoosterDataGlobalIdentifier,
            addedBoosterDataGlobalIdentifier,
            unknown0,
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(RemovedBoosterDataGlobalIdentifier);
        stream.WriteInt32(AddedBoosterDataGlobalIdentifier);
        stream.WriteBoolean(Unknown0);
        EncodeCommand(stream, environment);
    }
}
