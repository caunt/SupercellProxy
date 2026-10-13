using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Purchases;

/// <summary>Confirms a pending package purchase.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="VariantId">The variant id.</param>
/// <param name="PackageGlobalId">The package data id.</param>
/// <param name="FarmPassPurchase">The farm pass purchase.</param>
public sealed record ConfirmShopPurchaseServerCommand(int EventId, int VariantId, int PackageGlobalId, int FarmPassPurchase) : CodecServerCommand<ConfirmShopPurchaseServerCommand>, ICommandCodecProvider<ConfirmShopPurchaseServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ConfirmShopPurchaseServerCommand> Codec { get; } = Codecs.Command(
        type: 350,
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
