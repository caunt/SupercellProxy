using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

internal sealed record MapGameEventFieldSchema(
    MapGameEventFieldType FieldType,
    int ExpectedTableId = -1
)
{
    internal MapGameEventField Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        return FieldType switch
        {
            MapGameEventFieldType.VarInt => new MapGameEventVarIntField(stream.ReadVarInt()),
            MapGameEventFieldType.Boolean => new MapGameEventBooleanField(stream.ReadBoolean()),
            MapGameEventFieldType.Byte => new MapGameEventByteField(
                unchecked(sbyte.CreateTruncating(stream.ReadByte()))
            ),
            MapGameEventFieldType.LongId => new MapGameEventLongIdField(stream.ReadLongId()),
            MapGameEventFieldType.OptionalLongId => new MapGameEventOptionalLongIdField(
                MapGameWire.ReadOptionalLongId(stream)
            ),
            MapGameEventFieldType.DataReference => new MapGameEventDataReferenceField(
                stream.ReadVarInt(),
                ExpectedTableId
            ),
            MapGameEventFieldType.OptionalPawn => new MapGameEventOptionalPawnField(
                stream.ReadBoolean() ? MapGamePawn.Decode(stream) : null
            ),
            MapGameEventFieldType.OptionalTask => new MapGameEventOptionalTaskField(
                stream.ReadBoolean() ? MapGameTask.Decode(stream, dataResolver) : null
            ),
            MapGameEventFieldType.OptionalTaskCollection =>
                new MapGameEventOptionalTaskCollectionField(
                    stream.ReadBoolean() ? MapGameTaskCollection.Decode(stream, dataResolver) : null
                ),
            MapGameEventFieldType.OptionalVarIntArray => new MapGameEventOptionalVarIntArrayField(
                stream.ReadBoolean() ? CommandVarIntArrayField.Decode(stream).Values : null
            ),
            MapGameEventFieldType.OptionalState => new MapGameEventOptionalStateField(
                stream.ReadBoolean() ? MapGameState.Decode(stream, dataResolver) : null
            ),
            MapGameEventFieldType.OptionalDumpTaskState =>
                new MapGameEventOptionalDumpTaskStateField(
                    stream.ReadBoolean() ? MapGameDumpTaskStatePayload.Decode(stream) : null
                ),
            MapGameEventFieldType.OptionalProfileData => new MapGameEventOptionalProfileDataField(
                stream.ReadBoolean() ? MapGameEventProfileData.Decode(stream) : null
            ),
            _ => throw new InvalidDataException(
                $"Unsupported map-game event field type: {FieldType}."
            ),
        };
    }

    internal bool IsValid(MapGameEventField field)
    {
        if (field.FieldType != FieldType)
            return false;

        return field is not MapGameEventDataReferenceField dataReference
            || dataReference.ExpectedTableId == ExpectedTableId;
    }
}
