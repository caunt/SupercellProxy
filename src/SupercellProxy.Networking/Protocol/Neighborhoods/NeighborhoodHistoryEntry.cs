using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>One week in a neighbourhood's history: eight numbers and a flag.</summary>
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

    /// <summary>Flag of the week, set in the captures exactly for weeks whose fifth number is 0; its meaning is not identified.</summary>
    public bool Flag { get; init; }

    /// <summary>
    /// The week's numbers: the first is the week's start as Unix time in seconds; the others look like weekly event results such as
    /// points, and their exact meanings are not identified.
    /// </summary>
    public int[] Values { get; init; } = new int[ValueCount];
}
