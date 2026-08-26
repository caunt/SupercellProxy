using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

internal sealed record CommandPrimitiveFieldSchema(CommandFieldType FieldType) : CommandFieldSchema
{
    internal override CommandField Decode(MessageStream stream) =>
        CommandField.Decode(FieldType, stream);

    internal override bool IsValid(CommandField field) => field.FieldType == FieldType;
}
