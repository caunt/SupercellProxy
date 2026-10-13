using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>Requests an event leaderboard page.</summary>
/// <param name="EventId">The event id.</param>
/// <param name="VariantId">The variant id.</param>
/// <param name="Page">The page.</param>
/// <param name="PageSize">The page size.</param>
public sealed record RequestEventLeaderboardMessage(int EventId, int VariantId, int Page, int PageSize) : CodecMessage<RequestEventLeaderboardMessage>, ICodecProvider<RequestEventLeaderboardMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestEventLeaderboardMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.EventId,
        Codecs.VarInt,
        static message => message.VariantId,
        Codecs.VarInt,
        static message => message.Page,
        Codecs.VarInt,
        static message => message.PageSize,
        static (eventId, variantId, page, pageSize) => new RequestEventLeaderboardMessage(eventId, variantId, page, pageSize)
    );
}
