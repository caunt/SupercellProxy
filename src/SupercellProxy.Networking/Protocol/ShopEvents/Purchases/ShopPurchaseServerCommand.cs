using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Purchases;

/// <summary>Delivers a purchased package and its currencies and goods.</summary>
/// <param name="Diamonds">Diamonds added to the player's balance.</param>
/// <param name="Cash">Coins added to the player's balance.</param>
/// <param name="Gift">Whether the package was a gift rather than a purchase, which changes the reward reason recorded.</param>
/// <param name="ProductId">Store product that was bought.</param>
/// <param name="PackageGlobalId">Diamond or coin package table entry that was bought; it is also added to the inventory as owned.</param>
/// <param name="Goods">Goods the package grants; negative amounts take goods away (vouchers are spent).</param>
/// <param name="ExtraRewards">Additional rewards on top of the goods, used by special offers.</param>
/// <param name="RewardMode">0 for an ordinary event package; other values deliver storage packages (barn and silo upgrades).</param>
/// <param name="FarmPassPurchase">Farm Pass purchase slot the package fills; negative when it is not a Farm Pass purchase.</param>
/// <param name="EventId">Shop event that offered the package; 0 when it came from the plain store.</param>
/// <param name="VariantId">Variant of that shop event.</param>
/// <param name="TrackPurchaseTotals">Whether the diamonds and coins also count towards the player's lifetime purchase totals.</param>
/// <param name="Origin">Where in the game the purchase started, as native numbers purchase origins.</param>
/// <param name="GachaCurrency">Gacha currency added to the player's balance.</param>
/// <param name="UseStepIndex">Whether <paramref name="StepIndex"/> applies to this purchase; such special purchases are not ported yet.</param>
/// <param name="StepIndex">Offer step the purchase belongs to, when <paramref name="UseStepIndex"/> is set.</param>
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
