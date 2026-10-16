using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Directory;

/// <summary>One player in a neighbourhood comparison, with their neighbourhood and three compared numbers.</summary>
/// <param name="AvatarId">Account of the player.</param>
/// <param name="HomeId">Home of the player.</param>
/// <param name="NeighborhoodId">Neighbourhood the player belongs to.</param>
/// <param name="Name">Name shown for the entry.</param>
/// <param name="Badge">Emblem of the player's neighbourhood.</param>
/// <param name="Id">Further id of the entry; its meaning is not identified.</param>
/// <param name="Value0">First compared number; its meaning is not identified.</param>
/// <param name="Value1">Second compared number; its meaning is not identified.</param>
/// <param name="Value2">Third compared number; its meaning is not identified.</param>
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
