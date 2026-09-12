using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">OtherFishingHomeDataMessage</c> protocol message.
/// </summary>
public sealed record OtherFishingHomeDataMessage : OtherHomeDataMessage
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
        ArgumentNullException.ThrowIfNull(container);
        Memory<byte> rawPayload = container.Payload.ReadToEnd();
        OtherHomeDataMessage message = Decode(rawPayload);

        return new OtherFishingHomeDataMessage
        {
            HomeOwnerAvatar = message.HomeOwnerAvatar,
            Unknown0 = message.Unknown0,
            ClientAvatar = message.ClientAvatar,
            UnknownCompressedDocument = message.UnknownCompressedDocument,
            CompressedAvatarDataDocument = message.CompressedAvatarDataDocument,
            CompressedHomeDataDocument = message.CompressedHomeDataDocument,
            Fallback = message.Fallback,
            RawPayload = rawPayload,
        };
    }
}
