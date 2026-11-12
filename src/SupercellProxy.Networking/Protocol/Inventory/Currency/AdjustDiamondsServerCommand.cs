using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory.Currency;

/// <summary>Applies a server-directed diamond credit or debit.</summary>
/// <param name="Amount">Number of diamonds to add or take, written first as a VarInt.</param>
/// <param name="Credit"><see langword="true"/> to grant the diamonds to the player; <see langword="false"/> to spend them. Written as a Boolean after the amount.</param>
public sealed record AdjustDiamondsServerCommand(int Amount, bool Credit) : CodecServerCommand<AdjustDiamondsServerCommand>, ICommandCodecProvider<AdjustDiamondsServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AdjustDiamondsServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.Amount,
        Codecs.Boolean,
        static command => command.Credit,
        static (amount, credit) => new AdjustDiamondsServerCommand(amount, credit)));
}
