using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Passengers;

/// <summary>Moves town passengers picked by a neighbor, or the services they still need, between homes.</summary>
/// <param name="Action">The transfer kind: 1 receive the passengers' services, 2 reserve a personal train pickup.</param>
/// <param name="PickingHomeId">The picking home id.</param>
/// <param name="PassengerIds">The passenger ids.</param>
/// <param name="HelpIds">The help ids.</param>
/// <param name="Services">The services.</param>
/// <param name="BonusGlobalId">The bonus data id.</param>
/// <param name="BonusAmount">The bonus amount.</param>
public sealed record PassengerPickedFromNeighborhoodServerCommand(
    int Action,
    LongId? PickingHomeId,
    ReadOnlyMemory<int> PassengerIds,
    ReadOnlyMemory<int> HelpIds,
    ReadOnlyMemory<PassengerService> Services,
    int BonusGlobalId,
    int BonusAmount
) : CodecServerCommand<PassengerPickedFromNeighborhoodServerCommand>, ICommandCodecProvider<PassengerPickedFromNeighborhoodServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PassengerPickedFromNeighborhoodServerCommand> Codec { get; } = Codecs.Command(
        type: 148,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Action,
            Codecs.OptionalLongId,
            static command => command.PickingHomeId,
            Codecs.Memory(Codecs.VarInt),
            static command => command.PassengerIds,
            Codecs.Memory(Codecs.VarInt),
            static command => command.HelpIds,
            Codecs.Memory(PassengerService.Codec),
            static command => command.Services,
            Codecs.VarInt,
            static command => command.BonusGlobalId,
            Codecs.Int32,
            static command => command.BonusAmount,
            static (action, pickingHomeId, passengerIds, helpIds, services, bonusGlobalId, bonusAmount) => new PassengerPickedFromNeighborhoodServerCommand(action, pickingHomeId, passengerIds, helpIds, services, bonusGlobalId, bonusAmount)
        )
    );
}
