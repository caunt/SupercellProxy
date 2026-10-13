using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Search;

/// <summary>Searches neighborhoods.</summary>
/// <param name="Query">The query.</param>
/// <param name="MinimumMembers">The minimum members.</param>
/// <param name="MaximumMembers">The maximum members.</param>
/// <param name="RequiredLevel">The required level.</param>
/// <param name="NeighborhoodId">The neighborhood id.</param>
/// <param name="JoinableOnly">The joinable only.</param>
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
