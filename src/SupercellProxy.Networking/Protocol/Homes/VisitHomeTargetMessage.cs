using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">VisitHomeTargetMessage</c> protocol message.
/// </summary>
public sealed record VisitHomeTargetMessage : CodecMessage<VisitHomeTargetMessage>, ICodecProvider<VisitHomeTargetMessage>
{
    /// <inheritdoc />
    public static ICodec<VisitHomeTargetMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Unknown0,
        Codecs.LongId,
        static message => message.Target,
        static (unknown0, target) => new VisitHomeTargetMessage { Unknown0 = unknown0, Target = target }
    );

    /// <summary>
    /// Gets or sets the <c language="csharp">Target</c> value.
    /// </summary>
    public required LongId Target { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public required int Unknown0 { get; init; }
}
