using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Purchases;

/// <summary>Confirms a pending package purchase.</summary>
/// <param name="EventId">Shop event that offered the package; 0 leaves the purchase unconfirmed.</param>
/// <param name="VariantId">Variant of that shop event.</param>
/// <param name="PackageGlobalId">Package table entry whose pending purchase is confirmed.</param>
/// <param name="FarmPassPurchase">Farm Pass purchase slot the package is for; negative when it is not a Farm Pass purchase.</param>
public sealed record ConfirmShopPurchaseServerCommand(int EventId, int VariantId, int PackageGlobalId, int FarmPassPurchase) : CodecServerCommand<ConfirmShopPurchaseServerCommand>, ICommandCodecProvider<ConfirmShopPurchaseServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ConfirmShopPurchaseServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Int32,
            static command => command.EventId,
            Codecs.Int32,
            static command => command.VariantId,
            Codecs.Int32,
            static command => command.PackageGlobalId,
            Codecs.VarInt,
            static command => command.FarmPassPurchase,
            static (eventId, variantId, packageGlobalId, farmPassPurchase) => new ConfirmShopPurchaseServerCommand(eventId, variantId, packageGlobalId, farmPassPurchase)
        )
    );
}
