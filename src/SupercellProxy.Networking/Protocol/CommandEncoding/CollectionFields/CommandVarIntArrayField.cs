using System.Globalization;

using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandVarIntArrayField</c>.
/// </summary>
public sealed record CommandVarIntArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandVarIntArrayField"/> instance.
    /// </summary>
    public CommandVarIntArrayField(ReadOnlyMemory<int> values)
    {
        Values = values.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.VarIntArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<int> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandVarIntArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(DecodeValues(stream.ReadVarInt(), stream));
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
            values[index] = stream.ReadVarInt();

        return values;
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (int value in Values.Span)
            stream.WriteVarInt(value);
    }
}
