using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Reports that the player followed a link out to the app store, naming their account.</summary>
/// <param name="AvatarId">Account of the player who followed the link.</param>
public sealed record ReportStoreLinkOutAvatarMessage(LongId AvatarId) : CodecMessage<ReportStoreLinkOutAvatarMessage>, ICodecProvider<ReportStoreLinkOutAvatarMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportStoreLinkOutAvatarMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static avatarId => new ReportStoreLinkOutAvatarMessage(avatarId));
}
