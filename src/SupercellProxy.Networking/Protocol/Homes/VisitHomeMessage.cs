using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">VisitHomeMessage</c> protocol message.
/// </summary>
public sealed record VisitHomeMessage : CodecMessage<VisitHomeMessage>, ICodecProvider<VisitHomeMessage>
{
    /// <inheritdoc />
    public static ICodec<VisitHomeMessage> Codec { get; } = Codecs.Composite(
        Codecs.Byte,
        static message => message.Unknown0,
        Codecs.Byte,
        static message => message.Unknown1,
        static (unknown0, unknown1) => new VisitHomeMessage { Unknown0 = unknown0, Unknown1 = unknown1 }
    );

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public required byte Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public required byte Unknown1 { get; init; }
}
