using SupercellProxy.Networking.Protocol.Neighborhoods.Directory;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Streams;

/// <summary>Inbox entry type 5: the player was kicked out of a neighborhood.</summary>
/// <param name="Header">The fields every entry shares.</param>
/// <param name="MessageText">Text attached to the entry; null when none.</param>
/// <param name="NeighborhoodId">Neighborhood the player was removed from.</param>
/// <param name="NeighborhoodName">Display name of that neighborhood when the entry was written.</param>
/// <param name="Badge">The neighborhood badge.</param>
public sealed record NeighborhoodKickOutStreamEntry(AvatarStreamEntryHeader Header, string? MessageText, LongId NeighborhoodId, string? NeighborhoodName, NeighborhoodBadge Badge) : AvatarStreamEntry(Header), ICodecProvider<NeighborhoodKickOutStreamEntry>
{
    internal const int Type = 5;

    /// <inheritdoc />
    public static new ICodec<NeighborhoodKickOutStreamEntry> Codec { get; } = Codecs.Composite(
        AvatarStreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.OptionalString,
        static entry => entry.MessageText,
        Codecs.LongId,
        static entry => entry.NeighborhoodId,
        Codecs.OptionalString,
        static entry => entry.NeighborhoodName,
        NeighborhoodBadge.Codec,
        static entry => entry.Badge,
        static (header, messageText, neighborhoodId, neighborhoodName, badge) => new NeighborhoodKickOutStreamEntry(header, messageText, neighborhoodId, neighborhoodName, badge)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
