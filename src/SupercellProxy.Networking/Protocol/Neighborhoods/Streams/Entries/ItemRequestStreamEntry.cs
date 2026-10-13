using SupercellProxy.Networking.Protocol.Neighborhoods.Members;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Streams.Entries;

/// <summary>Stream entry type 7: a member asked the neighborhood for an item.</summary>
/// <param name="Header">The fields every stream entry shares.</param>
/// <param name="ExpiresAtSeconds">The time the request expires.</param>
/// <param name="ItemGlobalId">The requested item.</param>
/// <param name="Quantity">The requested quantity.</param>
/// <param name="Completed">Whether the request was completed.</param>
/// <param name="Donations">The members who donated.</param>
public sealed record ItemRequestStreamEntry(
    StreamEntryHeader Header,
    int ExpiresAtSeconds,
    int ItemGlobalId,
    int Quantity,
    bool Completed,
    ReadOnlyMemory<NeighborhoodMemberSummary>? Donations
) : NeighborhoodStreamEntry(Header), ICodecProvider<ItemRequestStreamEntry>
{
    internal const int Type = 7;

    /// <inheritdoc />
    public static new ICodec<ItemRequestStreamEntry> Codec { get; } = Codecs.Composite(
        StreamEntryHeader.Codec,
        static entry => entry.Header,
        Codecs.VarInt,
        static entry => entry.ExpiresAtSeconds,
        Codecs.VarInt,
        static entry => entry.ItemGlobalId,
        Codecs.VarInt,
        static entry => entry.Quantity,
        Codecs.Boolean,
        static entry => entry.Completed,
        Codecs.NullableMemory(NeighborhoodMemberSummary.Codec),
        static entry => entry.Donations,
        static (header, expiresAtSeconds, itemGlobalId, quantity, completed, donations) => new ItemRequestStreamEntry(header, expiresAtSeconds, itemGlobalId, quantity, completed, donations)
    );

    /// <inheritdoc/>
    public override int EntryType => Type;
}
