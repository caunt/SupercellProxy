using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// One typed field in a polymorphic native map-game event.
/// </summary>
public abstract record LogicMapGameEventField
{
    internal abstract LogicMapGameEventFieldType FieldType { get; }
    internal abstract void Encode(SupercellStream stream);
}

public sealed record LogicMapGameEventVarIntField(int Value) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.VarInt;
    internal override void Encode(SupercellStream stream) => stream.WriteVarInt(Value);
}

public sealed record LogicMapGameEventBooleanField(bool Value) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.Boolean;
    internal override void Encode(SupercellStream stream) => stream.WriteBoolean(Value);
}

public sealed record LogicMapGameEventByteField(sbyte Value) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.Byte;
    internal override void Encode(SupercellStream stream) => stream.WriteByte(unchecked((byte)Value));
}

public sealed record LogicMapGameEventLogicLongField(LogicLong Value) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.LogicLong;
    internal override void Encode(SupercellStream stream) => stream.WriteLogicLong(Value);
}

public sealed record LogicMapGameEventOptionalLogicLongField(LogicLong? Value) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.OptionalLogicLong;
    internal override void Encode(SupercellStream stream) => LogicMapGameWire.WriteOptionalLogicLong(stream, Value);
}

public sealed record LogicMapGameEventDataReferenceField(int GlobalId, int ExpectedTableId = -1) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.DataReference;
    internal override void Encode(SupercellStream stream) => stream.WriteVarInt(GlobalId);
}

public sealed record LogicMapGameEventOptionalPawnField(LogicMapGamePawn? Value) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.OptionalPawn;

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}

public sealed record LogicMapGameEventOptionalTaskField(LogicMapGameTask? Value) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.OptionalTask;

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}

public sealed record LogicMapGameEventOptionalTaskCollectionField(LogicMapGameTaskCollection? Value) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.OptionalTaskCollection;

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}

public sealed record LogicMapGameEventOptionalVarIntArrayField : LogicMapGameEventField
{
    public LogicMapGameEventOptionalVarIntArrayField(ReadOnlyMemory<int>? values)
    {
        Values = values?.ToArray();
    }

    public ReadOnlyMemory<int>? Values { get; }
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.OptionalVarIntArray;

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(Values is not null);

        if (Values is not null)
            new LogicCommandVarIntArrayField(Values.Value).Encode(stream);
    }
}

public sealed record LogicMapGameEventOptionalStateField(LogicMapGameState? Value) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.OptionalState;

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}

public sealed record LogicMapGameEventOptionalDumpTaskStateField(LogicMapGameDumpTaskStatePayload? Value) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.OptionalDumpTaskState;

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}

public sealed record LogicMapGameEventOptionalProfileDataField(LogicMapGameEventProfileData? Value) : LogicMapGameEventField
{
    internal override LogicMapGameEventFieldType FieldType => LogicMapGameEventFieldType.OptionalProfileData;

    internal override void Encode(SupercellStream stream)
    {
        stream.WriteBoolean(Value is not null);
        Value?.Encode(stream);
    }
}

internal enum LogicMapGameEventFieldType
{
    VarInt,
    Boolean,
    Byte,
    LogicLong,
    OptionalLogicLong,
    DataReference,
    OptionalPawn,
    OptionalTask,
    OptionalTaskCollection,
    OptionalVarIntArray,
    OptionalState,
    OptionalDumpTaskState,
    OptionalProfileData
}
