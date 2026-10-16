using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Requests a task event leaderboard.</summary>
/// <param name="NeighborhoodId">Neighborhood whose ranking to show; null when the request is not about one neighborhood.</param>
/// <param name="EventId">Task event the leaderboard belongs to.</param>
/// <param name="VariantId">Variant of that event the player is in.</param>
/// <param name="Page">Which page of the ranking to send.</param>
/// <param name="PageSize">How many entries one page holds.</param>
/// <param name="Global">Whether the ranking across all players is requested rather than the neighborhood one.</param>
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
