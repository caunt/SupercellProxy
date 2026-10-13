using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Defines the Neighborhood Profile contract.
/// </summary>
public sealed record NeighborhoodHeaderEntry : ICodecProvider<NeighborhoodHeaderEntry>
{
    private const int AdditionalValueCount = 4;

    private const int BadgeCount = 3;
    private const int HeaderCount = 3;
    private const int StatisticCount = 24;
    /// <inheritdoc />
    public static ICodec<NeighborhoodHeaderEntry> Codec { get; } = Codecs.Composite(
        Codecs.FixedArray(Codecs.VarInt, HeaderCount),
        static entry => entry.HeaderValues,
        Codecs.Boolean,
        static entry => entry.Flag0,
        Codecs.Boolean,
        static entry => entry.Flag1,
        Codecs.OptionalString,
        static entry => entry.Name,
        Codecs.LongId,
        static entry => entry.Id,
        Codecs.FixedArray(Codecs.VarInt, BadgeCount),
        static entry => entry.BadgeValues,
        Codecs.OptionalString,
        static entry => entry.Description,
        Codecs.VarInt,
        static entry => entry.Value,
        Codecs.LongId,
        static entry => entry.RelatedId,
        Codecs.OptionalLongId,
        static entry => entry.OptionalId0,
        Codecs.FixedArray(Codecs.VarInt, StatisticCount),
        static entry => entry.Statistics,
        Codecs.OptionalLongId,
        static entry => entry.OptionalId1,
        Codecs.VarInt,
        static entry => entry.Metadata,
        Codecs.Optional(NeighborhoodHistory.Codec),
        static entry => entry.History,
        TrailingBlock.Codec,
        static entry => new TrailingBlock(entry.AdditionalValues, entry.AdditionalText, entry.TrailingValue0, entry.TrailingValue1),
        static (
            headerValues,
            flag0,
            flag1,
            name,
            id,
            badgeValues,
            description,
            value,
            relatedId,
            optionalId0,
            statistics,
            optionalId1,
            metadata,
            history,
            trailingBlock
        ) => new NeighborhoodHeaderEntry
        {
            HeaderValues = headerValues,
            Flag0 = flag0,
            Flag1 = flag1,
            Name = name,
            Id = id,
            BadgeValues = badgeValues,
            Description = description,
            Value = value,
            RelatedId = relatedId,
            OptionalId0 = optionalId0,
            Statistics = statistics,
            OptionalId1 = optionalId1,
            Metadata = metadata,
            History = history,
            AdditionalValues = trailingBlock.AdditionalValues,
            AdditionalText = trailingBlock.AdditionalText,
            TrailingValue0 = trailingBlock.TrailingValue0,
            TrailingValue1 = trailingBlock.TrailingValue1,
        }
    );

    /// <summary>
    /// Gets the Additional Text value.
    /// </summary>
    public string? AdditionalText { get; init; }

    /// <summary>
    /// Gets the Header Values value.
    /// </summary>
    public int[] HeaderValues { get; init; } = new int[HeaderCount];

    /// <summary>
    /// Gets the Badge Values value.
    /// </summary>
    public int[] BadgeValues { get; init; } = new int[BadgeCount];

    /// <summary>
    /// Gets the Description value.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the Flag0 value.
    /// </summary>
    public bool Flag0 { get; init; }

    /// <summary>
    /// Gets the Flag1 value.
    /// </summary>
    public bool Flag1 { get; init; }

    /// <summary>
    /// Gets the Statistics value.
    /// </summary>
    public int[] Statistics { get; init; } = new int[StatisticCount];

    /// <summary>
    /// Gets the History value.
    /// </summary>
    public NeighborhoodHistory? History { get; init; }

    /// <summary>
    /// Gets the Id value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Id")]
    public LongId Id { get; init; }

    /// <summary>
    /// Gets the Metadata value.
    /// </summary>
    public int Metadata { get; init; }

    /// <summary>
    /// Gets the Name value.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets the Optional Id0 value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("OptionalId0")]
    public LongId? OptionalId0 { get; init; }

    /// <summary>
    /// Gets the Optional Id1 value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("OptionalId1")]
    public LongId? OptionalId1 { get; init; }

    /// <summary>
    /// Gets the Related Id value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("RelatedId")]
    public LongId RelatedId { get; init; }

    /// <summary>
    /// Gets the Additional Values value.
    /// </summary>
    public int[] AdditionalValues { get; init; } = new int[AdditionalValueCount];

    /// <summary>
    /// Gets the Trailing Value0 value.
    /// </summary>
    public int TrailingValue0 { get; init; }

    /// <summary>
    /// Gets the Trailing Value1 value.
    /// </summary>
    public int TrailingValue1 { get; init; }

    /// <summary>
    /// Gets the Value value.
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(NeighborhoodHeaderEntry);
    }

    // The values native reads after the history, as one group to keep the field list within a composite.
    private readonly record struct TrailingBlock(int[] AdditionalValues, string? AdditionalText, int TrailingValue0, int TrailingValue1)
    {
        public static ICodec<TrailingBlock> Codec { get; } = Codecs.Composite(
            Codecs.FixedArray(Codecs.VarInt, AdditionalValueCount),
            static segment => segment.AdditionalValues,
            Codecs.OptionalString,
            static segment => segment.AdditionalText,
            Codecs.VarInt,
            static segment => segment.TrailingValue0,
            Codecs.VarInt,
            static segment => segment.TrailingValue1,
            static (additionalValues, additionalText, trailingValue0, trailingValue1) => new TrailingBlock(additionalValues, additionalText, trailingValue0, trailingValue1)
        );
    }
}
