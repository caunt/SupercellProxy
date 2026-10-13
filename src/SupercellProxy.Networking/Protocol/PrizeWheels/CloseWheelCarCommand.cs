using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.PrizeWheels;

/// <summary>Closes the wheel car.</summary>
public sealed record CloseWheelCarCommand : CodecCommand<CloseWheelCarCommand>, ICommandCodecProvider<CloseWheelCarCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CloseWheelCarCommand> Codec { get; } = Codecs.Command(type: 590, Codecs.Unit(static () => new CloseWheelCarCommand()));
}
