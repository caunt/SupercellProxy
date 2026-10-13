using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.DeviceLinking;

/// <summary>An empty device link notice; the client ignores it.</summary>
public sealed record DeviceLinkNoticeMessage : CodecMessage<DeviceLinkNoticeMessage>, ICodecProvider<DeviceLinkNoticeMessage>
{
    /// <inheritdoc />
    public static ICodec<DeviceLinkNoticeMessage> Codec { get; } = Codecs.Unit(static () => new DeviceLinkNoticeMessage());
}
