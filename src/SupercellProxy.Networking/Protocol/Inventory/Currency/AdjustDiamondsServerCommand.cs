using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory.Currency;

/// <summary>Applies a server-directed diamond credit or debit.</summary>
public sealed record AdjustDiamondsServerCommand(int Amount, bool Credit) : CodecServerCommand<AdjustDiamondsServerCommand>, ICommandCodecProvider<AdjustDiamondsServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AdjustDiamondsServerCommand> Codec { get; } = Codecs.Command(
        type: 378,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Amount,
            Codecs.Boolean,
            static command => command.Credit,
            static (amount, credit) => new AdjustDiamondsServerCommand(amount, credit)
        )
    );
}
