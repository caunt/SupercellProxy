using System.Globalization;

using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.MapGame.Events.Fields;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>
/// <para>One polymorphic native map-game event carried by server command 274.</para>
/// </summary>
public sealed record MapGameEvent
{
    private static readonly Dictionary<int, MapGameEventFieldSchema[]> Schemas = CreateSchemas();

    /// <summary>
    /// Initializes a new <see cref="MapGameEvent"/> instance.
    /// </summary>
    public MapGameEvent(int type, ReadOnlyMemory<MapGameEventField> fields)
    {
        if (!Schemas.TryGetValue(type, out MapGameEventFieldSchema[]? schemas))
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Map-game event type {type} is not supported by the native 1.72.84 factory."));

        if (fields.Length != schemas.Length)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Map-game event type {type} has an invalid field count: {fields.Length}."));

        for (int index = 0; index < schemas.Length; index++)
        {
            if (!schemas[index].IsValid(fields.Span[index]))
                throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Map-game event type {type} field {index} does not match the native schema."));
        }

        Type = type;
        Fields = fields.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">Fields</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameEventField> Fields { get; }

    /// <summary>
    /// Gets the <c language="csharp">Type</c> value.
    /// </summary>
    public int Type { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameEvent Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int type = stream.ReadVariableInt();

        if (!Schemas.TryGetValue(type, out MapGameEventFieldSchema[]? schemas))
            throw new NotSupportedException(string.Create(CultureInfo.InvariantCulture, $"Map-game event type {type} is not supported by the native 1.72.84 factory."));

        MapGameEventField[] fields = new MapGameEventField[schemas.Length];

        for (int index = 0; index < fields.Length; index++)
            fields[index] = schemas[index].Decode(stream, dataResolver);

        return new MapGameEvent(type, fields);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Type);

        foreach (MapGameEventField field in Fields.Span)
            field.Encode(stream);
    }

    private static Dictionary<int, MapGameEventFieldSchema[]> CreateSchemas()
    {
        MapGameEventFieldSchema variableInt = new(MapGameEventFieldType.VariableInt);
        MapGameEventFieldSchema boolean = new(MapGameEventFieldType.Boolean);
        MapGameEventFieldSchema byteField = new(MapGameEventFieldType.Byte);
        MapGameEventFieldSchema logicLong = new(MapGameEventFieldType.LongIdentifier);
        MapGameEventFieldSchema optionalLongIdentifier = new(MapGameEventFieldType.OptionalLongIdentifier);
        MapGameEventFieldSchema dataReference = new(MapGameEventFieldType.DataReference);
        MapGameEventFieldSchema optionalPawn = new(MapGameEventFieldType.OptionalPawn);
        MapGameEventFieldSchema optionalTask = new(MapGameEventFieldType.OptionalTask);

        MapGameEventFieldSchema optionalTaskCollection = new(MapGameEventFieldType.OptionalTaskCollection);

        MapGameEventFieldSchema optionalVariableIntArray = new(MapGameEventFieldType.OptionalVariableIntArray);

        MapGameEventFieldSchema[] pawnAndTask = [optionalPawn, optionalTask];

        Dictionary<int, MapGameEventFieldSchema[]> schemas = [];
        AddFirstSchemas();
        AddRemainingSchemas();

        return schemas;

        void AddFirstSchemas()
        {
            schemas[key: 1] =
            [
                optionalLongIdentifier,
                variableInt,
                new(MapGameEventFieldType.OptionalState),
                optionalPawn,
                boolean,
            ];
            schemas[key: 2] =
            [
                variableInt,
                variableInt,
                variableInt,
                optionalPawn,
                optionalVariableIntArray,
                optionalVariableIntArray,
                optionalTask,
                optionalTask,
                optionalTaskCollection,
            ];
            schemas[key: 4] = pawnAndTask;
            schemas[key: 5] = [logicLong, variableInt, variableInt];
            schemas[key: 6] = pawnAndTask;
            schemas[key: 7] = pawnAndTask;
            schemas[key: 8] = pawnAndTask;
            schemas[key: 9] = pawnAndTask;
            schemas[key: 10] = pawnAndTask;
            schemas[key: 11] = pawnAndTask;
            schemas[key: 12] = pawnAndTask;
            schemas[key: 13] = pawnAndTask;
            schemas[key: 14] = [optionalPawn, optionalTask, variableInt];
            schemas[key: 15] = [optionalPawn, optionalTask, variableInt];
            schemas[key: 16] = pawnAndTask;
            schemas[key: 17] = [optionalPawn, optionalTask, optionalVariableIntArray];
            schemas[key: 18] =
            [
                optionalLongIdentifier,
                optionalPawn,
                new(MapGameEventFieldType.DataReference, ExpectedTableIdentifier: 219),
                variableInt,
                variableInt,
            ];
            schemas[key: 19] = pawnAndTask;
            schemas[key: 20] = pawnAndTask;
        }

        void AddRemainingSchemas()
        {
            schemas[key: 21] = [optionalPawn, optionalTask, optionalVariableIntArray];
            schemas[key: 22] = [optionalLongIdentifier, variableInt, dataReference, byteField];
            schemas[key: 23] = [optionalLongIdentifier, variableInt, dataReference];
            schemas[key: 24] = [optionalLongIdentifier, variableInt, dataReference];
            schemas[key: 25] = [optionalLongIdentifier, variableInt, new(MapGameEventFieldType.DataReference, ExpectedTableIdentifier: 162)];
            schemas[key: 26] = [variableInt, boolean];
            schemas[key: 27] =
            [
                logicLong,
                variableInt,
                variableInt,
                new(MapGameEventFieldType.DataReference, ExpectedTableIdentifier: 226),
            ];
            schemas[key: 28] = [optionalPawn];
            schemas[key: 29] =
            [
                variableInt,
                optionalPawn,
                optionalTask,
                new(MapGameEventFieldType.OptionalDumpTaskState),
            ];
            schemas[key: 30] = [optionalPawn];
            schemas[key: 31] = [optionalPawn];
            schemas[key: 32] = [optionalPawn, optionalTask, optionalVariableIntArray];
            schemas[key: 33] = [variableInt, optionalPawn];
            schemas[key: 34] = pawnAndTask;
            schemas[key: 35] = [optionalPawn, optionalTask, variableInt];
            schemas[key: 36] = pawnAndTask;
            schemas[key: 37] =
            [
                optionalLongIdentifier,
                optionalPawn,
                new(MapGameEventFieldType.DataReference, ExpectedTableIdentifier: 219),
                variableInt,
                variableInt,
            ];
            schemas[key: 38] = [optionalPawn, optionalTaskCollection];
            schemas[key: 39] = [variableInt, new(MapGameEventFieldType.OptionalProfileData)];
            schemas[key: 40] = [variableInt, new(MapGameEventFieldType.DataReference, ExpectedTableIdentifier: 260)];
        }
    }
}
