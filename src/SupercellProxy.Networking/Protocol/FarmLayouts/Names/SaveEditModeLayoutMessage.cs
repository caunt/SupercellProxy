using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts.Names;

/// <summary>Saves an edit mode layout.</summary>
/// <param name="Name">The name.</param>
/// <param name="Layout">The layout.</param>
/// <param name="SlotIndex">The slot index.</param>
public sealed record SaveEditModeLayoutMessage(string? Name, string? Layout, int SlotIndex) : CodecMessage<SaveEditModeLayoutMessage>, ICodecProvider<SaveEditModeLayoutMessage>
{
    /// <inheritdoc />
    public static ICodec<SaveEditModeLayoutMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.Name,
        Codecs.OptionalString,
        static message => message.Layout,
        Codecs.VarInt,
        static message => message.SlotIndex,
        static (name, layout, slotIndex) => new SaveEditModeLayoutMessage(name, layout, slotIndex)
    );
}
