using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Requests a task event leaderboard.</summary>
/// <param name="NeighborhoodId">The neighborhood id.</param>
/// <param name="EventId">The event id.</param>
/// <param name="VariantId">The variant id.</param>
/// <param name="Page">The page.</param>
/// <param name="PageSize">The page size.</param>
/// <param name="Global">The global.</param>
public sealed record RequestTaskEventLeaderboardMessage(LongId? NeighborhoodId, int EventId, int VariantId, int Page, int PageSize, bool Global) : CodecMessage<RequestTaskEventLeaderboardMessage>, ICodecProvider<RequestTaskEventLeaderboardMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestTaskEventLeaderboardMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static message => message.NeighborhoodId,
        Codecs.VarInt,
        static message => message.EventId,
        Codecs.VarInt,
        static message => message.VariantId,
        Codecs.VarInt,
        static message => message.Page,
        Codecs.VarInt,
        static message => message.PageSize,
        Codecs.Boolean,
        static message => message.Global,
        static (neighborhoodId, eventId, variantId, page, pageSize, global) => new RequestTaskEventLeaderboardMessage(neighborhoodId, eventId, variantId, page, pageSize, global)
    );
}
