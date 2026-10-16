using SupercellProxy.Networking.Protocol.Friends.Social;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers;

/// <summary>Answers RequestHelpersMessage with the social profiles shown in the follower bar.</summary>
/// <param name="Helpers">Profiles of the players who helped; null when the list is absent.</param>
public sealed record HelperListMessage(ReadOnlyMemory<SocialProfile>? Helpers) : CodecMessage<HelperListMessage>, ICodecProvider<HelperListMessage>
{
    /// <inheritdoc />
    public static ICodec<HelperListMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(Friends.Social.SocialProfile.Codec),
        static message => message.Helpers,
        static helpers => new HelperListMessage(helpers)
    );
}
