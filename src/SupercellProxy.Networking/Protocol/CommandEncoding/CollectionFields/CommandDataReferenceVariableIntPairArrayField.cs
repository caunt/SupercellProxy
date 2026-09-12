using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandDataReferenceVarIntPairArrayField</c>.
/// </summary>
public sealed record CommandDataReferenceVariableIntPairArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandDataReferenceVariableIntPairArrayField"/> instance.
    /// </summary>
    public CommandDataReferenceVariableIntPairArrayField(ReadOnlyMemory<CommandDataReferenceVariableIntPair> values)
    {
        Values = values.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.DataReferenceVariableIntPairArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVariableIntPair> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandDataReferenceVariableIntPairArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = CommandVariableIntPairArrayField.ReadCount(stream);
        CommandDataReferenceVariableIntPair[] values = new CommandDataReferenceVariableIntPair[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = new CommandDataReferenceVariableIntPair(stream.ReadVariableInt(), stream.ReadVariableInt());

        return new CommandDataReferenceVariableIntPairArrayField(values);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVariableInt(Values.Length);

        foreach (CommandDataReferenceVariableIntPair value in Values.Span)
        {
            stream.WriteVariableInt(value.GlobalIdentifier);
            stream.WriteVariableInt(value.Value);
        }
    }
}
