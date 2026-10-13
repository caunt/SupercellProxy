using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Derby;

/// <summary>Requests the avatar's derby league.</summary>
public sealed record RequestLeagueMessage : CodecMessage<RequestLeagueMessage>, ICodecProvider<RequestLeagueMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestLeagueMessage> Codec { get; } = Codecs.Unit(static () => new RequestLeagueMessage());
}
