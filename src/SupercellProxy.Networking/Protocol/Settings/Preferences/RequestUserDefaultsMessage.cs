using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings.Preferences;

/// <summary>Requests the stored user defaults.</summary>
public sealed record RequestUserDefaultsMessage : CodecMessage<RequestUserDefaultsMessage>, ICodecProvider<RequestUserDefaultsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestUserDefaultsMessage> Codec { get; } = Codecs.Unit(static () => new RequestUserDefaultsMessage());
}
