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
            CommandFieldType.VarInt => new CommandVarIntField(stream.ReadVarInt()),
            CommandFieldType.VarLong => new CommandVarLongField(stream.ReadVarLong()),
            CommandFieldType.Int32 => new CommandInt32Field(stream.ReadInt32()),
            CommandFieldType.Byte => new CommandByteField(unchecked(sbyte.CreateTruncating(stream.ReadByte()))),
            CommandFieldType.UInt16 => new CommandUInt16Field(stream.ReadUInt16()),
            CommandFieldType.Boolean => new CommandBooleanField(stream.ReadBoolean()),
            CommandFieldType.String => new CommandStringField(stream.ReadString()),
            CommandFieldType.LongId => new CommandLongIdField(stream.ReadLongId()),
            CommandFieldType.OptionalLongId => new CommandOptionalLongIdField(stream.ReadBoolean() ? stream.ReadLongId() : null),
            CommandFieldType.DataReference => new CommandDataReferenceField(stream.ReadVarInt()),
            CommandFieldType.ByteArray => new CommandByteArrayField(stream.ReadByteArray()),
            CommandFieldType.VarIntArray => CommandVarIntArrayField.Decode(stream),
            CommandFieldType.VarLongArray => CommandVarLongArrayField.Decode(stream),
            CommandFieldType.NullableVarLongArray => CommandNullableVarLongArrayField.Decode(stream),
            CommandFieldType.VarIntPairArray => CommandVarIntPairArrayField.Decode(stream),
            CommandFieldType.DataReferenceVarIntPairArray =>
                CommandDataReferenceVarIntPairArrayField.Decode(stream),
            CommandFieldType.DataReferenceArray => CommandDataReferenceArrayField.Decode(stream),
            CommandFieldType.StringArray => CommandStringArrayField.Decode(stream),
            CommandFieldType.ByteCountedVarIntArray => CommandByteCountedVarIntArrayField.Decode(stream),
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
