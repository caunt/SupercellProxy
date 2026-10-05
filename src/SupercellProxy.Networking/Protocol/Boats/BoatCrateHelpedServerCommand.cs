using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Reports a crate helper to the owner or an observer of that farm.</summary>
public sealed record BoatCrateHelpedServerCommand(bool OwnHomeNotification, LongId HomeOwnerId, LongId HelperId, int CrateIndex, int RequestKind) : ServerCommand
{
    /// <summary>Gets the native notification type.</summary>
    public override int Type => OwnHomeNotification ? CommandRegistry.OwnBoatCrateHelpedServerCommandType : CommandRegistry.VisitedBoatHelpServerCommandType;

    /// <summary>Decodes the two notification variants without changing their fields.</summary>
    public static BoatCrateHelpedServerCommand Decode(MessageStream stream, CommandEnvironment environment, bool ownHome)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId owner = stream.ReadLongId();
        LongId helper = stream.ReadLongId();
        int crate = stream.ReadVarInt();
        int kind = stream.ReadVarInt();


        return new(ownHome, owner, helper, crate, kind);
    }

    /// <summary>Encodes the crate-help fields.</summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(HomeOwnerId);
        stream.WriteLongId(HelperId);
        stream.WriteVarInt(CrateIndex);
        stream.WriteVarInt(RequestKind);
    }
}
