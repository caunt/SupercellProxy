using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Rewards;

/// <summary>Requests the daily rewards.</summary>
public sealed record RequestDailyRewardsMessage : CodecMessage<RequestDailyRewardsMessage>, ICodecProvider<RequestDailyRewardsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestDailyRewardsMessage> Codec { get; } = Codecs.Unit(static () => new RequestDailyRewardsMessage());
}
