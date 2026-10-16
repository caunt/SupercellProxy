using SupercellProxy.Networking.Protocol.Neighborhoods.Directory;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Streams;

/// <summary>Inbox entry type 6: the neighborhood changed a setting; 1 shares and 2 unshares the derby task log.</summary>
/// <param name="Header">The fields every entry shares.</param>
/// <param name="MessageText">Text attached to the entry; null when none.</param>
/// <param name="NeighborhoodId">Neighborhood whose setting changed.</param>
/// <param name="NeighborhoodName">Display name of that neighborhood when the entry was written.</param>
/// <param name="Setting">The value the inbox text is chosen by.</param>
/// <param name="Badge">The neighborhood badge.</param>
public sealed record NeighborhoodSettingsChangedStreamEntry(
    AvatarStreamEntryHeader Header,
    string? MessageText,
    LongId NeighborhoodId,
    string? NeighborhoodName,
    int Setting,
    NeighborhoodBadge Badge
) : AvatarStreamEntry(Header), ICodecProvider<NeighborhoodSettingsChangedStreamEntry>
{
    internal const int Type = 6;

    /// <inheritdoc />
    public static new ICodec<NeighborhoodSettingsChangedStreamEntry> Codec { get; } = Codecs.Composite(
        AvatarStreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.OptionalString,
        static entry => entry.MessageText,
        Codecs.LongId,
        static entry => entry.NeighborhoodId,
        Codecs.OptionalString,
        static entry => entry.NeighborhoodName,
        Codecs.VarInt,
        static entry => entry.Setting,
        NeighborhoodBadge.Codec,
        static entry => entry.Badge,
        static (header, messageText, neighborhoodId, neighborhoodName, setting, badge) => new NeighborhoodSettingsChangedStreamEntry(header, messageText, neighborhoodId, neighborhoodName, setting, badge)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
