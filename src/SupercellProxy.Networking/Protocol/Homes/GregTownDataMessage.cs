using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Greg's-town snapshot (native <c>AITownDataMessage</c>), carried by clientbound message 24829 and loaded in native game mode 8.
/// </summary>
/// <param name="Data">The home snapshot, encoded like the player's own home data.</param>
public sealed record GregTownDataMessage(OwnHomeDataMessage Data) : CodecMessage<GregTownDataMessage>, ICodecProvider<GregTownDataMessage>
{
    /// <inheritdoc />
    public static ICodec<GregTownDataMessage> Codec { get; } = Codecs.Composite(OwnHomeDataMessage.Codec, static message => message.Data, static (data) => new GregTownDataMessage(data));
}
