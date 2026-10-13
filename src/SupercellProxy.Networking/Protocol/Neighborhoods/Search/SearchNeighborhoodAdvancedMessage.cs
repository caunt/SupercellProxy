using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Search;

/// <summary>Searches neighborhoods with advanced options.</summary>
/// <param name="Tags">The tags.</param>
/// <param name="Query">The query.</param>
/// <param name="NeighborhoodId">The neighborhood id.</param>
/// <param name="RequiredLevel">The required level.</param>
/// <param name="FamilyFriendly">The family friendly.</param>
/// <param name="DerbyEnabled">The derby enabled.</param>
/// <param name="Language">The language.</param>
/// <param name="LeagueId">The league id.</param>
/// <param name="Type">The type.</param>
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
