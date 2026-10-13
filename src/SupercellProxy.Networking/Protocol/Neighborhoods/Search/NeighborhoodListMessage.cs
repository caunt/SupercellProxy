using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Search;

/// <summary>Lists neighborhoods for the neighborhood popup, such as search results.</summary>
/// <param name="Neighborhoods">The neighborhoods.</param>
/// <param name="SearchText">The text the popup shows with the list.</param>
public sealed record NeighborhoodListMessage(ReadOnlyMemory<SupercellProxy.Networking.Protocol.Neighborhoods.NeighborhoodHeaderEntry> Neighborhoods, string? SearchText) : CodecMessage<NeighborhoodListMessage>, ICodecProvider<NeighborhoodListMessage>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodListMessage> Codec { get; } = Codecs.Composite(
        Codecs.Memory(SupercellProxy.Networking.Protocol.Neighborhoods.NeighborhoodHeaderEntry.Codec),
        static message => message.Neighborhoods,
        Codecs.OptionalString,
        static message => message.SearchText,
        static (neighborhoods, searchText) => new NeighborhoodListMessage(neighborhoods, searchText)
    );
}
