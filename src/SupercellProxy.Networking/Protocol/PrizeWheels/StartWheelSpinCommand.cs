using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.PrizeWheels;

/// <summary>Starts a spin of the wheel car.</summary>
public sealed record StartWheelSpinCommand : CodecCommand<StartWheelSpinCommand>, ICommandCodecProvider<StartWheelSpinCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartWheelSpinCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new StartWheelSpinCommand()));
}
