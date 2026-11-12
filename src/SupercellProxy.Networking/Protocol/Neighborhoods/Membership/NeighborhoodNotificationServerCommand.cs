using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Delivers a neighborhood notification whose native effects are presentation callbacks.</summary>
/// <param name="Status">
/// Notification kind as a VarInt; 1 and 2 show one notice and 3 another, and the exact meaning of each is not identified.
/// </param>
/// <param name="NeighborhoodId">Id of the neighbourhood the notification is about.</param>
/// <param name="RelatedId">An optional second id sent with the notification, or <see langword="null"/>; what it names is not identified.</param>
public sealed record NeighborhoodNotificationServerCommand(int Status, LongId NeighborhoodId, LongId? RelatedId) : CodecServerCommand<NeighborhoodNotificationServerCommand>, ICommandCodecProvider<NeighborhoodNotificationServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<NeighborhoodNotificationServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.Status,
        Codecs.LongId,
        static command => command.NeighborhoodId,
        Codecs.OptionalLongId,
        static command => command.RelatedId,
        static (status, neighborhoodId, relatedId) => new NeighborhoodNotificationServerCommand(status, neighborhoodId, relatedId)));
}
