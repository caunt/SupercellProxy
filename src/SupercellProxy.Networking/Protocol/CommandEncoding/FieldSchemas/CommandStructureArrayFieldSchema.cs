using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding.StructureFields;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CommandEncoding.FieldSchemas;

/// <summary>
/// Defines the Command Structure Array Field Schema contract.
/// </summary>
public sealed record CommandStructureArrayFieldSchema : CommandFieldSchema
{
    /// <summary>
    /// Provides the Command Structure Array Field Schema value or operation.
    /// </summary>
    public CommandStructureArrayFieldSchema(bool nullable, ReadOnlyMemory<CommandFieldSchema> elementSchemas)
    {
        Nullable = nullable;
        ElementSchemas = elementSchemas.ToArray();
    }

    /// <summary>
    /// Gets the Element Schemas value.
    /// </summary>
    public ReadOnlyMemory<CommandFieldSchema> ElementSchemas { get; }

    /// <summary>
    /// Gets the Nullable value.
    /// </summary>
    public bool Nullable { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public override CommandField Decode(MessageStream stream)
    {
        int count = stream.ReadVariableInt();

        if (Nullable && count is -1)
            return new CommandStructureArrayField(values: null);

        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid command structure array count: {count}."));

        CommandStructure[] values = new CommandStructure[count];

        for (int index = 0; index < values.Length; index++)
            values[index] = new CommandStructure(DecodeFields(ElementSchemas.Span, stream));

        return new CommandStructureArrayField(values);
    }

    /// <summary>
    /// Provides the Is Valid value or operation.
    /// </summary>
    public override bool IsValid(CommandField field)
    {
        if (field is not CommandStructureArrayField structureArray)
            return false;

        if (structureArray.Values is null)
            return Nullable;

        foreach (CommandStructure value in structureArray.Values.Value.Span)
        {
            if (!AreValid(ElementSchemas.Span, value.Fields.Span))
                return false;
        }

        return true;
    }
}
