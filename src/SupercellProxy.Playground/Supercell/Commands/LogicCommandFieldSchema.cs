using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

internal abstract record LogicCommandFieldSchema
{
    internal abstract LogicCommandField Decode(SupercellStream stream);
    internal abstract bool IsValid(LogicCommandField field);

    internal static LogicCommandFieldSchema Primitive(LogicCommandFieldType fieldType)
    {
        return new LogicCommandPrimitiveFieldSchema(fieldType);
    }

    internal static LogicCommandFieldSchema Optional(params LogicCommandFieldSchema[] fieldSchemas)
    {
        return new LogicCommandOptionalStructureFieldSchema(fieldSchemas);
    }

    internal static LogicCommandFieldSchema Array(bool nullable, params LogicCommandFieldSchema[] elementSchemas)
    {
        return new LogicCommandStructureArrayFieldSchema(nullable, elementSchemas);
    }

    internal static LogicCommandField[] DecodeFields(ReadOnlySpan<LogicCommandFieldSchema> fieldSchemas, SupercellStream stream)
    {
        var fields = new LogicCommandField[fieldSchemas.Length];

        for (var i = 0; i < fields.Length; i++)
            fields[i] = fieldSchemas[i].Decode(stream);

        return fields;
    }

    internal static bool AreValid(ReadOnlySpan<LogicCommandFieldSchema> fieldSchemas, ReadOnlySpan<LogicCommandField> fields)
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

internal sealed record LogicCommandPrimitiveFieldSchema(LogicCommandFieldType FieldType) : LogicCommandFieldSchema
{
    internal override LogicCommandField Decode(SupercellStream stream)
    {
        return LogicCommandField.Decode(FieldType, stream);
    }

    internal override bool IsValid(LogicCommandField field)
    {
        return field.FieldType == FieldType;
    }
}

internal sealed record LogicCommandOptionalStructureFieldSchema : LogicCommandFieldSchema
{
    internal LogicCommandOptionalStructureFieldSchema(ReadOnlyMemory<LogicCommandFieldSchema> fieldSchemas)
    {
        FieldSchemas = fieldSchemas.ToArray();
    }

    internal ReadOnlyMemory<LogicCommandFieldSchema> FieldSchemas { get; }

    internal override LogicCommandField Decode(SupercellStream stream)
    {
        return new LogicCommandOptionalStructureField(
            stream.ReadBoolean() ? LogicCommandFieldSchema.DecodeFields(FieldSchemas.Span, stream) : null);
    }

    internal override bool IsValid(LogicCommandField field)
    {
        if (field is not LogicCommandOptionalStructureField optionalStructure)
            return false;

        return optionalStructure.Fields is null || LogicCommandFieldSchema.AreValid(FieldSchemas.Span, optionalStructure.Fields.Value.Span);
    }
}

internal sealed record LogicCommandStructureArrayFieldSchema : LogicCommandFieldSchema
{
    internal LogicCommandStructureArrayFieldSchema(bool nullable, ReadOnlyMemory<LogicCommandFieldSchema> elementSchemas)
    {
        Nullable = nullable;
        ElementSchemas = elementSchemas.ToArray();
    }

    internal bool Nullable { get; }
    internal ReadOnlyMemory<LogicCommandFieldSchema> ElementSchemas { get; }

    internal override LogicCommandField Decode(SupercellStream stream)
    {
        var count = stream.ReadVarInt();

        if (Nullable && count is -1)
            return new LogicCommandStructureArrayField(null);

        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException($"Invalid command structure array count: {count}.");

        var values = new LogicCommandStructure[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = new LogicCommandStructure(LogicCommandFieldSchema.DecodeFields(ElementSchemas.Span, stream));

        return new LogicCommandStructureArrayField(values);
    }

    internal override bool IsValid(LogicCommandField field)
    {
        if (field is not LogicCommandStructureArrayField structureArray)
            return false;

        if (structureArray.Values is null)
            return Nullable;

        foreach (var value in structureArray.Values.Value.Span)
        {
            if (!LogicCommandFieldSchema.AreValid(ElementSchemas.Span, value.Fields.Span))
                return false;
        }

        return true;
    }
}
