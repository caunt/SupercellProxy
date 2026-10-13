using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Adds one entry to the neighborhood chat stream.</summary>
/// <param name="Entry">The entry.</param>
public sealed record NeighborhoodChatMessage(NeighborhoodStreamEntry Entry) : CodecMessage<NeighborhoodChatMessage>, ICodecProvider<NeighborhoodChatMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodChatMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(NeighborhoodStreamEntry.Codec, static message => message.Entry, static (entry) => new NeighborhoodChatMessage(entry)),
        trailingDataMessage: "The neighborhood chat message has trailing data."
    );
}
