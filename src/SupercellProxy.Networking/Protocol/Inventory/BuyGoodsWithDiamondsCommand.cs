using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>Buys missing goods with diamonds before the action that needs them.</summary>
/// <param name="Goods">The goods bought with diamonds.</param>
/// <param name="Reason">The resource change reason of the action that needed the goods.</param>
/// <param name="ReasonDataGlobalId">The data id the change reason refers to, or -1.</param>
/// <param name="ReasonValue">The additional value the change reason carries, or -1.</param>
public sealed record BuyGoodsWithDiamondsCommand(ReadOnlyMemory<DiamondGoodsPurchase> Goods, int Reason, int ReasonDataGlobalId, int ReasonValue) : CodecCommand<BuyGoodsWithDiamondsCommand>, ICommandCodecProvider<BuyGoodsWithDiamondsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyGoodsWithDiamondsCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Memory(DiamondGoodsPurchase.Codec),
            static command => command.Goods,
            Codecs.VarInt,
            static command => command.Reason,
            Codecs.VarInt,
            static command => command.ReasonDataGlobalId,
            Codecs.VarInt,
            static command => command.ReasonValue,
            static (goods, reason, reasonDataGlobalId, reasonValue) => new BuyGoodsWithDiamondsCommand(goods, reason, reasonDataGlobalId, reasonValue)
        )
    );
}
