using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

internal sealed record CommandOptionalStructureFieldSchema : CommandFieldSchema
{
    internal CommandOptionalStructureFieldSchema(ReadOnlyMemory<CommandFieldSchema> fieldSchemas)
    {
        FieldSchemas = fieldSchemas.ToArray();
    }

    internal ReadOnlyMemory<CommandFieldSchema> FieldSchemas { get; }

    internal override CommandField Decode(MessageStream stream)
    {
        return new CommandOptionalStructureField(
            stream.ReadBoolean() ? DecodeFields(FieldSchemas.Span, stream) : null
        );
    }

    internal override bool IsValid(CommandField field)
    {
        return field is CommandOptionalStructureField optionalStructure
            && (
                optionalStructure.Fields is null
                || AreValid(FieldSchemas.Span, optionalStructure.Fields.Value.Span)
            );
    }
}
