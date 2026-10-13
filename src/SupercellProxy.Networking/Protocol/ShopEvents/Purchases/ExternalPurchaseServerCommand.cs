using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Purchases;

/// <summary>Reports a purchase made outside the game.</summary>
/// <param name="ProductId">The product id.</param>
/// <param name="Amount">The amount.</param>
public sealed record ExternalPurchaseServerCommand(string? ProductId, int Amount) : CodecServerCommand<ExternalPurchaseServerCommand>, ICommandCodecProvider<ExternalPurchaseServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ExternalPurchaseServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.OptionalString,
            static command => command.ProductId,
            Codecs.Int32,
            static command => command.Amount,
            static (productId, amount) => new ExternalPurchaseServerCommand(productId, amount)
        )
    );
}
