using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rankings;

/// <summary>Requests a town leaderboard.</summary>
/// <param name="Category">Which town ranking to send, as native numbers them.</param>
public sealed record RequestTownLeaderboardMessage(int Category) : CodecMessage<RequestTownLeaderboardMessage>, ICodecProvider<RequestTownLeaderboardMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestTownLeaderboardMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.Category, static category => new RequestTownLeaderboardMessage(category));
}
