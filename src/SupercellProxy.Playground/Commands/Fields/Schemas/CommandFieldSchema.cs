using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

internal abstract record CommandFieldSchema
{
    internal abstract CommandField Decode(MessageStream stream);

    internal abstract bool IsValid(CommandField field);

    internal static CommandFieldSchema Primitive(CommandFieldType fieldType)
    {
        return new CommandPrimitiveFieldSchema(fieldType);
    }

    internal static CommandFieldSchema Optional(params CommandFieldSchema[] fieldSchemas)
    {
        return new CommandOptionalStructureFieldSchema(fieldSchemas);
    }

    internal static CommandFieldSchema Array(
        bool nullable,
        params CommandFieldSchema[] elementSchemas
    )
    {
        return new CommandStructureArrayFieldSchema(nullable, elementSchemas);
    }

    internal static CommandField[] DecodeFields(
        ReadOnlySpan<CommandFieldSchema> fieldSchemas,
        MessageStream stream
    )
    {
        var fields = new CommandField[fieldSchemas.Length];

        for (var i = 0; i < fields.Length; i++)
            fields[i] = fieldSchemas[i].Decode(stream);

        return fields;
    }

    internal static bool AreValid(
        ReadOnlySpan<CommandFieldSchema> fieldSchemas,
        ReadOnlySpan<CommandField> fields
    )
    {
        if (fieldSchemas.Length != fields.Length)
            return false;

        for (var i = 0; i < fields.Length; i++)
        {
            if (!fieldSchemas[i].IsValid(fields[i]))
                return false;
        }

        return true;
    }
}
