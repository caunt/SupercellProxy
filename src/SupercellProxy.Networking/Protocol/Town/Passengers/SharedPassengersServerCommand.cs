using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Passengers;

/// <summary>Updates town passengers shared with the neighborhood.</summary>
/// <param name="Action">The action.</param>
/// <param name="SharingHomeId">The sharing home id.</param>
/// <param name="PassengerIds">The passenger ids.</param>
/// <param name="HelpIds">The help ids.</param>
public sealed record SharedPassengersServerCommand(int Action, LongId? SharingHomeId, ReadOnlyMemory<int> PassengerIds, ReadOnlyMemory<int> HelpIds) : CodecServerCommand<SharedPassengersServerCommand>, ICommandCodecProvider<SharedPassengersServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SharedPassengersServerCommand> Codec { get; } = Codecs.Command(
        type: 149,
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
