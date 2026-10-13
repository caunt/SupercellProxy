using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams;

/// <summary>Adds one neighborhood notification stream entry in the game log manager, with a value it passes on when nonzero.</summary>
/// <param name="Value">The value native reads first.</param>
/// <param name="Entry">The entry, when present.</param>
public sealed record NeighborhoodNotificationStreamEntryMessage(int Value, NeighborhoodStreamEntry? Entry) : CodecMessage<NeighborhoodNotificationStreamEntryMessage>, ICodecProvider<NeighborhoodNotificationStreamEntryMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodNotificationStreamEntryMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Value,
        Codecs.Optional(NeighborhoodStreamEntry.Codec),
        static message => message.Entry,
        static (@value, entry) => new NeighborhoodNotificationStreamEntryMessage(value, entry)
    );
}
