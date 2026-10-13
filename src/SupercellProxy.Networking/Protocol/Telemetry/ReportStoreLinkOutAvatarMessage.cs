using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Reports a store link-out for an avatar.</summary>
/// <param name="AvatarId">The avatar id.</param>
public sealed record ReportStoreLinkOutAvatarMessage(LongId AvatarId) : CodecMessage<ReportStoreLinkOutAvatarMessage>, ICodecProvider<ReportStoreLinkOutAvatarMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportStoreLinkOutAvatarMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static (avatarId) => new ReportStoreLinkOutAvatarMessage(avatarId));
}
