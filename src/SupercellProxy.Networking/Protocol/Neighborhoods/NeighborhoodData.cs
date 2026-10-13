using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Represents <c language="csharp">NeighborhoodData</c>.
/// </summary>
/// <param name="NeighborhoodId">The <c language="csharp">NeighborhoodId</c> value.</param>
/// <param name="NeighborhoodName">The <c language="csharp">NeighborhoodName</c> value.</param>
/// <param name="NeighborhoodRole">The <c language="csharp">NeighborhoodRole</c> value.</param>
/// <param name="BadgeBackground">The badge background: neighborhood_badges.csv global id with the color in the high byte.</param>
/// <param name="BadgePattern">The badge pattern: neighborhood_badges.csv global id with the color in the high byte.</param>
/// <param name="BadgeSymbol">The badge symbol: neighborhood_badges.csv global id with the color in the high byte.</param>
/// <param name="ObjectEventId">The live event id of the neighborhood object the points belong to.</param>
/// <param name="ObjectVariantId">The live event variant id of the neighborhood object.</param>
/// <param name="ObjectPoints">The neighborhood object points; native maps them to a level through the event's point thresholds.</param>
public sealed record NeighborhoodData(
    [property: System.Text.Json.Serialization.JsonPropertyName("NeighborhoodId")] LongId NeighborhoodId,
    string? NeighborhoodName,
    int NeighborhoodRole,
    int BadgeBackground,
    int BadgePattern,
    int BadgeSymbol,
    int ObjectEventId,
    int ObjectVariantId,
    int ObjectPoints
) : ICodecProvider<NeighborhoodData>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodData> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static value => value.NeighborhoodId,
        Codecs.OptionalString,
        static value => value.NeighborhoodName,
        Codecs.VarInt,
        static value => value.NeighborhoodRole,
        Codecs.VarInt,
        static value => value.BadgeBackground,
        Codecs.VarInt,
        static value => value.BadgePattern,
        Codecs.VarInt,
        static value => value.BadgeSymbol,
        Codecs.VarInt,
        static value => value.ObjectEventId,
        Codecs.VarInt,
        static value => value.ObjectVariantId,
        Codecs.VarInt,
        static value => value.ObjectPoints,
        static (
            neighborhoodId,
            neighborhoodName,
            neighborhoodRole,
            badgeBackground,
            badgePattern,
            badgeSymbol,
            objectEventId,
            objectVariantId,
            objectPoints
        ) => new NeighborhoodData(
            neighborhoodId,
            neighborhoodName,
            neighborhoodRole,
            badgeBackground,
            badgePattern,
            badgeSymbol,
            objectEventId,
            objectVariantId,
            objectPoints
        )
    );
}
