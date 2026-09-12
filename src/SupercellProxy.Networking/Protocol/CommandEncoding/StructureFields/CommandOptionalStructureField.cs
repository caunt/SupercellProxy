using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.StructureFields;

/// <summary>
/// Represents <c language="csharp">CommandOptionalStructureField</c>.
/// </summary>
public sealed record CommandOptionalStructureField : CommandField
{
    /// <summary>
    /// Initializes a new <see cref="CommandOptionalStructureField"/> instance.
    /// </summary>
    public CommandOptionalStructureField(ReadOnlyMemory<CommandField>? fields)
    {
        Fields = fields is null ? null : (ReadOnlyMemory<CommandField>?)fields.Value.ToArray();
    }

    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public override CommandFieldType FieldType => CommandFieldType.OptionalStructure;

    /// <summary>
    /// Gets the <c language="csharp">Fields</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandField>? Fields { get; }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream)
    {
        stream.WriteBoolean(Fields is not null);

        if (Fields is null)
            return;

        foreach (CommandField field in Fields.Value.Span)
            field.Encode(stream);
    }
}
