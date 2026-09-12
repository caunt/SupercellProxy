using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Protocol.MapGame.Events.Fields;
using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>
/// Defines the Map Game Event Field Schema contract.
/// </summary>
/// <summary>
/// Defines the Field Type contract.
/// </summary>
/// <summary>
/// Defines the Expected Table Id contract.
/// </summary>
public sealed record MapGameEventFieldSchema(
    MapGameEventFieldType FieldType,
    [property: System.Text.Json.Serialization.JsonPropertyName("ExpectedTableId")] int ExpectedTableIdentifier = -1
)
{
    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public MapGameEventField Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return FieldType switch
        {
            MapGameEventFieldType.VariableInt => new MapGameEventVariableIntField(stream.ReadVariableInt()),
            MapGameEventFieldType.Boolean => new MapGameEventBooleanField(stream.ReadBoolean()),
            MapGameEventFieldType.Byte => new MapGameEventByteField(unchecked(sbyte.CreateTruncating(stream.ReadByte()))),
            MapGameEventFieldType.LongIdentifier => new MapGameEventLongIdentifierField(stream.ReadLongIdentifier()),
            MapGameEventFieldType.OptionalLongIdentifier => new MapGameEventOptionalLongIdentifierField(MapGameFieldCodec.ReadOptionalLongIdentifier(stream)),
            MapGameEventFieldType.DataReference => new MapGameEventDataReferenceField(stream.ReadVariableInt(), ExpectedTableIdentifier),
            MapGameEventFieldType.OptionalPawn => new MapGameEventOptionalPawnField(stream.ReadBoolean() ? MapGamePawn.Decode(stream) : null),
            MapGameEventFieldType.OptionalTask => new MapGameEventOptionalTaskField(stream.ReadBoolean() ? MapGameTask.Decode(stream, dataResolver) : null),
            MapGameEventFieldType.OptionalTaskCollection =>
                new MapGameEventOptionalTaskCollectionField(stream.ReadBoolean() ? MapGameTaskCollection.Decode(stream, dataResolver) : null),
            MapGameEventFieldType.OptionalVariableIntArray => new MapGameEventOptionalVariableIntArrayField(stream.ReadBoolean() ? CommandVariableIntArrayField.Decode(stream).Values : null),
            MapGameEventFieldType.OptionalState => new MapGameEventOptionalStateField(stream.ReadBoolean() ? MapGameState.Decode(stream, dataResolver) : null),
            MapGameEventFieldType.OptionalDumpTaskState =>
                new MapGameEventOptionalDumpTaskStateField(stream.ReadBoolean() ? MapGameDumpTaskStatePayload.Decode(stream) : null),
            MapGameEventFieldType.OptionalProfileData => new MapGameEventOptionalProfileDataField(stream.ReadBoolean() ? MapGameEventProfileData.Decode(stream) : null),
            _ => throw new InvalidDataException($"Unsupported map-game event field type: {FieldType}."),
        };
    }

    /// <summary>
    /// Provides the Is Valid value or operation.
    /// </summary>
    public bool IsValid(MapGameEventField field)
    {
        ArgumentNullException.ThrowIfNull(field);

        return field.FieldType == FieldType && (field is not MapGameEventDataReferenceField dataReference
            || dataReference.ExpectedTableIdentifier == ExpectedTableIdentifier);
    }
}
