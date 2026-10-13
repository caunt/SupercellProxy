using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Decoration.Gallery;

/// <summary>Reports a decoration design.</summary>
/// <param name="DesignId">The design id.</param>
public sealed record ReportDecorationDesignMessage(LongId DesignId) : CodecMessage<ReportDecorationDesignMessage>, ICodecProvider<ReportDecorationDesignMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportDecorationDesignMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.DesignId, static (designId) => new ReportDecorationDesignMessage(designId));
}
