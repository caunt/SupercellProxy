using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat.Reactions;

/// <summary>Sends a chat emote to the neighborhood.</summary>
/// <param name="Emote">The emote.</param>
public sealed record SendNeighborhoodEmoteMessage(int Emote) : CodecMessage<SendNeighborhoodEmoteMessage>, ICodecProvider<SendNeighborhoodEmoteMessage>
{
    /// <inheritdoc />
    public static ICodec<SendNeighborhoodEmoteMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.Emote, static (emote) => new SendNeighborhoodEmoteMessage(emote));
}
