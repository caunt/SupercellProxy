using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandByteCountedVarIntArrayField</c>.
/// </summary>
public sealed record CommandByteCountedVarIntArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandByteCountedVarIntArrayField"/> instance.
    /// </summary>
    public CommandByteCountedVarIntArrayField(ReadOnlyMemory<int> values)
    {
        if (values.Length > sbyte.MaxValue)
            throw new InvalidDataException($"A byte-counted command array cannot contain more than {sbyte.MaxValue} values.");

        Values = values.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.ByteCountedVarIntArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<int> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandByteCountedVarIntArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        sbyte count = unchecked(sbyte.CreateTruncating(stream.ReadByte()));

        return new CommandByteCountedVarIntArrayField(CommandVarIntArrayField.DecodeValues(count, stream));
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteByte(byte.CreateTruncating(Values.Length));

        foreach (int value in Values.Span)
            stream.WriteVarInt(value);
    }
}
