using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// <summary>
/// Represents the <c language="csharp">OtherFishingHomeDataMessage</c> protocol message.
/// </summary>
internal sealed record OtherFishingHomeDataMessage : OtherHomeDataMessage
{
    /// <summary>
    /// Gets or sets the <c language="csharp">RawPayload</c> value.
    /// </summary>
    [JsonIgnore]
    public Memory<byte> RawPayload { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">OtherFishingHomeDataMessage</c> from the supplied data.
    /// </summary>
    public static new OtherFishingHomeDataMessage Create(MessageContainer container)
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
            RawPayload = rawPayload,
        };
    }
}
