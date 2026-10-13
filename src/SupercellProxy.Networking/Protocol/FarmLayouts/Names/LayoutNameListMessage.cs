using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts.Names;

/// <summary>Lists the names of the farm layouts.</summary>
/// <param name="Names">The names.</param>
/// <param name="ActiveLayout">The active layout.</param>
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
