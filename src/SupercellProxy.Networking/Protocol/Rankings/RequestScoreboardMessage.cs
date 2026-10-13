using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>Requests a scoreboard page.</summary>
/// <param name="ScoreboardId">The scoreboard id.</param>
/// <param name="Page">The page.</param>
/// <param name="PageSize">The page size.</param>
/// <param name="Region">The region.</param>
public sealed record RequestScoreboardMessage(int ScoreboardId, int Page, int PageSize, int Region) : CodecMessage<RequestScoreboardMessage>, ICodecProvider<RequestScoreboardMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestScoreboardMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.ScoreboardId,
        Codecs.VarInt,
        static message => message.Page,
        Codecs.VarInt,
        static message => message.PageSize,
        Codecs.VarInt,
        static message => message.Region,
        static (scoreboardId, page, pageSize, region) => new RequestScoreboardMessage(scoreboardId, page, pageSize, region)
    );
}
