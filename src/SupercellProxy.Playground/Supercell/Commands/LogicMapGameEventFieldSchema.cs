using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

internal sealed record LogicMapGameEventFieldSchema(LogicMapGameEventFieldType FieldType, int ExpectedTableId = -1)
{
    internal LogicMapGameEventField Decode(SupercellStream stream, ILogicCommandDataResolver? dataResolver)
    {
        return FieldType switch
        {
            LogicMapGameEventFieldType.VarInt => new LogicMapGameEventVarIntField(stream.ReadVarInt()),
            LogicMapGameEventFieldType.Boolean => new LogicMapGameEventBooleanField(stream.ReadBoolean()),
            LogicMapGameEventFieldType.Byte => new LogicMapGameEventByteField(unchecked((sbyte)stream.ReadByte())),
            LogicMapGameEventFieldType.LogicLong => new LogicMapGameEventLogicLongField(stream.ReadLogicLong()),
            LogicMapGameEventFieldType.OptionalLogicLong => new LogicMapGameEventOptionalLogicLongField(LogicMapGameWire.ReadOptionalLogicLong(stream)),
            LogicMapGameEventFieldType.DataReference => new LogicMapGameEventDataReferenceField(stream.ReadVarInt(), ExpectedTableId),
            LogicMapGameEventFieldType.OptionalPawn => new LogicMapGameEventOptionalPawnField(stream.ReadBoolean() ? LogicMapGamePawn.Decode(stream) : null),
            LogicMapGameEventFieldType.OptionalTask => new LogicMapGameEventOptionalTaskField(stream.ReadBoolean() ? LogicMapGameTask.Decode(stream, dataResolver) : null),
            LogicMapGameEventFieldType.OptionalTaskCollection => new LogicMapGameEventOptionalTaskCollectionField(stream.ReadBoolean() ? LogicMapGameTaskCollection.Decode(stream, dataResolver) : null),
            LogicMapGameEventFieldType.OptionalVarIntArray => new LogicMapGameEventOptionalVarIntArrayField(stream.ReadBoolean() ? LogicCommandVarIntArrayField.Decode(stream).Values : null),
            LogicMapGameEventFieldType.OptionalState => new LogicMapGameEventOptionalStateField(stream.ReadBoolean() ? LogicMapGameState.Decode(stream, dataResolver) : null),
            LogicMapGameEventFieldType.OptionalDumpTaskState => new LogicMapGameEventOptionalDumpTaskStateField(stream.ReadBoolean() ? LogicMapGameDumpTaskStatePayload.Decode(stream) : null),
            LogicMapGameEventFieldType.OptionalProfileData => new LogicMapGameEventOptionalProfileDataField(stream.ReadBoolean() ? LogicMapGameEventProfileData.Decode(stream) : null),
            _ => throw new InvalidDataException($"Unsupported map-game event field type: {FieldType}.")
        };
    }

    internal bool IsValid(LogicMapGameEventField field)
    {
        if (field.FieldType != FieldType)
            return false;

        return field is not LogicMapGameEventDataReferenceField dataReference || dataReference.ExpectedTableId == ExpectedTableId;
    }
}
