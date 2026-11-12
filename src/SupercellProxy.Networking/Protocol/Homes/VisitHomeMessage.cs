using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">VisitHomeMessage</c> protocol message.
/// </summary>
public sealed record VisitHomeMessage : CodecMessage<VisitHomeMessage>, ICodecProvider<VisitHomeMessage>
{
    /// <inheritdoc />
    public static ICodec<VisitHomeMessage> Codec { get; } = Codecs.Composite(Codecs.Byte,
        static message => message.Unknown0,
        Codecs.Byte,
        static message => message.Unknown1,
        static (unknown0, unknown1) => new VisitHomeMessage { Unknown0 = unknown0, Unknown1 = unknown1 });

    /// <summary>
    /// Gets or sets the first of the two bytes the message carries; its meaning is not identified.
    /// </summary>
    /// <value>One raw byte at wire position 0, kept as received so the message can be forwarded unchanged; the message exists only in the 1.72.86 protocol.</value>
    public required byte Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the second of the two bytes; not identified.
    /// </summary>
    /// <value>One raw byte at wire position 1, read right after <see cref="Unknown0"/> and kept as received.</value>
    public required byte Unknown1 { get; init; }
}
