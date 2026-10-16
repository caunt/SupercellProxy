using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Search;

/// <summary>Searches neighborhoods with advanced options.</summary>
/// <param name="Tags">Play-style tags the listed neighbourhoods must have; null when not filtered.</param>
/// <param name="Query">Text to look for in neighbourhood names or tags.</param>
/// <param name="NeighborhoodId">Neighbourhood sent with the search; its role in the search is not identified.</param>
/// <param name="RequiredLevel">Farm level the listed neighbourhoods may ask for at most.</param>
/// <param name="FamilyFriendly">Whether to list only neighbourhoods marked as suitable for all ages.</param>
/// <param name="DerbyEnabled">Whether to list only neighbourhoods that take part in the derby.</param>
/// <param name="Language">Language index the listed neighbourhoods must use.</param>
/// <param name="LeagueId">Derby league the listed neighbourhoods must play in; null when not filtered.</param>
/// <param name="Type">Join type the listed neighbourhoods must have; its values are not identified.</param>
public sealed record SearchNeighborhoodAdvancedMessage(
    ReadOnlyMemory<NeighborhoodTag>? Tags,
    string? Query,
    LongId? NeighborhoodId,
    int RequiredLevel,
    bool FamilyFriendly,
    bool DerbyEnabled,
    int Language,
    LongId? LeagueId,
    int Type
) : CodecMessage<SearchNeighborhoodAdvancedMessage>, ICodecProvider<SearchNeighborhoodAdvancedMessage>
{
    /// <inheritdoc />
    public static ICodec<SearchNeighborhoodAdvancedMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(NeighborhoodTag.Codec),
        static message => message.Tags,
        Codecs.OptionalString,
        static message => message.Query,
        Codecs.OptionalLongId,
        static message => message.NeighborhoodId,
        Codecs.VarInt,
        static message => message.RequiredLevel,
        Codecs.Boolean,
        static message => message.FamilyFriendly,
        Codecs.Boolean,
        static message => message.DerbyEnabled,
        Codecs.Int32,
        static message => message.Language,
        Codecs.OptionalLongId,
        static message => message.LeagueId,
        Codecs.VarInt,
        static message => message.Type,
        static (tags, query, neighborhoodId, requiredLevel, familyFriendly, derbyEnabled, language, leagueId, type) => new SearchNeighborhoodAdvancedMessage(tags, query, neighborhoodId, requiredLevel, familyFriendly, derbyEnabled, language, leagueId, type)
    );
}
