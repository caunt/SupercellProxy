using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandNullableVarLongArrayField</c>.
/// </summary>
public sealed record CommandNullableVarLongArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandNullableVarLongArrayField"/> instance.
    /// </summary>
    public CommandNullableVarLongArrayField(ReadOnlyMemory<long>? values)
    {
        Values = values is null ? null : (ReadOnlyMemory<long>?)values.Value.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.NullableVarLongArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<long>? Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandNullableVarLongArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVarInt();

        return count is -1
            ? new CommandNullableVarLongArrayField(values: null)
            : new CommandNullableVarLongArrayField(CommandVarLongArrayField.DecodeValues(count, stream));
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        if (Values is null)
        {
            stream.WriteVarInt(valueToWrite: -1);

            return;
        }

        stream.WriteVarInt(Values.Value.Length);

        foreach (long value in Values.Value.Span)
            stream.WriteVarLong(value);
    }
}
