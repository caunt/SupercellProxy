using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts.Names;

/// <summary>Selects an edit mode layout slot.</summary>
/// <param name="SlotIndex">The slot index.</param>
public sealed record SelectEditModeSlotMessage(int SlotIndex) : CodecMessage<SelectEditModeSlotMessage>, ICodecProvider<SelectEditModeSlotMessage>
{
    /// <inheritdoc />
    public static ICodec<SelectEditModeSlotMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.SlotIndex, static (slotIndex) => new SelectEditModeSlotMessage(slotIndex));
}
