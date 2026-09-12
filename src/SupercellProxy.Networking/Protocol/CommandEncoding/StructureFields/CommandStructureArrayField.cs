using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.StructureFields;

/// <summary>
/// Represents <c language="csharp">CommandStructureArrayField</c>.
/// </summary>
public sealed record CommandStructureArrayField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandStructureArrayField"/> instance.
    /// </summary>
    public CommandStructureArrayField(ReadOnlyMemory<CommandStructure>? values)
    {
        Values = values is null ? null : (ReadOnlyMemory<CommandStructure>?)values.Value.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.StructureArray;

    /// <summary>
    /// Gets the <c language="csharp">Values</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandStructure>? Values { get; }

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

        foreach (CommandStructure value in Values.Value.Span)
        {
            foreach (CommandField field in value.Fields.Span)
                field.Encode(stream);
        }
    }
}
