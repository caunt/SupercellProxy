using SupercellProxy.Networking.Protocol.Neighborhoods.Directory;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Streams;

/// <summary>Inbox entry type 4: an invitation to join a neighborhood.</summary>
/// <param name="Header">The fields every entry shares.</param>
/// <param name="NeighborhoodId">The inviting neighborhood.</param>
/// <param name="NeighborhoodName">Display name of the inviting neighborhood.</param>
/// <param name="Badge">The neighborhood badge.</param>
/// <param name="InviterId">The optional id native reads after the badge.</param>
public sealed record NeighborhoodInvitationAvatarStreamEntry(AvatarStreamEntryHeader Header, LongId NeighborhoodId, string? NeighborhoodName, NeighborhoodBadge Badge, LongId? InviterId) : AvatarStreamEntry(Header), ICodecProvider<NeighborhoodInvitationAvatarStreamEntry>
{
    internal const int Type = 4;

    /// <inheritdoc />
    public static new ICodec<NeighborhoodInvitationAvatarStreamEntry> Codec { get; } = Codecs.Composite(
        AvatarStreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.LongId,
        static entry => entry.NeighborhoodId,
        Codecs.OptionalString,
        static entry => entry.NeighborhoodName,
        NeighborhoodBadge.Codec,
        static entry => entry.Badge,
        Codecs.OptionalValue(Codecs.LongId),
        static entry => entry.InviterId,
        static (header, neighborhoodId, neighborhoodName, badge, inviterId) => new NeighborhoodInvitationAvatarStreamEntry(header, neighborhoodId, neighborhoodName, badge, inviterId)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
