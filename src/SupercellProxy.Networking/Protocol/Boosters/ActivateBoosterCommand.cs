using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boosters;

/// <summary>
/// Activates a booster held in the player's booster storage.
/// The native purpose of the trailing modifier fields is unestablished; every retained input uses
/// <c>0, false, false</c>.
/// </summary>
/// <param name="BoosterDataGlobalIdentifier">The activated booster's data row in the boosters table.</param>
/// <param name="Unknown0">Retained native modifier; only zero is proven.</param>
/// <param name="Unknown1">Retained native modifier; only false is proven.</param>
/// <param name="Unknown2">Retained native modifier; only false is proven.</param>
/// <param name="ExecutionPhaseCounter">The execution phase counter.</param>
/// <param name="DebugData0">The first optional debug command data.</param>
/// <param name="DebugData1">The second optional debug command data.</param>
public sealed record ActivateBoosterCommand(
    int BoosterDataGlobalIdentifier,
    int Unknown0,
    bool Unknown1,
    bool Unknown2,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : Command(ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ActivateBoosterCommandType;

    /// <summary>Decodes a booster activation command.</summary>
    public static ActivateBoosterCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int boosterDataGlobalIdentifier = stream.ReadVariableInt();
        int unknown0 = stream.ReadVariableInt();
        bool unknown1 = stream.ReadBoolean();
        bool unknown2 = stream.ReadBoolean();
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new ActivateBoosterCommand(boosterDataGlobalIdentifier, unknown0, unknown1, unknown2, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <inheritdoc />
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVariableInt(BoosterDataGlobalIdentifier);
        stream.WriteVariableInt(Unknown0);
        stream.WriteBoolean(Unknown1);
        stream.WriteBoolean(Unknown2);
        EncodeCommand(stream, environment);
    }
}
