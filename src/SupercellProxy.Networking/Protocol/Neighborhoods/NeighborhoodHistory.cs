using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Weekly history of a neighbourhood, one record per week, as sent with its profile.</summary>
public sealed record NeighborhoodHistory : ICodecProvider<NeighborhoodHistory>
{
    private const int MaximumEntryCount = 10_000;

    /// <inheritdoc />
    public static ICodec<NeighborhoodHistory> Codec { get; } = Codecs.Composite(Codecs.NullableArray(NeighborhoodHistoryEntry.Codec, CollectionCount.AtMost(MaximumEntryCount).WithStrictAbsence()),
        static history => history.Entries,
        static entries => new NeighborhoodHistory
        {
            Entries = entries,
        });

    /// <summary>Gets or sets the weekly records, oldest first; <see langword="null"/> when the history is absent.</summary>
    /// <value>At most 10,000 weeks, each eight numbers and a flag; a longer list fails to decode.</value>
    public ImmutableArray<NeighborhoodHistoryEntry>? Entries { get; init; }
}
