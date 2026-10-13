using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Seen;

/// <summary>Buys a Valley fuel wheel spin with diamonds.</summary>
public sealed record BuyMapGameFuelSpinCommand : CodecCommand<BuyMapGameFuelSpinCommand>, ICommandCodecProvider<BuyMapGameFuelSpinCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyMapGameFuelSpinCommand> Codec { get; } = Codecs.Command(type: 2591, Codecs.Unit(static () => new BuyMapGameFuelSpinCommand()));
}
