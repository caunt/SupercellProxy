using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>Confirms a login from a confirmation popup.</summary>
/// <param name="AvatarId">Account whose login the player confirmed in the popup.</param>
public sealed record ConfirmLoginMessage(LongId AvatarId) : CodecMessage<ConfirmLoginMessage>, ICodecProvider<ConfirmLoginMessage>
{
    /// <inheritdoc />
    public static ICodec<ConfirmLoginMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static avatarId => new ConfirmLoginMessage(avatarId));
}
