using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.SupercellId;

/// <summary>An empty Supercell ID notice; the client ignores it.</summary>
public sealed record SupercellIdNoticeMessage : CodecMessage<SupercellIdNoticeMessage>, ICodecProvider<SupercellIdNoticeMessage>
{
    /// <inheritdoc />
    public static ICodec<SupercellIdNoticeMessage> Codec { get; } = Codecs.Unit(static () => new SupercellIdNoticeMessage());
}
