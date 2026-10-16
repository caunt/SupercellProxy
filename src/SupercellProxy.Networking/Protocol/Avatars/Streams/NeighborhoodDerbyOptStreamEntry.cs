using SupercellProxy.Networking.Protocol.Neighborhoods.Directory;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Streams;

/// <summary>Inbox entry type 7: the player was opted out of the derby; 1 by the leader and 2 automatically.</summary>
/// <param name="Header">The fields every entry shares.</param>
/// <param name="MessageText">Text attached to the entry; null when none.</param>
/// <param name="NeighborhoodId">Neighborhood whose derby the player was opted out of.</param>
/// <param name="NeighborhoodName">Display name of that neighborhood when the entry was written.</param>
/// <param name="Reason">The value the inbox text is chosen by.</param>
/// <param name="Badge">The neighborhood badge.</param>
public sealed record NeighborhoodDerbyOptStreamEntry(
    AvatarStreamEntryHeader Header,
    string? MessageText,
    LongId NeighborhoodId,
    string? NeighborhoodName,
    int Reason,
    NeighborhoodBadge Badge
) : AvatarStreamEntry(Header), ICodecProvider<NeighborhoodDerbyOptStreamEntry>
{
    internal const int Type = 7;

    /// <inheritdoc />
    public static new ICodec<NeighborhoodDerbyOptStreamEntry> Codec { get; } = Codecs.Composite(
        AvatarStreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.OptionalString,
        static entry => entry.MessageText,
        Codecs.LongId,
        static entry => entry.NeighborhoodId,
        Codecs.OptionalString,
        static entry => entry.NeighborhoodName,
        Codecs.VarInt,
        static entry => entry.Reason,
        NeighborhoodBadge.Codec,
        static entry => entry.Badge,
        static (header, messageText, neighborhoodId, neighborhoodName, reason, badge) => new NeighborhoodDerbyOptStreamEntry(header, messageText, neighborhoodId, neighborhoodName, reason, badge)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
