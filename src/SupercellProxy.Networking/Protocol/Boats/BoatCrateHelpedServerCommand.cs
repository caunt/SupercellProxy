using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Reports a crate helper to the owner or an observer of that farm.</summary>
public sealed record BoatCrateHelpedServerCommand(
    bool OwnHomeNotification,
    LongIdentifier HomeOwnerIdentifier,
    LongIdentifier HelperIdentifier,
    int CrateIndex,
    int RequestKind,
    int ServerCommandIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : ServerCommand(ServerCommandIdentifier, ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>Gets the native notification type.</summary>
    public override int Type => OwnHomeNotification ? CommandRegistry.OwnBoatCrateHelpedServerCommandType : CommandRegistry.VisitedBoatHelpServerCommandType;

    /// <summary>Decodes the two notification variants without changing their fields.</summary>
    public static BoatCrateHelpedServerCommand Decode(MessageStream stream, CommandEnvironment environment, bool ownHome)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongIdentifier owner = stream.ReadLongIdentifier();
        LongIdentifier helper = stream.ReadLongIdentifier();
        int crate = stream.ReadVariableInt();
        int kind = stream.ReadVariableInt();
        (int identifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields) = DecodeServerCommand(stream, environment);

        return new(ownHome, owner, helper, crate, kind, identifier, fields.ExecutionPhaseCounter, fields.DebugData0, fields.DebugData1);
    }

    /// <summary>Encodes the original body-before-header layout.</summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongIdentifier(HomeOwnerIdentifier);
        stream.WriteLongIdentifier(HelperIdentifier);
        stream.WriteVariableInt(CrateIndex);
        stream.WriteVariableInt(RequestKind);
        EncodeServerCommand(stream, environment);
    }
}
