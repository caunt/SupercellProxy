using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Defines the Neighborhood History contract.
/// </summary>
public sealed record NeighborhoodHistory : ICodecProvider<NeighborhoodHistory>
{
    private const int MaximumEntryCount = 10_000;

    /// <inheritdoc />
    public static ICodec<NeighborhoodHistory> Codec { get; } = Codecs.Composite(
        Codecs.NullableArray(NeighborhoodHistoryEntry.Codec, CollectionCount.AtMost(MaximumEntryCount).WithStrictAbsence()),
        static history => history.Entries,
        static (entries) => new NeighborhoodHistory
        {
            Entries = entries,
        }
    );

    /// <summary>
    /// Gets the Entries value.
    /// </summary>
    public NeighborhoodHistoryEntry[]? Entries { get; init; }
}
