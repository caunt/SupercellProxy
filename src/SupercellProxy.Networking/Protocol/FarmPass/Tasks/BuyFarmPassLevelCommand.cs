using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Tasks;

/// <summary>Buys the next Farm Pass level with diamonds.</summary>
/// <param name="Level">The current level the purchase is made from; native rejects any other value.</param>
public sealed record BuyFarmPassLevelCommand(int Level) : CodecCommand<BuyFarmPassLevelCommand>, ICommandCodecProvider<BuyFarmPassLevelCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyFarmPassLevelCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.Level, static (level) => new BuyFarmPassLevelCommand(level)));
}
