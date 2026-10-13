using System.Collections.Frozen;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Reports a crate helper to the owner or an observer of that farm.</summary>
public sealed record BoatCrateHelpedServerCommand(bool OwnHomeNotification, LongId HomeOwnerId, LongId HelperId, int CrateIndex, int RequestKind)
    : ServerCommand
{
    /// <summary>The contract id when <see cref="OwnHomeNotification"/> is true.</summary>
    public const int OwnBoatCrateHelpedServerCommandType = 387;

    /// <summary>The contract id when <see cref="OwnHomeNotification"/> is false.</summary>
    public const int VisitedBoatHelpServerCommandType = 386;

    /// <summary>Gets the codec of each contract the record carries, by contract id.</summary>
    public static FrozenDictionary<int, CommandCodec<BoatCrateHelpedServerCommand>> ContractCodecs { get; } = new Dictionary<int, CommandCodec<BoatCrateHelpedServerCommand>>
    {
        [VisitedBoatHelpServerCommandType] = Create(VisitedBoatHelpServerCommandType, ownHomeNotification: false),
        [OwnBoatCrateHelpedServerCommandType] = Create(OwnBoatCrateHelpedServerCommandType, ownHomeNotification: true),
    }.ToFrozenDictionary();

    /// <inheritdoc />
    public override int Type => OwnHomeNotification ? OwnBoatCrateHelpedServerCommandType : VisitedBoatHelpServerCommandType;

    private static CommandCodec<BoatCrateHelpedServerCommand> Create(int type, bool ownHomeNotification)
    {
        return Codecs.Command(
            type,
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
