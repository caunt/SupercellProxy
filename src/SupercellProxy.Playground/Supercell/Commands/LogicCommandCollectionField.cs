using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

public sealed record LogicCommandByteArrayField : LogicCommandField
{
    public LogicCommandByteArrayField(ReadOnlyMemory<byte> value)
    {
        Value = value.ToArray();
    }

    public ReadOnlyMemory<byte> Value { get; }
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.ByteArray;
    internal override void Encode(SupercellStream stream) => stream.WriteByteArray(Value.Span);
}

public sealed record LogicCommandOptionalStructureField : LogicCommandField
{
    public LogicCommandOptionalStructureField(ReadOnlyMemory<LogicCommandField>? fields)
    {
        Fields = fields?.ToArray();
    }

    public ReadOnlyMemory<LogicCommandField>? Fields { get; }
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.OptionalStructure;

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(Fields is not null);

        if (Fields is null)
            return;

        foreach (var field in Fields.Value.Span)
            field.Encode(stream);
    }
}

public sealed record LogicCommandStructure
{
    public LogicCommandStructure(ReadOnlyMemory<LogicCommandField> fields)
    {
        Fields = fields.ToArray();
    }

    public ReadOnlyMemory<LogicCommandField> Fields { get; }
}

public sealed record LogicCommandStructureArrayField : LogicCommandField
{
    public LogicCommandStructureArrayField(ReadOnlyMemory<LogicCommandStructure>? values)
    {
        Values = values?.ToArray();
    }

    public ReadOnlyMemory<LogicCommandStructure>? Values { get; }
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.StructureArray;

    internal override void Encode(SupercellStream stream)
    {
        if (Values is null)
        {
            stream.WriteVarInt(-1);
            return;
        }

        stream.WriteVarInt(Values.Value.Length);

        foreach (var value in Values.Value.Span)
        {
            foreach (var field in value.Fields.Span)
                field.Encode(stream);
        }
    }
}

public sealed record LogicCommandVarIntArrayField : LogicCommandField
{
    public LogicCommandVarIntArrayField(ReadOnlyMemory<int> values)
    {
        Values = values.ToArray();
    }

    public ReadOnlyMemory<int> Values { get; }
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.VarIntArray;

    internal static LogicCommandVarIntArrayField Decode(SupercellStream stream)
    {
        return new LogicCommandVarIntArrayField(DecodeValues(stream.ReadVarInt(), stream));
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (var value in Values.Span)
            stream.WriteVarInt(value);
    }

    internal static int[] DecodeValues(int count, SupercellStream stream)
    {
        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException($"Invalid command array count: {count}.");

        var values = new int[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadVarInt();

        return values;
    }
}

public sealed record LogicCommandVarLongArrayField : LogicCommandField
{
    public LogicCommandVarLongArrayField(ReadOnlyMemory<long> values)
    {
        Values = values.ToArray();
    }

    public ReadOnlyMemory<long> Values { get; }
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.VarLongArray;

    internal static LogicCommandVarLongArrayField Decode(SupercellStream stream)
    {
        return new LogicCommandVarLongArrayField(DecodeValues(stream.ReadVarInt(), stream));
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (var value in Values.Span)
            stream.WriteVarLong(value);
    }

    internal static long[] DecodeValues(int count, SupercellStream stream)
    {
        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException($"Invalid command array count: {count}.");

        var values = new long[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadVarLong();

        return values;
    }
}

public sealed record LogicCommandNullableVarLongArrayField : LogicCommandField
{
    public LogicCommandNullableVarLongArrayField(ReadOnlyMemory<long>? values)
    {
        Values = values?.ToArray();
    }

    public ReadOnlyMemory<long>? Values { get; }
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.NullableVarLongArray;

    internal static LogicCommandNullableVarLongArrayField Decode(SupercellStream stream)
    {
        var count = stream.ReadVarInt();
        return new LogicCommandNullableVarLongArrayField(count is -1 ? null : LogicCommandVarLongArrayField.DecodeValues(count, stream));
    }

    internal override void Encode(SupercellStream stream)
    {
        if (Values is null)
        {
            stream.WriteVarInt(-1);
            return;
        }

        stream.WriteVarInt(Values.Value.Length);

        foreach (var value in Values.Value.Span)
            stream.WriteVarLong(value);
    }
}

public readonly record struct LogicCommandVarIntPair(int Value0, int Value1);

public sealed record LogicCommandVarIntPairArrayField : LogicCommandField
{
    public LogicCommandVarIntPairArrayField(ReadOnlyMemory<LogicCommandVarIntPair> values)
    {
        Values = values.ToArray();
    }

    public ReadOnlyMemory<LogicCommandVarIntPair> Values { get; }
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.VarIntPairArray;

