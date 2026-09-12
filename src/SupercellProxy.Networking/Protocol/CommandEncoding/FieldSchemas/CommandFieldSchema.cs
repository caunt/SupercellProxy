using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;

/// <summary>
/// Defines the Command Field Schema contract.
/// </summary>
public abstract record CommandFieldSchema
{

    /// <summary>
    /// Provides the Are Valid value or operation.
    /// </summary>
    public static bool AreValid(ReadOnlySpan<CommandFieldSchema> fieldSchemas, ReadOnlySpan<CommandField> fields)
    {
        if (fieldSchemas.Length != fields.Length)
            return false;

        for (int index = 0; index < fields.Length; index++)
        {
            if (!fieldSchemas[index].IsValid(fields[index]))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Provides the Array value or operation.
    /// </summary>
    public static CommandFieldSchema Array(bool nullable, params CommandFieldSchema[] elementSchemas)
    {
        return new CommandStructureArrayFieldSchema(nullable, elementSchemas);
    }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandField[] DecodeFields(ReadOnlySpan<CommandFieldSchema> fieldSchemas, MessageStream stream)
    {
        CommandField[] fields = new CommandField[fieldSchemas.Length];

        for (int index = 0; index < fields.Length; index++)
            fields[index] = fieldSchemas[index].Decode(stream);

        return fields;
    }

    /// <summary>
    /// Provides the Optional value or operation.
    /// </summary>
    public static CommandFieldSchema Optional(params CommandFieldSchema[] fieldSchemas)
    {
        return new CommandOptionalStructureFieldSchema(fieldSchemas);
    }

    /// <summary>
    /// Provides the Primitive value or operation.
    /// </summary>
    public static CommandFieldSchema Primitive(CommandFieldType fieldType)
    {
        return new CommandPrimitiveFieldSchema(fieldType);
    }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public abstract CommandField Decode(MessageStream stream);

    /// <summary>
    /// Provides the Is Valid value or operation.
    /// </summary>
    public abstract bool IsValid(CommandField field);
}
