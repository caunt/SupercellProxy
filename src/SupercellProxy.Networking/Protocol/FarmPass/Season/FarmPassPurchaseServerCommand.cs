using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Season;

/// <summary>Delivers a Farm Pass purchase.</summary>
/// <param name="PassType">The pass type.</param>
/// <param name="ProductId">The product id.</param>
/// <param name="TransactionId">The transaction id.</param>
/// <param name="Receipt">The receipt.</param>
/// <param name="Timestamp">The timestamp.</param>
public sealed record FarmPassPurchaseServerCommand(int PassType, string? ProductId, string? TransactionId, string? Receipt, int Timestamp) : CodecServerCommand<FarmPassPurchaseServerCommand>, ICommandCodecProvider<FarmPassPurchaseServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FarmPassPurchaseServerCommand> Codec { get; } = Codecs.Command(
        type: 396,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.PassType,
            Codecs.OptionalString,
            static command => command.ProductId,
            Codecs.OptionalString,
            static command => command.TransactionId,
            Codecs.OptionalString,
            static command => command.Receipt,
            Codecs.VarInt,
            static command => command.Timestamp,
            static (passType, productId, transactionId, receipt, timestamp) => new FarmPassPurchaseServerCommand(passType, productId, transactionId, receipt, timestamp)
        )
    );
}