    internal static LogicCommandVarIntPairArrayField Decode(SupercellStream stream)
    {
        var count = ReadCount(stream);
        var values = new LogicCommandVarIntPair[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = new LogicCommandVarIntPair(stream.ReadVarInt(), stream.ReadVarInt());

        return new LogicCommandVarIntPairArrayField(values);
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (var value in Values.Span)
        {
            stream.WriteVarInt(value.Value0);
            stream.WriteVarInt(value.Value1);
        }
    }

    internal static int ReadCount(SupercellStream stream)
    {
        var count = stream.ReadVarInt();

        if (count < 0 || count > (stream.Length - stream.Position) / 2)
            throw new InvalidDataException($"Invalid command pair array count: {count}.");

        return count;
    }
}

public readonly record struct LogicCommandDataReferenceVarIntPair(int GlobalId, int Value);

public sealed record LogicCommandDataReferenceVarIntPairArrayField : LogicCommandField
{
    public LogicCommandDataReferenceVarIntPairArrayField(ReadOnlyMemory<LogicCommandDataReferenceVarIntPair> values)
    {
        Values = values.ToArray();
    }

    public ReadOnlyMemory<LogicCommandDataReferenceVarIntPair> Values { get; }
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.DataReferenceVarIntPairArray;

    internal static LogicCommandDataReferenceVarIntPairArrayField Decode(SupercellStream stream)
    {
        var count = LogicCommandVarIntPairArrayField.ReadCount(stream);
        var values = new LogicCommandDataReferenceVarIntPair[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = new LogicCommandDataReferenceVarIntPair(stream.ReadVarInt(), stream.ReadVarInt());

        return new LogicCommandDataReferenceVarIntPairArrayField(values);
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (var value in Values.Span)
        {
            stream.WriteVarInt(value.GlobalId);
            stream.WriteVarInt(value.Value);
        }
    }
}

public sealed record LogicCommandDataReferenceArrayField : LogicCommandField
{
    public LogicCommandDataReferenceArrayField(ReadOnlyMemory<int> globalIds)
    {
        GlobalIds = globalIds.ToArray();
    }

    public ReadOnlyMemory<int> GlobalIds { get; }
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.DataReferenceArray;

    internal static LogicCommandDataReferenceArrayField Decode(SupercellStream stream)
    {
        return new LogicCommandDataReferenceArrayField(LogicCommandVarIntArrayField.DecodeValues(stream.ReadVarInt(), stream));
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(GlobalIds.Length);

        foreach (var globalId in GlobalIds.Span)
            stream.WriteVarInt(globalId);
    }
}

public sealed record LogicCommandStringArrayField : LogicCommandField
{
    public LogicCommandStringArrayField(ReadOnlyMemory<string> values)
    {
        Values = values.ToArray();
    }

    public ReadOnlyMemory<string> Values { get; }
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.StringArray;

    internal static LogicCommandStringArrayField Decode(SupercellStream stream)
    {
        var count = stream.ReadVarInt();

        if (count < 0 || count > stream.Length - stream.Position)
            throw new InvalidDataException($"Invalid command string array count: {count}.");

        var values = new string[count];

        for (var i = 0; i < values.Length; i++)
            values[i] = stream.ReadString();

        return new LogicCommandStringArrayField(values);
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Values.Length);

        foreach (var value in Values.Span)
            stream.WriteString(value);
    }
}

public sealed record LogicCommandByteCountedVarIntArrayField : LogicCommandField
{
    public LogicCommandByteCountedVarIntArrayField(ReadOnlyMemory<int> values)
    {
        if (values.Length > sbyte.MaxValue)
            throw new InvalidDataException($"A byte-counted command array cannot contain more than {sbyte.MaxValue} values.");

        Values = values.ToArray();
    }

    public ReadOnlyMemory<int> Values { get; }
    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.ByteCountedVarIntArray;

    internal static LogicCommandByteCountedVarIntArrayField Decode(SupercellStream stream)
    {
        var count = unchecked((sbyte)stream.ReadByte());
        return new LogicCommandByteCountedVarIntArrayField(LogicCommandVarIntArrayField.DecodeValues(count, stream));
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteByte((byte)Values.Length);

        foreach (var value in Values.Span)
            stream.WriteVarInt(value);
    }
}

public sealed record LogicCommandOptionalInt32StringField(int Value, string Text) : LogicCommandField
{
    public bool HasValue { get; init; } = true;

    internal override LogicCommandFieldType FieldType => LogicCommandFieldType.OptionalInt32String;

    public static LogicCommandOptionalInt32StringField Empty => new(0, string.Empty) { HasValue = false };

    internal static LogicCommandOptionalInt32StringField Decode(SupercellStream stream)
    {
        return stream.ReadBoolean()
            ? new LogicCommandOptionalInt32StringField(stream.ReadInt32(), stream.ReadString())
            : Empty;
    }

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(HasValue);

        if (!HasValue)
            return;

        stream.WriteInt32(Value);
        stream.WriteString(Text);
    }
}
