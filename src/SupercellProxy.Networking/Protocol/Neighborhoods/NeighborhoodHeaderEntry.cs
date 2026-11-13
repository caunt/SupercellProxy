using System.Collections.Immutable;
using System.Runtime.InteropServices;
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
    public static ICodec<NeighborhoodHeaderEntry> Codec { get; } = Codecs.Composite(Codecs.FixedArray(Codecs.VarInt, HeaderCount),
        static entry => entry.HeaderValues,
        Codecs.Boolean,
        static entry => entry.Flag0,
        Codecs.Boolean,
        static entry => entry.Flag1,
        Codecs.OptionalUtf8String,
        static entry => entry.Name,
        Codecs.LongId,
        static entry => entry.Id,
        Codecs.FixedArray(Codecs.VarInt, BadgeCount),
        static entry => entry.BadgeValues,
        Codecs.OptionalUtf8String,
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
        static (headerValues,
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
            trailingBlock) => new NeighborhoodHeaderEntry
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
            });

    /// <summary>Gets or sets the neighbourhood's tag, such as #GJQ0JP9J.</summary>
    /// <value>An optional string read near the end of the profile, after <see cref="AdditionalValues"/>; <see langword="null"/> when the server writes it as absent.</value>
    public string? AdditionalText { get; init; }

    /// <summary>
    /// Gets or sets three settings: the first is 0 or 1 and likely the join type, the second looks like the language index, and the third
    /// like the required level (10 to 70 in the captures).
    /// </summary>
    /// <value>Exactly three VarInts, the first fields of the profile; defaults to three zeros and must keep that length to encode.</value>
    public ImmutableArray<int> HeaderValues { get; init; } = ImmutableCollectionsMarshal.AsImmutableArray(new int[HeaderCount]);

    /// <summary>Gets or sets the global ids of the three parts of the neighbourhood's emblem.</summary>
    /// <value>Exactly three VarInts, read right after <see cref="Id"/>; defaults to three zeros and must keep that length to encode.</value>
    public ImmutableArray<int> BadgeValues { get; init; } = ImmutableCollectionsMarshal.AsImmutableArray(new int[BadgeCount]);

    /// <summary>Gets or sets the description the neighbourhood wrote about itself.</summary>
    /// <value>Free text chosen by the neighbourhood, or <see langword="null"/> when the server writes it as absent.</value>
    public string? Description { get; init; }

    /// <summary>Gets or sets the first setting flag, set for almost every captured neighbourhood; its meaning is not identified.</summary>
    /// <value><see langword="true"/> if the server set the boolean read right after <see cref="HeaderValues"/>; otherwise, <see langword="false"/>.</value>
    public bool Flag0 { get; init; }

    /// <summary>Gets or sets the second setting flag, set for about a third of the captured neighbourhoods; its meaning is not identified.</summary>
    /// <value><see langword="true"/> if the server set the boolean read after <see cref="Flag0"/> and before <see cref="Name"/>; otherwise, <see langword="false"/>.</value>
    public bool Flag1 { get; init; }

    /// <summary>Gets or sets 24 small counters, the second-to-last always -1 in the captures; their meanings are not identified.</summary>
    /// <value>Exactly 24 VarInts read after <see cref="OptionalId0"/>; defaults to zeros and must keep that length to encode.</value>
    public ImmutableArray<int> Statistics { get; init; } = ImmutableCollectionsMarshal.AsImmutableArray(new int[StatisticCount]);

    /// <summary>Gets or sets the weekly history records of the neighbourhood; <see langword="null"/> when the profile carries none.</summary>
    /// <value>An optional block read after <see cref="Metadata"/>, holding one record per week.</value>
    public NeighborhoodHistory? History { get; init; }

    /// <summary>Gets or sets the id of the neighbourhood.</summary>
    /// <value>The neighbourhood's server id, the same id that membership commands and member-list requests name.</value>
    [JsonPropertyName("Id")]
    public LongId Id { get; init; }

    /// <summary>Gets or sets a small number from -1 to 4 in the captures; its meaning is not identified.</summary>
    /// <value>The VarInt read after <see cref="OptionalId1"/> and before <see cref="History"/>.</value>
    public int Metadata { get; init; }

    /// <summary>Gets or sets the name of the neighbourhood.</summary>
    /// <value>The display name players chose, or <see langword="null"/> when the server writes it as absent.</value>
    public string? Name { get; init; }

    /// <summary>Gets or sets an optional id that is absent in every capture; its meaning is not identified.</summary>
    /// <value>An optional long id read after <see cref="RelatedId"/>; <see langword="null"/> when absent.</value>
    [JsonPropertyName("OptionalId0")]
    public LongId? OptionalId0 { get; init; }

    /// <summary>Gets or sets the account id sent with most profiles, likely the leader's; not confirmed.</summary>
    /// <value>An optional long id read after <see cref="Statistics"/>; <see langword="null"/> when the profile carries none.</value>
    [JsonPropertyName("OptionalId1")]
    public LongId? OptionalId1 { get; init; }

    /// <summary>Gets or sets a small id from 0 to a few thousand in the captures; its meaning is not identified.</summary>
    /// <value>A required long id read after <see cref="Value"/>.</value>
    [JsonPropertyName("RelatedId")]
    public LongId RelatedId { get; init; }

    /// <summary>
    /// Gets or sets four numbers: the third looks like points and the fourth is 0 or a Unix time in seconds; the first two are not identified.
    /// </summary>
    /// <value>Exactly four VarInts read after <see cref="History"/>; defaults to zeros and must keep that length to encode.</value>
    public ImmutableArray<int> AdditionalValues { get; init; } = ImmutableCollectionsMarshal.AsImmutableArray(new int[AdditionalValueCount]);

    /// <summary>Gets or sets a number that is 3 or 4 in the captures; its meaning is not identified.</summary>
    /// <value>The VarInt read after <see cref="AdditionalText"/>.</value>
    public int TrailingValue0 { get; init; }

    /// <summary>Gets or sets a number that is -1 in every capture; its meaning is not identified.</summary>
    /// <value>The last VarInt of the profile.</value>
    public int TrailingValue1 { get; init; }

    /// <summary>Gets or sets a number that is 12 in every capture; its meaning is not identified.</summary>
    /// <value>The VarInt read after <see cref="Description"/>.</value>
    public int Value { get; init; }

    /// <summary>Returns only the type name, without the profile's fields.</summary>
    /// <returns>Always <c>NeighborhoodHeaderEntry</c>, so logs never show names, tags or ids.</returns>
    public override string ToString()
    {
        return nameof(NeighborhoodHeaderEntry);
    }

    // The values native reads after the history, as one group to keep the field list within a composite.
    private readonly record struct TrailingBlock(ImmutableArray<int> AdditionalValues, string? AdditionalText, int TrailingValue0, int TrailingValue1)
    {
        public static ICodec<TrailingBlock> Codec { get; } = Codecs.Composite(Codecs.FixedArray(Codecs.VarInt, AdditionalValueCount),
            static segment => segment.AdditionalValues,
            Codecs.OptionalUtf8String,
            static segment => segment.AdditionalText,
            Codecs.VarInt,
            static segment => segment.TrailingValue0,
            Codecs.VarInt,
            static segment => segment.TrailingValue1,
            static (additionalValues, additionalText, trailingValue0, trailingValue1) => new TrailingBlock(additionalValues, additionalText, trailingValue0, trailingValue1));
    }
}
