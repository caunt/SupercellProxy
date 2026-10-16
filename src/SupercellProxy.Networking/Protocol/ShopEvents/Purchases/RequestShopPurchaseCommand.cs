using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Purchases;

/// <summary>Starts a package purchase, keeping it pending until the server confirms or delivers it.</summary>
/// <param name="EventId">Shop event offering the package; 0 for the plain store.</param>
/// <param name="VariantId">Variant of that shop event.</param>
/// <param name="PackageGlobalId">Package table entry the player is buying.</param>
/// <param name="Origin">Where in the game the purchase started, as native numbers purchase origins.</param>
/// <param name="FarmPassPurchase">Farm Pass purchase slot the package is for; negative when it is not a Farm Pass purchase.</param>
public sealed record RequestShopPurchaseCommand(int EventId, int VariantId, int PackageGlobalId, int Origin, int FarmPassPurchase) : CodecCommand<RequestShopPurchaseCommand>, ICommandCodecProvider<RequestShopPurchaseCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestShopPurchaseCommand> Codec { get; } = Codecs.Command(
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
