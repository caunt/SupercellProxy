using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Search;

/// <summary>Serverbound search for neighbourhoods by name and size.</summary>
/// <param name="Query">Text to look for in neighbourhood names or tags.</param>
/// <param name="MinimumMembers">Fewest members a listed neighbourhood may have.</param>
/// <param name="MaximumMembers">Most members a listed neighbourhood may have.</param>
/// <param name="RequiredLevel">Farm level the listed neighbourhoods may ask for at most.</param>
/// <param name="NeighborhoodId">Neighbourhood sent with the search, such as the player's own; its role in the search is not identified.</param>
/// <param name="JoinableOnly">Whether to list only neighbourhoods the player can join.</param>
public sealed record SearchNeighborhoodMessage(string? Query, int MinimumMembers, int MaximumMembers, int RequiredLevel, LongId? NeighborhoodId, bool JoinableOnly) : CodecMessage<SearchNeighborhoodMessage>, ICodecProvider<SearchNeighborhoodMessage>
{
    /// <inheritdoc />
    public static ICodec<SearchNeighborhoodMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.Query,
        Codecs.Int32,
        static message => message.MinimumMembers,
        Codecs.Int32,
        static message => message.MaximumMembers,
        Codecs.Int32,
        static message => message.RequiredLevel,
        Codecs.OptionalLongId,
        static message => message.NeighborhoodId,
        Codecs.Boolean,
        static message => message.JoinableOnly,
        static (query, minimumMembers, maximumMembers, requiredLevel, neighborhoodId, joinableOnly) => new SearchNeighborhoodMessage(query, minimumMembers, maximumMembers, requiredLevel, neighborhoodId, joinableOnly)
    );
}
