using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Purchases;

/// <summary>Cancels a pending package purchase.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="VariantId">The variant id.</param>
/// <param name="PackageGlobalId">The package data id.</param>
/// <param name="FarmPassPurchase">The farm pass purchase.</param>
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
