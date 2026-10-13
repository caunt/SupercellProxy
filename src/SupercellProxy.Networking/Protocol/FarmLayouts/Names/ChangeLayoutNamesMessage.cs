using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts.Names;

/// <summary>Renames a farm layout slot.</summary>
/// <param name="LayoutIndex">The layout index.</param>
/// <param name="NameIndex">The name index.</param>
public sealed record ChangeLayoutNamesMessage(int LayoutIndex, int NameIndex) : CodecMessage<ChangeLayoutNamesMessage>, ICodecProvider<ChangeLayoutNamesMessage>
{
    /// <inheritdoc />
    public static ICodec<ChangeLayoutNamesMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.LayoutIndex,
        Codecs.VarInt,
        static message => message.NameIndex,
        static (layoutIndex, nameIndex) => new ChangeLayoutNamesMessage(layoutIndex, nameIndex)
    );
}
