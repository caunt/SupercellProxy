using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Directory;

/// <summary>One neighborhood comparison entry entry.</summary>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="HomeId">The home id.</param>
/// <param name="NeighborhoodId">The neighborhood id.</param>
/// <param name="Name">The name.</param>
/// <param name="Badge">The badge.</param>
/// <param name="Id">The id.</param>
/// <param name="Value0">The value0.</param>
/// <param name="Value1">The value1.</param>
/// <param name="Value2">The value2.</param>
public sealed record NeighborhoodComparisonEntry(
    LongId? AvatarId,
    LongId? HomeId,
    LongId? NeighborhoodId,
    string? Name,
    NeighborhoodBadge? Badge,
    LongId Id,
    int Value0,
    int Value1,
    int Value2
) : ICodecProvider<NeighborhoodComparisonEntry>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodComparisonEntry> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static value => value.AvatarId,
        Codecs.OptionalLongId,
        static value => value.HomeId,
        Codecs.OptionalLongId,
        static value => value.NeighborhoodId,
        Codecs.OptionalString,
        static value => value.Name,
        Codecs.Optional(NeighborhoodBadge.Codec),
        static value => value.Badge,
        Codecs.LongId,
        static value => value.Id,
        Codecs.VarInt,
        static value => value.Value0,
        Codecs.VarInt,
        static value => value.Value1,
        Codecs.VarInt,
        static value => value.Value2,
        static (avatarId, homeId, neighborhoodId, name, badge, id, value0, value1, value2) => new NeighborhoodComparisonEntry(avatarId, homeId, neighborhoodId, name, badge, id, value0, value1, value2)
    );
}
