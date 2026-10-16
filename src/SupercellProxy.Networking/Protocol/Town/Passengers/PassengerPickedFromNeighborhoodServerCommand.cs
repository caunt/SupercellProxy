using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Passengers;

/// <summary>Moves town passengers picked by a neighbor, or the services they still need, between homes.</summary>
/// <param name="Action">The transfer kind: 1 receive the passengers' services, 2 reserve a personal train pickup.</param>
/// <param name="PickingHomeId">Home whose personal train picks the passengers up; null when no home is named.</param>
/// <param name="PassengerIds">Ids of the moved passengers.</param>
/// <param name="HelpIds">Help ids the passengers are matched by in the town that receives them.</param>
/// <param name="Services">Service requests the passengers bring along, listed in reverse of the order they are kept in.</param>
/// <param name="BonusGlobalId">Global id of the resource the passengers give as a bonus once all their services are done.</param>
/// <param name="BonusAmount">How much of that bonus resource they give.</param>
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
