using System.Globalization;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandVarLongArrayField</c>.
/// </summary>
public sealed record CommandVarLongArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandVarLongArrayField"/> instance.
    /// </summary>
    public CommandVarLongArrayField(ReadOnlyMemory<long> values)
    {
        Values = values.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.VarLongArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<long> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandVarLongArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(DecodeValues(stream.ReadVarInt(), stream));
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
            values[index] = stream.ReadVarLong();

        return values;
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (long value in Values.Span)
            stream.WriteVarLong(value);
    }
}
