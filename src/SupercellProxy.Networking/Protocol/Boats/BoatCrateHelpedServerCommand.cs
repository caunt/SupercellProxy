using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Reports a crate helper to the owner or an observer of that farm.</summary>
/// <param name="OwnHomeNotification"><see langword="true"/> for the native <c>OwnBoatCrateHelpedServerCommand</c> sent to the farm owner; <see langword="false"/> for <c>VisitedBoatHelpServerCommand</c> sent to someone visiting the farm. It is not written on the wire; it picks the contract.</param>
/// <param name="HomeOwnerId">The id of the farm whose boat crate was filled.</param>
/// <param name="HelperId">The id of the player who filled the crate.</param>
/// <param name="CrateIndex">The zero-based index of the filled crate in the boat order's crate list, written as a VarInt.</param>
/// <param name="RequestKind">The kind of help request the crate was filled through: 1 friends, 2 the neighbourhood, 3 the Farm Pass perk.</param>
public sealed record BoatCrateHelpedServerCommand(bool OwnHomeNotification, LongId HomeOwnerId, LongId HelperId, int CrateIndex, int RequestKind)
    : ServerCommand
{
    /// <summary>Gets the contract of the native command that notifies the farm owner.</summary>
    /// <value>The <c>OwnBoatCrateHelpedServerCommand</c> codec; commands it reads have <see cref="OwnHomeNotification"/> set to <see langword="true"/>.</value>
    public static CommandCodec<BoatCrateHelpedServerCommand> OwnHomeCodec { get; } = Create(name: "OwnBoatCrateHelpedServerCommand", ownHomeNotification: true);

    /// <summary>Gets the contract of the native command that notifies an observer of the farm.</summary>
    /// <value>The <c>VisitedBoatHelpServerCommand</c> codec; commands it reads have <see cref="OwnHomeNotification"/> set to <see langword="false"/>.</value>
    public static CommandCodec<BoatCrateHelpedServerCommand> VisitedHomeCodec { get; } = Create(name: "VisitedBoatHelpServerCommand", ownHomeNotification: false);

    /// <inheritdoc />
    public override CommandContract Contract => OwnHomeNotification ? OwnHomeCodec : VisitedHomeCodec;

    private static CommandCodec<BoatCrateHelpedServerCommand> Create(string name, bool ownHomeNotification)
    {
        return Codecs.Command(name,
            Codecs.Composite(Codecs.LongId,
                static command => command.HomeOwnerId,
                Codecs.LongId,
                static command => command.HelperId,
                Codecs.VarInt,
                static command => command.CrateIndex,
                Codecs.VarInt,
                static command => command.RequestKind,
                (homeOwnerId, helperId, crateIndex, requestKind) => new BoatCrateHelpedServerCommand(ownHomeNotification, homeOwnerId, helperId, crateIndex, requestKind)));
    }
}
