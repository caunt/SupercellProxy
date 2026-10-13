using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Requests the data of the current game mode.</summary>
public sealed record RequestGameModeDataMessage : CodecMessage<RequestGameModeDataMessage>, ICodecProvider<RequestGameModeDataMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestGameModeDataMessage> Codec { get; } = Codecs.Unit(static () => new RequestGameModeDataMessage());
}
