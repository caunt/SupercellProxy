using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Serverbound request to visit another player's farm; the server answers with the farm's data or a home-load failure.
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
    /// Home id of the farm to visit.
    /// </summary>
    public required LongId Target { get; init; }

    /// <summary>
    /// Number sent before the target; its meaning is not identified. The managed client sends 0.
    /// </summary>
    public required int Unknown0 { get; init; }
}
