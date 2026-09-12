using System.Globalization;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandVarIntArrayField</c>.
/// </summary>
public sealed record CommandVariableIntArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandVariableIntArrayField"/> instance.
    /// </summary>
    public CommandVariableIntArrayField(ReadOnlyMemory<int> values)
    {
        Values = values.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.VariableIntArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<int> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandVariableIntArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(DecodeValues(stream.ReadVariableInt(), stream));
    }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static int[] DecodeValues(int count, MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid command array count: {count}."));

        int[] values = new int[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = stream.ReadVariableInt();

        return values;
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVariableInt(Values.Length);

        foreach (int value in Values.Span)
            stream.WriteVariableInt(value);
    }
}
