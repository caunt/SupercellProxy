using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// One typed field in a command whose native semantic field names are unavailable.
/// </summary>
public abstract record LogicCommandField
{
    internal abstract LogicCommandFieldType FieldType { get; }
    internal abstract void Encode(SupercellStream stream);

    internal static LogicCommandField Decode(LogicCommandFieldType fieldType, SupercellStream stream)
    {
        return fieldType switch
        {
            LogicCommandFieldType.VarInt => new LogicCommandVarIntField(stream.ReadVarInt()),
            LogicCommandFieldType.VarLong => new LogicCommandVarLongField(stream.ReadVarLong()),
            LogicCommandFieldType.Int32 => new LogicCommandInt32Field(stream.ReadInt32()),
            LogicCommandFieldType.Byte => new LogicCommandByteField(unchecked((sbyte)stream.ReadByte())),
            LogicCommandFieldType.UInt16 => new LogicCommandUInt16Field(stream.ReadUInt16()),
            LogicCommandFieldType.Boolean => new LogicCommandBooleanField(stream.ReadBoolean()),
            LogicCommandFieldType.String => new LogicCommandStringField(stream.ReadString()),
            LogicCommandFieldType.LogicLong => new LogicCommandLogicLongField(stream.ReadLogicLong()),
            LogicCommandFieldType.OptionalLogicLong => new LogicCommandOptionalLogicLongField(stream.ReadBoolean() ? stream.ReadLogicLong() : null),
            LogicCommandFieldType.DataReference => new LogicCommandDataReferenceField(stream.ReadVarInt()),
            LogicCommandFieldType.ByteArray => new LogicCommandByteArrayField(stream.ReadByteArray()),
            LogicCommandFieldType.VarIntArray => LogicCommandVarIntArrayField.Decode(stream),
            LogicCommandFieldType.VarLongArray => LogicCommandVarLongArrayField.Decode(stream),
            LogicCommandFieldType.NullableVarLongArray => LogicCommandNullableVarLongArrayField.Decode(stream),
            LogicCommandFieldType.VarIntPairArray => LogicCommandVarIntPairArrayField.Decode(stream),
            LogicCommandFieldType.DataReferenceVarIntPairArray => LogicCommandDataReferenceVarIntPairArrayField.Decode(stream),
            LogicCommandFieldType.DataReferenceArray => LogicCommandDataReferenceArrayField.Decode(stream),
            LogicCommandFieldType.StringArray => LogicCommandStringArrayField.Decode(stream),
            LogicCommandFieldType.ByteCountedVarIntArray => LogicCommandByteCountedVarIntArrayField.Decode(stream),
            LogicCommandFieldType.OptionalInt32String => LogicCommandOptionalInt32StringField.Decode(stream),
            _ => throw new InvalidDataException($"Unsupported logic command field type: {fieldType}.")
        };
    }
}

public sealed record LogicCommandVarIntField(int Value) : LogicCommandField
{
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.VarInt;
    internal override void Encode(SupercellStream stream) => stream.WriteVarInt(Value);
}

public sealed record LogicCommandVarLongField(long Value) : LogicCommandField
{
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.VarLong;
    internal override void Encode(SupercellStream stream) => stream.WriteVarLong(Value);
}

public sealed record LogicCommandInt32Field(int Value) : LogicCommandField
{
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.Int32;
    internal override void Encode(SupercellStream stream) => stream.WriteInt32(Value);
}

public sealed record LogicCommandByteField(sbyte Value) : LogicCommandField
{
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.Byte;
    internal override void Encode(SupercellStream stream) => stream.WriteByte(unchecked((byte)Value));
}

public sealed record LogicCommandUInt16Field(ushort Value) : LogicCommandField
{
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.UInt16;
    internal override void Encode(SupercellStream stream) => stream.WriteUInt16(Value);
}

public sealed record LogicCommandBooleanField(bool Value) : LogicCommandField
{
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.Boolean;
    internal override void Encode(SupercellStream stream) => stream.WriteBoolean(Value);
}

public sealed record LogicCommandStringField(string Value) : LogicCommandField
{
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.String;
    internal override void Encode(SupercellStream stream) => stream.WriteString(Value);
}

public sealed record LogicCommandLogicLongField(LogicLong Value) : LogicCommandField
{
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.LogicLong;
    internal override void Encode(SupercellStream stream) => stream.WriteLogicLong(Value);
}

public sealed record LogicCommandOptionalLogicLongField(LogicLong? Value) : LogicCommandField
{
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.OptionalLogicLong;

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(Value is not null);

        if (Value is not null)
            stream.WriteLogicLong(Value.Value);
    }
}

public sealed record LogicCommandDataReferenceField(int GlobalId) : LogicCommandField
{
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.DataReference;
    internal override void Encode(SupercellStream stream) => stream.WriteVarInt(GlobalId);
}

internal enum LogicCommandFieldType
{
    VarInt,
    VarLong,
    Int32,
    Byte,
    UInt16,
    Boolean,
    String,
    LogicLong,
    OptionalLogicLong,
    DataReference,
    ByteArray,
    VarIntArray,
    VarLongArray,
    NullableVarLongArray,
    VarIntPairArray,
    DataReferenceVarIntPairArray,
    DataReferenceArray,
    StringArray,
    ByteCountedVarIntArray,
    OptionalInt32String,
    OptionalStructure,
    StructureArray
}
