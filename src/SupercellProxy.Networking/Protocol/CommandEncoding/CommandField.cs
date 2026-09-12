using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Protocol.CommandEncoding.ScalarFields;
using SupercellProxy.Networking.Protocol.CommandEncoding.StructureFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CommandEncoding;

/// <summary>
/// <para>One typed field in a command whose native semantic field names are unavailable.</para>
/// </summary>
public abstract record CommandField
{
    /// <summary>
    /// Gets the Field Type value.
    /// </summary>
    public abstract CommandFieldType FieldType { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CommandField Decode(CommandFieldType fieldType, MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return fieldType switch
        {
            CommandFieldType.VariableInt => new CommandVariableIntField(stream.ReadVariableInt()),
            CommandFieldType.VariableLong => new CommandVariableLongField(stream.ReadVariableLong()),
            CommandFieldType.Int32 => new CommandInt32Field(stream.ReadInt32()),
            CommandFieldType.Byte => new CommandByteField(unchecked(sbyte.CreateTruncating(stream.ReadByte()))),
            CommandFieldType.UInt16 => new CommandUInt16Field(stream.ReadUInt16()),
            CommandFieldType.Boolean => new CommandBooleanField(stream.ReadBoolean()),
            CommandFieldType.String => new CommandStringField(stream.ReadString()),
            CommandFieldType.LongIdentifier => new CommandLongIdentifierField(stream.ReadLongIdentifier()),
            CommandFieldType.OptionalLongIdentifier => new CommandOptionalLongIdentifierField(stream.ReadBoolean() ? stream.ReadLongIdentifier() : null),
            CommandFieldType.DataReference => new CommandDataReferenceField(stream.ReadVariableInt()),
            CommandFieldType.ByteArray => new CommandByteArrayField(stream.ReadByteArray()),
            CommandFieldType.VariableIntArray => CommandVariableIntArrayField.Decode(stream),
            CommandFieldType.VariableLongArray => CommandVariableLongArrayField.Decode(stream),
            CommandFieldType.NullableVariableLongArray => CommandNullableVariableLongArrayField.Decode(stream),
            CommandFieldType.VariableIntPairArray => CommandVariableIntPairArrayField.Decode(stream),
            CommandFieldType.DataReferenceVariableIntPairArray =>
                CommandDataReferenceVariableIntPairArrayField.Decode(stream),
            CommandFieldType.DataReferenceArray => CommandDataReferenceArrayField.Decode(stream),
            CommandFieldType.StringArray => CommandStringArrayField.Decode(stream),
            CommandFieldType.ByteCountedVariableIntArray => CommandByteCountedVariableIntArrayField.Decode(stream),
            CommandFieldType.OptionalInt32String => CommandOptionalInt32StringField.Decode(stream),
            CommandFieldType.OptionalStructure or CommandFieldType.StructureArray =>
                throw new InvalidDataException($"Logic command field type {fieldType} requires its registered field schema."),
            _ => throw new InvalidDataException($"Unsupported logic command field type: {fieldType}."),
        };
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public abstract void Encode(MessageStream stream);
}
