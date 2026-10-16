using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>Requests a scoreboard page.</summary>
/// <param name="ScoreboardId">Which scoreboard to send.</param>
/// <param name="Page">Which page of it to send.</param>
/// <param name="PageSize">How many lines one page holds.</param>
/// <param name="Region">Region filter for the board, as native numbers regions.</param>
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
