using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandNullableVarLongArrayField</c>.
/// </summary>
public sealed record CommandNullableVariableLongArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandNullableVariableLongArrayField"/> instance.
    /// </summary>
    public CommandNullableVariableLongArrayField(ReadOnlyMemory<long>? values)
    {
        Values = values is null ? null : (ReadOnlyMemory<long>?)values.Value.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.NullableVariableLongArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<long>? Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandNullableVariableLongArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVariableInt();

        return count is -1
            ? new CommandNullableVariableLongArrayField(values: null)
            : new CommandNullableVariableLongArrayField(CommandVariableLongArrayField.DecodeValues(count, stream));
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        if (Values is null)
        {
            stream.WriteVariableInt(valueToWrite: -1);

            return;
        }

        stream.WriteVariableInt(Values.Value.Length);

        foreach (long value in Values.Value.Span)
            stream.WriteVariableLong(value);
    }
}
