using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Purchases;

/// <summary>Cancels a pending package purchase.</summary>
/// <param name="EventId">Shop event that offered the package; 0 for the plain store.</param>
/// <param name="VariantId">Variant of that shop event.</param>
/// <param name="PackageGlobalId">Package table entry whose pending purchase is dropped.</param>
/// <param name="FarmPassPurchase">Farm Pass purchase slot the package was for; negative when it was not a Farm Pass purchase.</param>
public sealed record CancelShopPurchaseCommand(int EventId, int VariantId, int PackageGlobalId, int FarmPassPurchase) : CodecCommand<CancelShopPurchaseCommand>, ICommandCodecProvider<CancelShopPurchaseCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CancelShopPurchaseCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Int32,
            static command => command.EventId,
            Codecs.Int32,
            static command => command.VariantId,
            Codecs.Int32,
            static command => command.PackageGlobalId,
            Codecs.VarInt,
            static command => command.FarmPassPurchase,
            static (eventId, variantId, packageGlobalId, farmPassPurchase) => new CancelShopPurchaseCommand(eventId, variantId, packageGlobalId, farmPassPurchase)
        )
    );
}
