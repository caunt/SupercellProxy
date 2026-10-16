using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.DeviceLinking;

/// <summary>Carries a device link text.</summary>
/// <param name="Text">Free text the server attaches to the device-link flow; null when absent.</param>
public sealed record DeviceLinkTextMessage(string? Text) : CodecMessage<DeviceLinkTextMessage>, ICodecProvider<DeviceLinkTextMessage>
{
    /// <inheritdoc />
    public static ICodec<DeviceLinkTextMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalString, static message => message.Text, static text => new DeviceLinkTextMessage(text));
}
