using System.Globalization;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandVarLongArrayField</c>.
/// </summary>
public sealed record CommandVariableLongArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandVariableLongArrayField"/> instance.
    /// </summary>
    public CommandVariableLongArrayField(ReadOnlyMemory<long> values)
    {
        Values = values.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.VariableLongArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<long> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandVariableLongArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(DecodeValues(stream.ReadVariableInt(), stream));
    }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static long[] DecodeValues(int count, MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid command array count: {count}."));

        long[] values = new long[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = stream.ReadVariableLong();

        return values;
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVariableInt(Values.Length);

        foreach (long value in Values.Span)
            stream.WriteVariableLong(value);
    }
}
