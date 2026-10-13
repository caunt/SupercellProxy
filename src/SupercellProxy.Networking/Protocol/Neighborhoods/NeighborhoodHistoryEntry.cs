using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Defines the Neighborhood History Entry contract.
/// </summary>
public sealed record NeighborhoodHistoryEntry : ICodecProvider<NeighborhoodHistoryEntry>
{
    private const int ValueCount = 8;

    /// <inheritdoc />
    public static ICodec<NeighborhoodHistoryEntry> Codec { get; } = Codecs.Composite(
        Codecs.FixedArray(Codecs.VarInt, ValueCount),
        static entry => entry.Values,
        Codecs.Boolean,
        static entry => entry.Flag,
        static (values, flag) => new NeighborhoodHistoryEntry
        {
            Values = values,
            Flag = flag,
        }
    );

    /// <summary>
    /// Gets the Flag value.
    /// </summary>
    public bool Flag { get; init; }

    /// <summary>
    /// Gets the Values value.
    /// </summary>
    public int[] Values { get; init; } = new int[ValueCount];
}
