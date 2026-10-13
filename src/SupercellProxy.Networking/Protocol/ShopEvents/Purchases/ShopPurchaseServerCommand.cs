using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Purchases;

/// <summary>Delivers a purchased package and its currencies and goods.</summary>
/// <param name="Diamonds">The diamonds.</param>
/// <param name="Cash">The cash.</param>
/// <param name="Gift">The gift.</param>
/// <param name="ProductId">The product id.</param>
/// <param name="PackageGlobalId">The package data id.</param>
/// <param name="Goods">The goods.</param>
/// <param name="ExtraRewards">The extra rewards.</param>
/// <param name="RewardMode">The reward mode.</param>
/// <param name="FarmPassPurchase">The farm pass purchase.</param>
/// <param name="EventId">The event id.</param>
/// <param name="VariantId">The variant id.</param>
/// <param name="TrackPurchaseTotals">The track purchase totals.</param>
/// <param name="Origin">The origin.</param>
/// <param name="GachaCurrency">The gacha currency.</param>
/// <param name="UseStepIndex">The use step index.</param>
/// <param name="StepIndex">The step index.</param>
public sealed record ShopPurchaseServerCommand(
    int Diamonds,
    int Cash,
    bool Gift,
    string? ProductId,
    int PackageGlobalId,
    ReadOnlyMemory<ShopPurchaseGood> Goods,
    ReadOnlyMemory<ShopPurchaseGood> ExtraRewards,
    int RewardMode,
    int FarmPassPurchase,
    int EventId,
    int VariantId,
    bool TrackPurchaseTotals,
    int Origin,
    int GachaCurrency,
    bool UseStepIndex,
    int StepIndex
) : CodecServerCommand<ShopPurchaseServerCommand>, ICommandCodecProvider<ShopPurchaseServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ShopPurchaseServerCommand> Codec { get; } = Codecs.Command(
        type: 229,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Diamonds,
            Codecs.VarInt,
            static command => command.Cash,
            Codecs.Boolean,
            static command => command.Gift,
            Codecs.OptionalString,
            static command => command.ProductId,
            Codecs.VarInt,
            static command => command.PackageGlobalId,
            Codecs.Memory(ShopPurchaseGood.Codec),
            static command => command.Goods,
            Codecs.Memory(ShopPurchaseGood.Codec),
            static command => command.ExtraRewards,
            Codecs.VarInt,
            static command => command.RewardMode,
            Codecs.VarInt,
            static command => command.FarmPassPurchase,
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.VariantId,
            Codecs.Boolean,
            static command => command.TrackPurchaseTotals,
            Codecs.VarInt,
            static command => command.Origin,
            Codecs.VarInt,
            static command => command.GachaCurrency,
            Codecs.Boolean,
            static command => command.UseStepIndex,
            Codecs.VarInt,
            static command => command.StepIndex,
            static (
                diamonds,
                cash,
                gift,
                productId,
                packageGlobalId,
                goods,
                extraRewards,
                rewardMode,
                farmPassPurchase,
                eventId,
                variantId,
                trackPurchaseTotals,
                origin,
                gachaCurrency,
                useStepIndex,
                stepIndex
            ) => new ShopPurchaseServerCommand(
                diamonds,
                cash,
                gift,
                productId,
                packageGlobalId,
                goods,
                extraRewards,
                rewardMode,
                farmPassPurchase,
                eventId,
                variantId,
                trackPurchaseTotals,
                origin,
                gachaCurrency,
                useStepIndex,
                stepIndex
            )
        )
    );
}
