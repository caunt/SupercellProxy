using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Represents the <c language="csharp">OtherFishingHomeDataMessage</c> protocol message.
/// </summary>
public sealed record OtherFishingHomeDataMessage : OtherHomeDataMessage, ICodecProvider<OtherFishingHomeDataMessage>
{
    /// <inheritdoc />
    public static new ICodec<OtherFishingHomeDataMessage> Codec { get; } = Codecs.Create(
        static stream =>
        {
            ReadOnlyMemory<byte> rawPayload = stream.ReadToEnd();
            OtherHomeDataMessage message = FromPayload(rawPayload, stream.GameVersion);

            return new OtherFishingHomeDataMessage
            {
                HomeOwnerAvatar = message.HomeOwnerAvatar,
                ServerTimestamp = message.ServerTimestamp,
                ClientAvatar = message.ClientAvatar,
                CompressedHomeOwnerAvatarDataDocument = message.CompressedHomeOwnerAvatarDataDocument,
                CompressedAvatarDataDocument = message.CompressedAvatarDataDocument,
                CompressedHomeDataDocument = message.CompressedHomeDataDocument,
                Fallback = message.Fallback,
                DecodeError = message.DecodeError,
                RawPayload = rawPayload,
            };
        },
        static (stream, message) => OtherHomeDataMessage.Codec.Write(stream, message)
    );

    /// <summary>
    /// Gets or sets the <c language="csharp">RawPayload</c> value.
    /// </summary>
    [JsonIgnore]
    public ReadOnlyMemory<byte> RawPayload { get; init; }
}
