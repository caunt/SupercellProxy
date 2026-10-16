using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Information;

/// <summary>Answers an avatar profile request with the avatar's social ids and farm name.</summary>
/// <param name="AvatarId">Account the profile describes.</param>
/// <param name="FacebookId">Facebook user id linked to that account, if any.</param>
/// <param name="GamecenterId">Game Center player id linked to that account, if any.</param>
/// <param name="SupercellId">Supercell ID linked to that account, if any.</param>
/// <param name="FarmName">Name of that player's farm; null for a farm that was never named.</param>
public sealed record AvatarProfileMessage(LongId AvatarId, string? FacebookId, string? GamecenterId, string? SupercellId, string? FarmName) : CodecMessage<AvatarProfileMessage>, ICodecProvider<AvatarProfileMessage>
{
    /// <inheritdoc />
    public static ICodec<AvatarProfileMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.AvatarId,
        Codecs.OptionalString,
        static message => message.FacebookId,
        Codecs.OptionalString,
        static message => message.GamecenterId,
        Codecs.OptionalString,
        static message => message.SupercellId,
        Codecs.OptionalString,
        static message => message.FarmName,
        static (avatarId, facebookId, gamecenterId, supercellId, farmName) => new AvatarProfileMessage(avatarId, facebookId, gamecenterId, supercellId, farmName)
    );
}
