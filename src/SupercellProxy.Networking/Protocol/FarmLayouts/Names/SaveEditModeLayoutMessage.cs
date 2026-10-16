using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts.Names;

/// <summary>Saves an edit mode layout.</summary>
/// <param name="Name">Name to store the layout under.</param>
/// <param name="Layout">The layout as the client serialised it (object positions and the like).</param>
/// <param name="SlotIndex">Layout slot to save into.</param>
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
