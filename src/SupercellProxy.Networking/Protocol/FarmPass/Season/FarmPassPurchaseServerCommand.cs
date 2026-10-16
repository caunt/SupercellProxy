using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Season;

/// <summary>Delivers a Farm Pass bought in the app store; the client logic does not port this command yet.</summary>
/// <param name="PassType">Which pass was bought; its values are not identified.</param>
/// <param name="ProductId">App store product the pass was bought as.</param>
/// <param name="TransactionId">App store transaction of the purchase.</param>
/// <param name="Receipt">App store receipt of the purchase.</param>
/// <param name="Timestamp">When the purchase was made; its unit is not identified.</param>
public sealed record FarmPassPurchaseServerCommand(int PassType, string? ProductId, string? TransactionId, string? Receipt, int Timestamp) : CodecServerCommand<FarmPassPurchaseServerCommand>, ICommandCodecProvider<FarmPassPurchaseServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FarmPassPurchaseServerCommand> Codec { get; } = Codecs.Command(
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
