using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Reports a crate helper to the owner or an observer of that farm.</summary>
public sealed record BoatCrateHelpedServerCommand(bool OwnHomeNotification, LongId HomeOwnerId, LongId HelperId, int CrateIndex, int RequestKind)
    : ServerCommand
{
    /// <summary>Gets the contract of the native command that notifies the farm owner.</summary>
    public static CommandCodec<BoatCrateHelpedServerCommand> OwnHomeCodec { get; } = Create(name: "OwnBoatCrateHelpedServerCommand", ownHomeNotification: true);

    /// <summary>Gets the contract of the native command that notifies an observer of the farm.</summary>
    public static CommandCodec<BoatCrateHelpedServerCommand> VisitedHomeCodec { get; } = Create(name: "VisitedBoatHelpServerCommand", ownHomeNotification: false);

    /// <inheritdoc />
    public override CommandContract Contract => OwnHomeNotification ? OwnHomeCodec : VisitedHomeCodec;

    private static CommandCodec<BoatCrateHelpedServerCommand> Create(string name, bool ownHomeNotification)
    {
        return Codecs.Command(
            name,
            Codecs.Composite(
                Codecs.LongId,
                static command => command.HomeOwnerId,
                Codecs.LongId,
                static command => command.HelperId,
                Codecs.VarInt,
                static command => command.CrateIndex,
                Codecs.VarInt,
                static command => command.RequestKind,
                (homeOwnerId, helperId, crateIndex, requestKind) => new BoatCrateHelpedServerCommand(ownHomeNotification, homeOwnerId, helperId, crateIndex, requestKind)
            )
        );
    }
}
