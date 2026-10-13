using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.PrizeWheels;

/// <summary>Buys a wheel car spin with diamonds.</summary>
public sealed record BuyWheelSpinWithDiamondsCommand : CodecCommand<BuyWheelSpinWithDiamondsCommand>, ICommandCodecProvider<BuyWheelSpinWithDiamondsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyWheelSpinWithDiamondsCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new BuyWheelSpinWithDiamondsCommand()));
}
