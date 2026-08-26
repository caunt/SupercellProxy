using System.Globalization;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

internal sealed record CommandStructureArrayFieldSchema : CommandFieldSchema
{
    internal CommandStructureArrayFieldSchema(
        bool nullable,
        ReadOnlyMemory<CommandFieldSchema> elementSchemas
    )
    {
        Nullable = nullable;
        ElementSchemas = elementSchemas.ToArray();
    }

    internal bool Nullable { get; }
    internal ReadOnlyMemory<CommandFieldSchema> ElementSchemas { get; }

    internal override CommandField Decode(MessageStream stream)
    {
        var count = stream.ReadVarInt();

        if (Nullable && count is -1)
            return new CommandStructureArrayField(values: null);

        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Invalid command structure array count: {count}."
                )
            );

        var values = new CommandStructure[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = new CommandStructure(DecodeFields(ElementSchemas.Span, stream));

        return new CommandStructureArrayField(values);
    }

    internal override bool IsValid(CommandField field)
    {
        if (field is not CommandStructureArrayField structureArray)
            return false;

        if (structureArray.Values is null)
            return Nullable;

        foreach (var value in structureArray.Values.Value.Span)
        {
            if (!AreValid(ElementSchemas.Span, value.Fields.Span))
                return false;
        }

        return true;
    }
}
