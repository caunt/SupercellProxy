using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Reports a neighbourhood member to moderation.</summary>
/// <param name="ReporterId">Account of the player making the report.</param>
/// <param name="NeighborhoodId">Neighbourhood the reported member belongs to.</param>
/// <param name="AvatarId">Account of the reported member.</param>
public sealed record ReportNeighborhoodMemberMessage(LongId ReporterId, LongId NeighborhoodId, LongId AvatarId) : CodecMessage<ReportNeighborhoodMemberMessage>, ICodecProvider<ReportNeighborhoodMemberMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportNeighborhoodMemberMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.ReporterId,
        Codecs.LongId,
        static message => message.NeighborhoodId,
        Codecs.LongId,
        static message => message.AvatarId,
        static (reporterId, neighborhoodId, avatarId) => new ReportNeighborhoodMemberMessage(reporterId, neighborhoodId, avatarId)
    );
}
