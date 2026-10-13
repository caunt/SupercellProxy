using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.PrizeWheels;

/// <summary>Resets the wheel car.</summary>
public sealed record ResetWheelCarCommand : CodecCommand<ResetWheelCarCommand>, ICommandCodecProvider<ResetWheelCarCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ResetWheelCarCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new ResetWheelCarCommand()));
}
