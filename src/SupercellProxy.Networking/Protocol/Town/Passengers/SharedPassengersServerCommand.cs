using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Passengers;

/// <summary>
/// Tells the own town that a neighbour picked up some of its waiting passengers, so they are reserved for that neighbour's
/// personal train.
/// </summary>
/// <param name="Action">Kind of update; the own town reserves the passengers whatever its value, and its values are not identified.</param>
/// <param name="SharingHomeId">Home of the neighbour whose personal train picks the passengers up.</param>
/// <param name="PassengerIds">Ids of the picked passengers.</param>
/// <param name="HelpIds">Help ids the picked passengers are matched by in the own town.</param>
public sealed record SharedPassengersServerCommand(int Action, LongId? SharingHomeId, ReadOnlyMemory<int> PassengerIds, ReadOnlyMemory<int> HelpIds) : CodecServerCommand<SharedPassengersServerCommand>, ICommandCodecProvider<SharedPassengersServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SharedPassengersServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Action,
            Codecs.OptionalLongId,
            static command => command.SharingHomeId,
            Codecs.Memory(Codecs.VarInt),
            static command => command.PassengerIds,
            Codecs.Memory(Codecs.VarInt),
            static command => command.HelpIds,
            static (action, sharingHomeId, passengerIds, helpIds) => new SharedPassengersServerCommand(action, sharingHomeId, passengerIds, helpIds)
        )
    );
}
