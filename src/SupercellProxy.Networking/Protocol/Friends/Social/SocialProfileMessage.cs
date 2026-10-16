using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Social;

/// <summary>Carries one optional social profile to the social manager's listeners.</summary>
/// <param name="Profile">The player's public card; null when the server sent none.</param>
public sealed record SocialProfileMessage(SocialProfile? Profile) : CodecMessage<SocialProfileMessage>, ICodecProvider<SocialProfileMessage>
{
    /// <inheritdoc />
    public static ICodec<SocialProfileMessage> Codec { get; } = Codecs.Composite(Codecs.Optional(SocialProfile.Codec), static message => message.Profile, static profile => new SocialProfileMessage(profile));
}
