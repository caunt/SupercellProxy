using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Buys an edit-mode layout slot for the current home.</summary>
/// <param name="SlotIndex">The bought layout slot index.</param>
public sealed record PurchaseLayoutSlotCommand(int SlotIndex) : CodecCommand<PurchaseLayoutSlotCommand>, ICommandCodecProvider<PurchaseLayoutSlotCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PurchaseLayoutSlotCommand> Codec { get; } = Codecs.Command(
        type: 185,
        Codecs.Composite(Codecs.VarInt, static command => command.SlotIndex, static (slotIndex) => new PurchaseLayoutSlotCommand(slotIndex))
    );
}
