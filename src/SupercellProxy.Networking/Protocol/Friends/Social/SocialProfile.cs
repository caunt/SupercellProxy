using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Social;

/// <summary>A player's public card as social screens show it: linked accounts and farm name.</summary>
/// <param name="AvatarId">The player's account.</param>
/// <param name="FacebookId">Facebook user id linked to the account, if any.</param>
/// <param name="GamecenterId">Game Center player id linked to the account, if any.</param>
/// <param name="SupercellId">Supercell ID linked to the account, if any.</param>
/// <param name="FarmName">Name of the player's farm; null for a farm that was never named.</param>
/// <param name="Value0">First number after the farm name; its meaning is not identified.</param>
/// <param name="Value1">Second number after the farm name; not identified.</param>
/// <param name="Value2">Third number after the farm name; not identified.</param>
/// <param name="Value3">Fourth number after the farm name; not identified.</param>
public sealed record SocialProfile(
    LongId AvatarId,
    string? FacebookId,
    string? GamecenterId,
    string? SupercellId,
    string? FarmName,
    int Value0,
    int Value1,
    int Value2,
    int Value3
) : ICodecProvider<SocialProfile>
{
    /// <inheritdoc />
    public static ICodec<SocialProfile> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static value => value.AvatarId,
        Codecs.OptionalString,
        static value => value.FacebookId,
        Codecs.OptionalString,
        static value => value.GamecenterId,
        Codecs.OptionalString,
        static value => value.SupercellId,
        Codecs.OptionalString,
        static value => value.FarmName,
        Codecs.VarInt,
        static value => value.Value0,
        Codecs.VarInt,
        static value => value.Value1,
        Codecs.VarInt,
        static value => value.Value2,
        Codecs.VarInt,
        static value => value.Value3,
        static (avatarId, facebookId, gamecenterId, supercellId, farmName, value0, value1, value2, value3) => new SocialProfile(avatarId, facebookId, gamecenterId, supercellId, farmName, value0, value1, value2, value3)
    );
}
