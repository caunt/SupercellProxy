using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Seen;

/// <summary>Spins the Valley fuel wheel.</summary>
public sealed record SpinMapGameFuelWheelCommand : CodecCommand<SpinMapGameFuelWheelCommand>, ICommandCodecProvider<SpinMapGameFuelWheelCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpinMapGameFuelWheelCommand> Codec { get; } = Codecs.Command(type: 616, Codecs.Unit(static () => new SpinMapGameFuelWheelCommand()));
}
