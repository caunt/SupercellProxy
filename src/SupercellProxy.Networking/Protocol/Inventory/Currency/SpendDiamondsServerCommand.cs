using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory.Currency;

/// <summary>Applies a server-directed diamond debit.</summary>
public sealed record SpendDiamondsServerCommand(int Amount) : CodecServerCommand<SpendDiamondsServerCommand>, ICommandCodecProvider<SpendDiamondsServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpendDiamondsServerCommand> Codec { get; } = Codecs.Command(
        type: 379,
        Codecs.Composite(Codecs.VarInt, static command => command.Amount, static (amount) => new SpendDiamondsServerCommand(amount))
    );
}
