using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Information;

/// <summary>One avatar information entry.</summary>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="FacebookId">The facebook id.</param>
/// <param name="GamecenterId">The gamecenter id.</param>
/// <param name="SupercellId">The supercell id.</param>
/// <param name="FarmName">The farm name.</param>
public sealed record AvatarInformation(LongId AvatarId, string? FacebookId, string? GamecenterId, string? SupercellId, string? FarmName) : ICodecProvider<AvatarInformation>
{
    /// <inheritdoc />
    public static ICodec<AvatarInformation> Codec { get; } = Codecs.Composite(
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
        static (avatarId, facebookId, gamecenterId, supercellId, farmName) => new AvatarInformation(avatarId, facebookId, gamecenterId, supercellId, farmName)
    );
}
