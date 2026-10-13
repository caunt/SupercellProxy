using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Trains;

/// <summary>Picks up AI visitor passengers waiting in the player's own town.</summary>
/// <param name="PassengerGlobalIds">Gets the passenger game object ids picked up together.</param>
public sealed record PickUpTownVisitorPassengersCommand(ReadOnlyMemory<int> PassengerGlobalIds) : CodecCommand<PickUpTownVisitorPassengersCommand>, ICommandCodecProvider<PickUpTownVisitorPassengersCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PickUpTownVisitorPassengersCommand> Codec { get; } = Codecs.Command(
        type: 152,
        Codecs.Composite(
            Codecs.Memory(Codecs.VarInt),
            static command => command.PassengerGlobalIds,
            static (passengerGlobalIds) => new PickUpTownVisitorPassengersCommand(passengerGlobalIds)
        )
    );
}
