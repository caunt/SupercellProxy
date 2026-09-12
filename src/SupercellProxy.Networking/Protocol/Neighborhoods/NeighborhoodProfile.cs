using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>
/// Defines the Neighborhood Profile contract.
/// </summary>
public sealed record NeighborhoodProfile
{
    private const int AdditionalValueCount = 4;
    private const int BadgeCount = 3;
    private const int HeaderCount = 3;
    private const int StatisticCount = 24;

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
    public LongIdentifier Identifier { get; init; }

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
    public LongIdentifier? OptionalIdentifier0 { get; init; }

    /// <summary>
    /// Gets the Optional Id1 value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("OptionalId1")]
    public LongIdentifier? OptionalIdentifier1 { get; init; }

    /// <summary>
    /// Gets the Related Id value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("RelatedId")]
    public LongIdentifier RelatedIdentifier { get; init; }

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
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static NeighborhoodProfile Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new()
        {
            HeaderValues = stream.ReadVariableIntArray(HeaderCount),
            Flag0 = stream.ReadBoolean(),
            Flag1 = stream.ReadBoolean(),
            Name = stream.ReadOptionalString(),
            Identifier = stream.ReadLongIdentifier(),
            BadgeValues = stream.ReadVariableIntArray(BadgeCount),
            Description = stream.ReadOptionalString(),
            Value = stream.ReadVariableInt(),
            RelatedIdentifier = stream.ReadLongIdentifier(),
            OptionalIdentifier0 = stream.ReadOptionalLongIdentifier(),
            Statistics = stream.ReadVariableIntArray(StatisticCount),
            OptionalIdentifier1 = stream.ReadOptionalLongIdentifier(),
            Metadata = stream.ReadVariableInt(),
            History = stream.ReadBoolean() ? NeighborhoodHistory.Decode(stream) : null,
            AdditionalValues = stream.ReadVariableIntArray(AdditionalValueCount),
            AdditionalText = stream.ReadOptionalString(),
            TrailingValue0 = stream.ReadVariableInt(),
            TrailingValue1 = stream.ReadVariableInt(),
        };
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        WriteValues(stream, HeaderValues, HeaderCount);
        stream.WriteBoolean(Flag0);
        stream.WriteBoolean(Flag1);
        stream.WriteOptionalString(Name);
        stream.WriteLongIdentifier(Identifier);
        WriteValues(stream, BadgeValues, BadgeCount);
        stream.WriteOptionalString(Description);
        stream.WriteVariableInt(Value);
        stream.WriteLongIdentifier(RelatedIdentifier);
        stream.WriteOptionalLongIdentifier(OptionalIdentifier0);
        WriteValues(stream, Statistics, StatisticCount);
        stream.WriteOptionalLongIdentifier(OptionalIdentifier1);
        stream.WriteVariableInt(Metadata);
        stream.WriteBoolean(History is not null);
        History?.Encode(stream);
        WriteValues(stream, AdditionalValues, AdditionalValueCount);
        stream.WriteOptionalString(AdditionalText);
        stream.WriteVariableInt(TrailingValue0);
        stream.WriteVariableInt(TrailingValue1);
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(NeighborhoodProfile);
    }

    private static void WriteValues(MessageStream stream, int[] values, int count)
    {
        if (values.Length != count)
            throw new InvalidDataException(message: "Invalid neighborhood profile scalar count.");

        foreach (int value in values)
            stream.WriteVariableInt(value);
    }
}
