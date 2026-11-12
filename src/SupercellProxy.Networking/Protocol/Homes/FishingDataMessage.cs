using SupercellProxy.Networking.Protocol.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>Represents the own-fishing-home snapshot carried by clientbound message 24222.</summary>
/// <param name="Data">The fishing area payload in the <see cref="OwnHomeDataMessage"/> layout; its home document holds the player's lake objects instead of the farm's.</param>
public sealed record FishingDataMessage(OwnHomeDataMessage Data) : CodecMessage<FishingDataMessage>, ICodecProvider<FishingDataMessage>
{
    /// <inheritdoc />
    public static ICodec<FishingDataMessage> Codec { get; } = Codecs.Composite(OwnHomeDataMessage.Codec, static message => message.Data, static data => new FishingDataMessage(data));
}
