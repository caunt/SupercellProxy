using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Social;

/// <summary>The social profile.</summary>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="FacebookId">The facebook id.</param>
/// <param name="GamecenterId">The gamecenter id.</param>
/// <param name="SupercellId">The supercell id.</param>
/// <param name="FarmName">The farm name.</param>
/// <param name="Value0">The value0.</param>
/// <param name="Value1">The value1.</param>
/// <param name="Value2">The value2.</param>
/// <param name="Value3">The value3.</param>
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
