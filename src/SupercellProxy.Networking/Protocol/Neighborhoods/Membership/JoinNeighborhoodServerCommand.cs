using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Confirms neighborhood creation or membership, including badge and next-league state.</summary>
/// <param name="NeighborhoodId">Id of the neighbourhood the player now belongs to.</param>
/// <param name="NeighborhoodName">Display name of the neighbourhood, or <see langword="null"/> when the server sends none.</param>
/// <param name="Created"><see langword="true"/> if the player founded the neighbourhood; otherwise, <see langword="false"/> for a plain join.</param>
/// <param name="BadgeBackground">Index of the badge background the neighbourhood uses.</param>
/// <param name="BadgePattern">Index of the badge pattern drawn over the background.</param>
/// <param name="BadgeSymbol">Index of the badge symbol drawn on top.</param>
/// <param name="Source">VarInt written after the badge; its meaning is not identified.</param>
/// <param name="NextLeagueIndex">Derby league the player will enter next in the new neighbourhood.</param>
/// <param name="CompleteNeighborhoodTutorials"><see langword="true"/> to complete the neighbourhood tutorial steps along with a creation; otherwise, <see langword="false"/>.</param>
/// <param name="ObjectEventId">Id of the neighbourhood event the new neighbourhood is in.</param>
/// <param name="ObjectVariantId">Variant id of that event.</param>
/// <param name="ObjectPoints">The new neighbourhood's combined event points.</param>
public sealed record JoinNeighborhoodServerCommand(LongId NeighborhoodId,
    string? NeighborhoodName,
    bool Created,
    int BadgeBackground,
    int BadgePattern,
    int BadgeSymbol,
    int Source,
    int NextLeagueIndex,
    bool CompleteNeighborhoodTutorials,
    int ObjectEventId,
    int ObjectVariantId,
    int ObjectPoints) : CodecServerCommand<JoinNeighborhoodServerCommand>, ICommandCodecProvider<JoinNeighborhoodServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<JoinNeighborhoodServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.LongId,
        static command => command.NeighborhoodId,
        Codecs.OptionalString,
        static command => command.NeighborhoodName,
        Codecs.Boolean,
        static command => command.Created,
        Codecs.VarInt,
        static command => command.BadgeBackground,
        Codecs.VarInt,
        static command => command.BadgePattern,
        Codecs.VarInt,
        static command => command.BadgeSymbol,
        Codecs.VarInt,
        static command => command.Source,
        Codecs.VarInt,
        static command => command.NextLeagueIndex,
        Codecs.Boolean,
        static command => command.CompleteNeighborhoodTutorials,
        Codecs.VarInt,
        static command => command.ObjectEventId,
        Codecs.VarInt,
        static command => command.ObjectVariantId,
        Codecs.VarInt,
        static command => command.ObjectPoints,
        static (neighborhoodId,
            neighborhoodName,
            created,
            badgeBackground,
            badgePattern,
            badgeSymbol,
            source,
            nextLeagueIndex,
            completeNeighborhoodTutorials,
            objectEventId,
            objectVariantId,
            objectPoints) => new JoinNeighborhoodServerCommand(neighborhoodId,
                neighborhoodName,
                created,
                badgeBackground,
                badgePattern,
                badgeSymbol,
                source,
                nextLeagueIndex,
                completeNeighborhoodTutorials,
                objectEventId,
                objectVariantId,
                objectPoints)));
}
