using SupercellProxy.Networking.Protocol.CommandEncoding.StructureFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;

/// <summary>
/// Defines the Command Optional Structure Field Schema contract.
/// </summary>
public sealed record CommandOptionalStructureFieldSchema : CommandFieldSchema
{
    /// <summary>
    /// Provides the Command Optional Structure Field Schema value or operation.
    /// </summary>
    public CommandOptionalStructureFieldSchema(ReadOnlyMemory<CommandFieldSchema> fieldSchemas)
    {
        FieldSchemas = fieldSchemas.ToArray();
    }

    /// <summary>
    /// Gets the Field Schemas value.
    /// </summary>
    public ReadOnlyMemory<CommandFieldSchema> FieldSchemas { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public override CommandField Decode(MessageStream stream)
    {
        return !stream.ReadBoolean()
            ? new CommandOptionalStructureField(fields: null)
            : new CommandOptionalStructureField(DecodeFields(FieldSchemas.Span, stream));
    }

    /// <summary>
    /// Provides the Is Valid value or operation.
    /// </summary>
    public override bool IsValid(CommandField field)
    {
        return field is CommandOptionalStructureField optionalStructure
            && (
                optionalStructure.Fields is null
                || AreValid(FieldSchemas.Span, optionalStructure.Fields.Value.Span)
            );
    }
}
