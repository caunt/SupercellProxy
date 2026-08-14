using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

public record OtherFishingHomeDataMessage : OtherHomeDataMessage
{
    [JsonIgnore]
    public Memory<byte> RawPayload { get; init; }

    public new static OtherFishingHomeDataMessage Create(MessageContainer container)
    {
        var rawPayload = container.Payload.ReadToEnd();
        var message = OtherHomeDataMessage.Decode(rawPayload);

        return new OtherFishingHomeDataMessage
        {
            HomeOwnerAvatar = message.HomeOwnerAvatar,
            Unknown0 = message.Unknown0,
            ClientAvatar = message.ClientAvatar,
            UnknownCompressedJson = message.UnknownCompressedJson,
            CompressedAvatarDataJson = message.CompressedAvatarDataJson,
            CompressedHomeDataJson = message.CompressedHomeDataJson,
            Fallback = message.Fallback,
            RawPayload = rawPayload
        };
    }
}
