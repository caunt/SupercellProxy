using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Reports a neighborhood member.</summary>
/// <param name="ReporterId">The reporter id.</param>
/// <param name="NeighborhoodId">The neighborhood id.</param>
/// <param name="AvatarId">The avatar id.</param>
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
