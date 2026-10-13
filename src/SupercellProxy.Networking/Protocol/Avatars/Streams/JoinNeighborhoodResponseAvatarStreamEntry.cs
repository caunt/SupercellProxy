using SupercellProxy.Networking.Protocol.Neighborhoods.Directory;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Streams;

/// <summary>Inbox entry type 3: a neighborhood accepted or denied the player's join request.</summary>
/// <param name="Header">The fields every entry shares.</param>
/// <param name="NeighborhoodId">The neighborhood that answered.</param>
/// <param name="NeighborhoodName">The neighborhood name.</param>
/// <param name="MessageText">The answer's message text.</param>
/// <param name="Badge">The neighborhood badge.</param>
public sealed record JoinNeighborhoodResponseAvatarStreamEntry(AvatarStreamEntryHeader Header, LongId NeighborhoodId, string? NeighborhoodName, string? MessageText, NeighborhoodBadge Badge) : AvatarStreamEntry(Header), ICodecProvider<JoinNeighborhoodResponseAvatarStreamEntry>
{
    internal const int Type = 3;

    /// <inheritdoc />
    public static new ICodec<JoinNeighborhoodResponseAvatarStreamEntry> Codec { get; } = Codecs.Composite(
        AvatarStreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.LongId,
        static entry => entry.NeighborhoodId,
        Codecs.OptionalString,
        static entry => entry.NeighborhoodName,
        Codecs.OptionalString,
        static entry => entry.MessageText,
        NeighborhoodBadge.Codec,
        static entry => entry.Badge,
        static (header, neighborhoodId, neighborhoodName, messageText, badge) => new JoinNeighborhoodResponseAvatarStreamEntry(header, neighborhoodId, neighborhoodName, messageText, badge)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
