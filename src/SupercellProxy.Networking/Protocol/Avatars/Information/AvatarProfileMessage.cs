using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Information;

/// <summary>Answers an avatar profile request with the avatar's social ids and farm name.</summary>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="FacebookId">The facebook id.</param>
/// <param name="GamecenterId">The gamecenter id.</param>
/// <param name="SupercellId">The supercell id.</param>
/// <param name="FarmName">The farm name.</param>
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
