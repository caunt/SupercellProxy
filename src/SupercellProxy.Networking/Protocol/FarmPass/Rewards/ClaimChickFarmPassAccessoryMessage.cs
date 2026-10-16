using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Rewards;

/// <summary>Claims a Farm Pass chick accessory.</summary>
/// <param name="AvatarId">Account of the player claiming the accessory.</param>
public sealed record ClaimChickFarmPassAccessoryMessage(LongId AvatarId) : CodecMessage<ClaimChickFarmPassAccessoryMessage>, ICodecProvider<ClaimChickFarmPassAccessoryMessage>
{
    /// <inheritdoc />
    public static ICodec<ClaimChickFarmPassAccessoryMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.AvatarId, static avatarId => new ClaimChickFarmPassAccessoryMessage(avatarId));
}
