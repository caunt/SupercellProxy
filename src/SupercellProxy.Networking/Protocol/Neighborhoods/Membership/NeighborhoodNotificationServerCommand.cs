using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Delivers a neighborhood notification whose native effects are presentation callbacks.</summary>
public sealed record NeighborhoodNotificationServerCommand(int Status, LongId NeighborhoodId, LongId? RelatedId) : CodecServerCommand<NeighborhoodNotificationServerCommand>, ICommandCodecProvider<NeighborhoodNotificationServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodNotificationServerCommand> Codec { get; } = Codecs.Command(
        type: 182,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Status,
            Codecs.LongId,
            static command => command.NeighborhoodId,
            Codecs.OptionalLongId,
            static command => command.RelatedId,
            static (status, neighborhoodId, relatedId) => new NeighborhoodNotificationServerCommand(status, neighborhoodId, relatedId)
        )
    );
}
