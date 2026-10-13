using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Race;

/// <summary>Preserves one neighborhood's complete native derby race entry.</summary>
public sealed record DerbyRaceEntry : ICodecProvider<DerbyRaceEntry>
{
    /// <summary>Gets the number of fixed native values following the neighborhood name.</summary>
    public const int RetainedValueCount = 28;

    private const int RankFieldIndex = 0;

    private const int ScoreFieldIndex = 1;
    /// <inheritdoc />
    public static ICodec<DerbyRaceEntry> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static entry => entry.Badge0,
        Codecs.VarInt,
        static entry => entry.Badge1,
        Codecs.VarInt,
        static entry => entry.Badge2,
        Codecs.LongId,
        static entry => entry.InstanceId,
        Codecs.LongId,
        static entry => entry.RelatedId,
        Codecs.LongId,
        static entry => entry.NeighborhoodId,
        Codecs.VarInt,
        static entry => entry.Value,
        Codecs.OptionalString,
        static entry => entry.NeighborhoodName,
        Codecs.FixedArray(Codecs.VarInt, RetainedValueCount),
        static entry => entry.Values,
        Codecs.Optional(DerbyResultEntries.Codec),
        static entry => entry.Results,
        static (badge0, badge1, badge2, instanceId, relatedId, neighborhoodId, value, neighborhoodName, values, results) => new DerbyRaceEntry
        {
            Badge0 = badge0,
            Badge1 = badge1,
            Badge2 = badge2,
            InstanceId = instanceId,
            RelatedId = relatedId,
            NeighborhoodId = neighborhoodId,
            Value = value,
            NeighborhoodName = neighborhoodName,
            Values = values,
            Results = results,
        }
    );

    /// <summary>Gets the first neighborhood badge component.</summary>
    public int Badge0 { get; init; }
    /// <summary>Gets the second neighborhood badge component.</summary>
    public int Badge1 { get; init; }
    /// <summary>Gets the third neighborhood badge component.</summary>
    public int Badge2 { get; init; }
    /// <summary>Gets the derby instance used to identify this race entry.</summary>
    public LongId InstanceId { get; init; }
    /// <summary>Gets the neighborhood identifier.</summary>
    public LongId NeighborhoodId { get; init; }
    /// <summary>Gets the neighborhood's display name.</summary>
    public string? NeighborhoodName { get; init; }
    /// <summary>Gets the score used by the native race ordering.</summary>
    public int RaceScore => Values[ScoreFieldIndex];
    /// <summary>Gets the neighborhood's race rank.</summary>
    public int Rank => Values[RankFieldIndex];
    /// <summary>Gets the additional native race identifier.</summary>
    public LongId RelatedId { get; init; }
    /// <summary>Gets the optional end-of-derby result collection.</summary>
    public DerbyResultEntries? Results { get; init; }
    /// <summary>Gets the native value between the neighborhood identifier and name.</summary>
    public int Value { get; init; }
    /// <summary>Gets the remaining fixed values in their native wire order.</summary>
    public int[] Values { get; init; } = new int[RetainedValueCount];

    /// <summary>Omits neighborhood details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyRaceEntry);
    }

    /// <summary>Copies the entry with the rank assigned by the native race ordering.</summary>
    public DerbyRaceEntry WithRank(int rank)
    {
        int[] values = [.. Values];
        values[RankFieldIndex] = rank;

        return this with { Values = values };
    }
}
