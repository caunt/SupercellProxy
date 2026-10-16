using SupercellProxy.Networking.Protocol.Neighborhoods.Directory;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>Stream entry type 8: a neighborhood notification.</summary>
/// <param name="Header">The fields every stream entry shares.</param>
/// <param name="Message">The notification message.</param>
/// <param name="NeighborhoodName">Name of the neighbourhood the notification is about.</param>
/// <param name="Badge">The neighborhood badge.</param>
public sealed record NeighborhoodNotificationStreamEntry(StreamEntryHeader Header, string? Message, string? NeighborhoodName, NeighborhoodBadge Badge) : NeighborhoodStreamEntry(Header), ICodecProvider<NeighborhoodNotificationStreamEntry>
{
    internal const int Type = 8;

    /// <inheritdoc />
    public static new ICodec<NeighborhoodNotificationStreamEntry> Codec { get; } = Codecs.Composite(
        StreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.OptionalString,
        static entry => entry.Message,
        Codecs.OptionalString,
        static entry => entry.NeighborhoodName,
        NeighborhoodBadge.Codec,
        static entry => entry.Badge,
        static (header, message, neighborhoodName, badge) => new NeighborhoodNotificationStreamEntry(header, message, neighborhoodName, badge)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
