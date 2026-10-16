using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Information;

/// <summary>One avatar information entry.</summary>
/// <param name="AvatarId">Account the entry describes.</param>
/// <param name="FacebookId">Facebook user id linked to that account, if any.</param>
/// <param name="GamecenterId">Game Center player id linked to that account, if any.</param>
/// <param name="SupercellId">Supercell ID linked to that account, if any.</param>
/// <param name="FarmName">Name of that player's farm; null for a farm that was never named.</param>
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
