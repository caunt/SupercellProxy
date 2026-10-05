using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;

/// <summary>
/// Represents <c language="csharp">CommandDataReferenceVarIntPairArrayField</c>.
/// </summary>
public sealed record CommandDataReferenceVarIntPairArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandDataReferenceVarIntPairArrayField"/> instance.
    /// </summary>
    public CommandDataReferenceVarIntPairArrayField(ReadOnlyMemory<CommandDataReferenceVarIntPair> values)
    {
        Values = values.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.DataReferenceVarIntPairArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandDataReferenceVarIntPair> Values { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandDataReferenceVarIntPairArrayField Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = CommandVarIntPairArrayField.ReadCount(stream);
        CommandDataReferenceVarIntPair[] values = new CommandDataReferenceVarIntPair[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = new CommandDataReferenceVarIntPair(stream.ReadVarInt(), stream.ReadVarInt());

        return new CommandDataReferenceVarIntPairArrayField(values);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (CommandDataReferenceVarIntPair value in Values.Span)
        {
            stream.WriteVarInt(value.GlobalId);
            stream.WriteVarInt(value.Value);
        }
    }
}
