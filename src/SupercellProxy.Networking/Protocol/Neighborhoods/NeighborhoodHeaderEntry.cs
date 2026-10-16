using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Profile of one neighbourhood as neighbourhood lists show it: name, tag, emblem, description, settings and statistics.
/// Meanings marked as likely come from comparing captured profiles.
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

    /// <summary>The neighbourhood's tag, such as #GJQ0JP9J.</summary>
    public string? AdditionalText { get; init; }

    /// <summary>
    /// Three settings: the first is 0 or 1 and likely the join type, the second looks like the language index, and the third
    /// like the required level (10 to 70 in the captures).
    /// </summary>
    public int[] HeaderValues { get; init; } = new int[HeaderCount];

    /// <summary>Global ids of the three parts of the neighbourhood's emblem.</summary>
    public int[] BadgeValues { get; init; } = new int[BadgeCount];

    /// <summary>Description the neighbourhood wrote about itself.</summary>
    public string? Description { get; init; }

    /// <summary>First setting flag, set for almost every captured neighbourhood; its meaning is not identified.</summary>
    public bool Flag0 { get; init; }

    /// <summary>Second setting flag, set for about a third of the captured neighbourhoods; its meaning is not identified.</summary>
    public bool Flag1 { get; init; }

    /// <summary>24 small counters, the second-to-last always -1 in the captures; their meanings are not identified.</summary>
    public int[] Statistics { get; init; } = new int[StatisticCount];

    /// <summary>Weekly history records of the neighbourhood; null when the profile carries none.</summary>
    public NeighborhoodHistory? History { get; init; }

    /// <summary>Id of the neighbourhood.</summary>
    [JsonPropertyName("Id")]
    public LongId Id { get; init; }

    /// <summary>Small number from -1 to 4 in the captures; its meaning is not identified.</summary>
    public int Metadata { get; init; }

    /// <summary>Name of the neighbourhood.</summary>
    public string? Name { get; init; }

    /// <summary>Optional id that is absent in every capture; its meaning is not identified.</summary>
    [JsonPropertyName("OptionalId0")]
    public LongId? OptionalId0 { get; init; }

    /// <summary>Account id sent with most profiles, likely the leader's; not confirmed.</summary>
    [JsonPropertyName("OptionalId1")]
    public LongId? OptionalId1 { get; init; }

    /// <summary>Small id from 0 to a few thousand in the captures; its meaning is not identified.</summary>
    [JsonPropertyName("RelatedId")]
    public LongId RelatedId { get; init; }

    /// <summary>
    /// Four numbers: the third looks like points and the fourth is 0 or a Unix time in seconds; the first two are not identified.
    /// </summary>
    public int[] AdditionalValues { get; init; } = new int[AdditionalValueCount];

    /// <summary>Number that is 3 or 4 in the captures; its meaning is not identified.</summary>
    public int TrailingValue0 { get; init; }

    /// <summary>Number that is -1 in every capture; its meaning is not identified.</summary>
    public int TrailingValue1 { get; init; }

    /// <summary>Number that is 12 in every capture; its meaning is not identified.</summary>
    public int Value { get; init; }

    /// <summary>Returns only the type name, without the profile's fields.</summary>
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
