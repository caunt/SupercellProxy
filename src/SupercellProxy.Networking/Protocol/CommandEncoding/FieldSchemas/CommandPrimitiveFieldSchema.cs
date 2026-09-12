using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;

/// <summary>
/// Defines the Command Primitive Field Schema contract.
/// </summary>
/// <summary>
/// Defines the Field Type contract.
/// </summary>
public sealed record CommandPrimitiveFieldSchema(CommandFieldType FieldType) : CommandFieldSchema
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public override CommandField Decode(MessageStream stream)
    {
        return CommandField.Decode(FieldType, stream);
    }

    /// <summary>
    /// Provides the Is Valid value or operation.
    /// </summary>
    public override bool IsValid(CommandField field)
    {
        return field.FieldType == FieldType;
    }
}
