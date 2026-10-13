using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops.AiShop;

/// <summary>Rerolls the goods of the AI roadside shop.</summary>
public sealed record RerollAiRoadsideShopCommand : CodecCommand<RerollAiRoadsideShopCommand>, ICommandCodecProvider<RerollAiRoadsideShopCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RerollAiRoadsideShopCommand> Codec { get; } = Codecs.Command(type: 622, Codecs.Unit(static () => new RerollAiRoadsideShopCommand()));
}
