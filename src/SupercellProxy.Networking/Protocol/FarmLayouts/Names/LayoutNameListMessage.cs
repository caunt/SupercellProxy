using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts.Names;

/// <summary>Lists the names of the farm layouts.</summary>
/// <param name="Names">Name of each layout slot in slot order; null when the list is absent.</param>
/// <param name="ActiveLayout">Slot of the layout the farm currently uses.</param>
public sealed record LayoutNameListMessage(ReadOnlyMemory<string?>? Names, int ActiveLayout) : CodecMessage<LayoutNameListMessage>, ICodecProvider<LayoutNameListMessage>
{
    /// <inheritdoc />
    public static ICodec<LayoutNameListMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(Codecs.OptionalString),
        static message => message.Names,
        Codecs.VarInt,
        static message => message.ActiveLayout,
        static (names, activeLayout) => new LayoutNameListMessage(names, activeLayout)
    );
}
