using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>One typed field in a command whose native semantic field names are unavailable.</para>
/// </summary>
internal abstract record CommandField
{
    internal abstract CommandFieldType FieldType { get; }

    internal abstract void Encode(MessageStream stream);

    internal static CommandField Decode(CommandFieldType fieldType, MessageStream stream)
    {
        return fieldType switch
        {
            CommandFieldType.VarInt => new CommandVarIntField(stream.ReadVarInt()),
            CommandFieldType.VarLong => new CommandVarLongField(stream.ReadVarLong()),
            CommandFieldType.Int32 => new CommandInt32Field(stream.ReadInt32()),
            CommandFieldType.Byte => new CommandByteField(
                unchecked(sbyte.CreateTruncating(stream.ReadByte()))
            ),
            CommandFieldType.UInt16 => new CommandUInt16Field(stream.ReadUInt16()),
            CommandFieldType.Boolean => new CommandBooleanField(stream.ReadBoolean()),
            CommandFieldType.String => new CommandStringField(stream.ReadString()),
            CommandFieldType.LongId => new CommandLongIdField(stream.ReadLongId()),
            CommandFieldType.OptionalLongId => new CommandOptionalLongIdField(
                stream.ReadBoolean() ? stream.ReadLongId() : null
            ),
            CommandFieldType.DataReference => new CommandDataReferenceField(stream.ReadVarInt()),
            CommandFieldType.ByteArray => new CommandByteArrayField(stream.ReadByteArray()),
            CommandFieldType.VarIntArray => CommandVarIntArrayField.Decode(stream),
            CommandFieldType.VarLongArray => CommandVarLongArrayField.Decode(stream),
            CommandFieldType.NullableVarLongArray => CommandNullableVarLongArrayField.Decode(
                stream
            ),
            CommandFieldType.VarIntPairArray => CommandVarIntPairArrayField.Decode(stream),
            CommandFieldType.DataReferenceVarIntPairArray =>
                CommandDataReferenceVarIntPairArrayField.Decode(stream),
            CommandFieldType.DataReferenceArray => CommandDataReferenceArrayField.Decode(stream),
            CommandFieldType.StringArray => CommandStringArrayField.Decode(stream),
            CommandFieldType.ByteCountedVarIntArray => CommandByteCountedVarIntArrayField.Decode(
                stream
            ),
            CommandFieldType.OptionalInt32String => CommandOptionalInt32StringField.Decode(stream),
            CommandFieldType.OptionalStructure or CommandFieldType.StructureArray =>
                throw new InvalidDataException(
                    $"Logic command field type {fieldType} requires its registered field schema."
                ),
            _ => throw new InvalidDataException(
                $"Unsupported logic command field type: {fieldType}."
            ),
        };
    }
}
