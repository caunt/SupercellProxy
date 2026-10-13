using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Confirms neighborhood creation or membership, including badge and next-league state.</summary>
public sealed record JoinNeighborhoodServerCommand(
    LongId NeighborhoodId,
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
    int ObjectPoints
) : CodecServerCommand<JoinNeighborhoodServerCommand>, ICommandCodecProvider<JoinNeighborhoodServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<JoinNeighborhoodServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
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
            static (
                neighborhoodId,
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
                objectPoints
            ) => new JoinNeighborhoodServerCommand(
                neighborhoodId,
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
                objectPoints
            )
        )
    );
}
