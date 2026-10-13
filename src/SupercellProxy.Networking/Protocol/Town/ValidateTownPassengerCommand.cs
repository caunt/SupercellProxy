using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Validates that a passenger is present in the player's town without changing its state.</summary>
public sealed record ValidateTownPassengerCommand(int PassengerGlobalId) : CodecCommand<ValidateTownPassengerCommand>, ICommandCodecProvider<ValidateTownPassengerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ValidateTownPassengerCommand> Codec { get; } = Codecs.Command(
        type: 141,
        Codecs.Composite(Codecs.VarInt, static command => command.PassengerGlobalId, static (passenger) => new ValidateTownPassengerCommand(passenger))
    );
}
