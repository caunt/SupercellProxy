using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Race;

/// <summary>Preserves one neighborhood's complete native derby race entry.</summary>
public sealed record DerbyRaceEntry
{
    /// <summary>Gets the number of fixed native values following the neighborhood name.</summary>
    public const int RetainedValueCount = 28;

    private const int RankFieldIndex = 0;
    private const int ScoreFieldIndex = 1;

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

    /// <summary>Decodes the badge, identity, score, fixed values and optional results.</summary>
    public static DerbyRaceEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int badge0 = stream.ReadVarInt();
        int badge1 = stream.ReadVarInt();
        int badge2 = stream.ReadVarInt();
        LongId instance = stream.ReadLongId();
        LongId related = stream.ReadLongId();
        LongId neighborhood = stream.ReadLongId();
        int value = stream.ReadVarInt();
        string? name = stream.ReadOptionalString();
        int[] values = new int[RetainedValueCount];

        for (int index = 0; index < values.Length; index++)
            values[index] = stream.ReadVarInt();

        return new()
        {
            Badge0 = badge0,
            Badge1 = badge1,
            Badge2 = badge2,
            InstanceId = instance,
            RelatedId = related,
            NeighborhoodId = neighborhood,
            Value = value,
            NeighborhoodName = name,
            Values = values,
            Results = stream.ReadBoolean() ? DerbyResultEntries.Decode(stream) : null,
        };
    }

    /// <summary>Encodes the complete race entry in native order.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (Values.Length != RetainedValueCount)
            throw new InvalidDataException(message: "The derby race entry has an invalid fixed-value count.");

        stream.WriteVarInt(Badge0);
        stream.WriteVarInt(Badge1);
        stream.WriteVarInt(Badge2);
        stream.WriteLongId(InstanceId);
        stream.WriteLongId(RelatedId);
        stream.WriteLongId(NeighborhoodId);
        stream.WriteVarInt(Value);
        stream.WriteOptionalString(NeighborhoodName);

        foreach (int value in Values)
            stream.WriteVarInt(value);

        stream.WriteBoolean(Results is not null);
        Results?.Encode(stream);
    }

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
