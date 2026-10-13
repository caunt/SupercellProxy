using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Purchases;

/// <summary>Starts a package purchase, keeping it pending until the server confirms or delivers it.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="VariantId">The variant id.</param>
/// <param name="PackageGlobalId">The package data id.</param>
/// <param name="Origin">The origin.</param>
/// <param name="FarmPassPurchase">The farm pass purchase.</param>
public sealed record RequestShopPurchaseCommand(int EventId, int VariantId, int PackageGlobalId, int Origin, int FarmPassPurchase) : CodecCommand<RequestShopPurchaseCommand>, ICommandCodecProvider<RequestShopPurchaseCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestShopPurchaseCommand> Codec { get; } = Codecs.Command(
        type: 227,
        Codecs.Composite(
            Codecs.Int32,
            static command => command.EventId,
            Codecs.Int32,
            static command => command.VariantId,
            Codecs.Int32,
            static command => command.PackageGlobalId,
            Codecs.VarInt,
            static command => command.Origin,
            Codecs.VarInt,
            static command => command.FarmPassPurchase,
            static (eventId, variantId, packageGlobalId, origin, farmPassPurchase) => new RequestShopPurchaseCommand(eventId, variantId, packageGlobalId, origin, farmPassPurchase)
        )
    );
}
