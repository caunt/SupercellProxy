using System.Collections.Immutable;
using System.Runtime.InteropServices;

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
    public static ICodec<DerbyRaceEntry> Codec { get; } = Codecs.Composite(Codecs.VarInt,
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
        Codecs.OptionalUtf8String,
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
        });

    /// <summary>Gets or sets the first neighborhood badge component.</summary>
    /// <value>The first of the three var-ints that together select the neighborhood's badge, written first in the entry; which badge part it picks is not yet mapped.</value>
    public int Badge0 { get; init; }

    /// <summary>Gets or sets the second neighborhood badge component.</summary>
    /// <value>The second of the three badge var-ints, read after <see cref="Badge0"/>; which badge part it picks is not yet mapped.</value>
    public int Badge1 { get; init; }

    /// <summary>Gets or sets the third neighborhood badge component.</summary>
    /// <value>The last of the three badge var-ints, read just before <see cref="InstanceId"/>; which badge part it picks is not yet mapped.</value>
    public int Badge2 { get; init; }

    /// <summary>Gets or sets the derby instance used to identify this race entry.</summary>
    /// <value>The identifier of the derby race instance this entry belongs to, the same kind of identifier a race-page request selects.</value>
    public LongId InstanceId { get; init; }

    /// <summary>Gets or sets the neighborhood identifier.</summary>
    /// <value>The identifier of the neighborhood racing in this entry.</value>
    public LongId NeighborhoodId { get; init; }

    /// <summary>Gets or sets the neighborhood's display name.</summary>
    /// <value>The name shown on the race page, or <see langword="null"/> when native wrote the absent-string marker.</value>
    public string? NeighborhoodName { get; init; }

    /// <summary>Gets the score used by the native race ordering.</summary>
    /// <value>The second element of <see cref="Values"/>, the neighborhood's score in the race; it changes only when <see cref="Values"/> is replaced.</value>
    public int RaceScore => Values[ScoreFieldIndex];

    /// <summary>Gets the neighborhood's race rank.</summary>
    /// <value>The first element of <see cref="Values"/>, the neighborhood's place in the race; <see cref="With(int)"/> returns a copy with a different rank.</value>
    public int Rank => Values[RankFieldIndex];

    /// <summary>Gets or sets the additional native race identifier.</summary>
    /// <value>The identifier written between <see cref="InstanceId"/> and <see cref="NeighborhoodId"/>; what it refers to is not yet identified.</value>
    public LongId RelatedId { get; init; }

    /// <summary>Gets or sets the optional end-of-derby result collection.</summary>
    /// <value>The result rows that close the entry once the derby has ended, or <see langword="null"/> when the presence flag before them is clear.</value>
    public DerbyResultEntries? Results { get; init; }

    /// <summary>Gets or sets the native value between the neighborhood identifier and name.</summary>
    /// <value>The var-int written after <see cref="NeighborhoodId"/> and before <see cref="NeighborhoodName"/>; its meaning is not yet identified.</value>
    public int Value { get; init; }

    /// <summary>Gets or sets the remaining fixed values in their native wire order.</summary>
    /// <value>Exactly <see cref="RetainedValueCount"/> var-ints, where index 0 is <see cref="Rank"/> and index 1 is <see cref="RaceScore"/> and the rest are not yet identified; writing an array of any other length throws <see cref="InvalidDataException"/>. It defaults to 28 zeros.</value>
    public ImmutableArray<int> Values { get; init; } = ImmutableCollectionsMarshal.AsImmutableArray(new int[RetainedValueCount]);

    /// <summary>Omits neighborhood details from diagnostic text.</summary>
    /// <returns>The type name <c>DerbyRaceEntry</c> alone, so the neighborhood name and identifiers never reach logs.</returns>
    public override string ToString()
    {
        return nameof(DerbyRaceEntry);
    }

    /// <summary>Copies the entry with the rank assigned by the native race ordering.</summary>
    /// <param name="rank">The place to store in the first element of <see cref="Values"/>; it is stored as given, without range checks.</param>
    /// <returns>A new entry equal to this one except that <see cref="Rank"/> is <paramref name="rank"/>; the values array is copied, so this entry is left unchanged.</returns>
    public DerbyRaceEntry With(int rank)
    {
        return this with
        {
            Values = Values.SetItem(RankFieldIndex, rank)
        };
    }
}
