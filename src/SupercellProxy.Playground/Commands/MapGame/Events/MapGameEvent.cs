using System.Globalization;
using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>One polymorphic native map-game event carried by server command 274.</para>
/// </summary>
public sealed record MapGameEvent
{
    private static readonly Dictionary<int, MapGameEventFieldSchema[]> _schemas = CreateSchemas();

    /// <summary>
    /// Initializes a new <see cref="MapGameEvent"/> instance.
    /// </summary>
    public MapGameEvent(int type, ReadOnlyMemory<MapGameEventField> fields)
    {
        if (!_schemas.TryGetValue(type, out var schemas))
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Map-game event type {type} is not supported by the native 1.72.84 factory."
                )
            );

        if (fields.Length != schemas.Length)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Map-game event type {type} has an invalid field count: {fields.Length}."
                )
            );

        for (var i = 0; i < schemas.Length; i++)
        {
            if (!schemas[i].IsValid(fields.Span[i]))
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Map-game event type {type} field {i} does not match the native schema."
                    )
                );
        }

        Type = type;
        Fields = fields.ToArray();
    }

    /// <summary>
    /// Gets the <c>Type</c> value.
    /// </summary>
    public int Type { get; }

    /// <summary>
    /// Gets the <c>Fields</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameEventField> Fields { get; }

    internal static MapGameEvent Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        var type = stream.ReadVarInt();

        if (!_schemas.TryGetValue(type, out var schemas))
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Map-game event type {type} is not supported by the native 1.72.84 factory."
                )
            );

        var fields = new MapGameEventField[schemas.Length];

        for (var i = 0; i < fields.Length; i++)
            fields[i] = schemas[i].Decode(stream, dataResolver);

        return new MapGameEvent(type, fields);
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Type);

        foreach (var field in Fields.Span)
            field.Encode(stream);
    }

    private static Dictionary<int, MapGameEventFieldSchema[]> CreateSchemas()
    {
        var varInt = new MapGameEventFieldSchema(MapGameEventFieldType.VarInt);
        var boolean = new MapGameEventFieldSchema(MapGameEventFieldType.Boolean);
        var byteField = new MapGameEventFieldSchema(MapGameEventFieldType.Byte);
        var logicLong = new MapGameEventFieldSchema(MapGameEventFieldType.LongId);
        var optionalLongId = new MapGameEventFieldSchema(MapGameEventFieldType.OptionalLongId);
        var dataReference = new MapGameEventFieldSchema(MapGameEventFieldType.DataReference);
        var optionalPawn = new MapGameEventFieldSchema(MapGameEventFieldType.OptionalPawn);
        var optionalTask = new MapGameEventFieldSchema(MapGameEventFieldType.OptionalTask);
        var optionalTaskCollection = new MapGameEventFieldSchema(
            MapGameEventFieldType.OptionalTaskCollection
        );
        var optionalVarIntArray = new MapGameEventFieldSchema(
            MapGameEventFieldType.OptionalVarIntArray
        );
        var pawnAndTask = new[] { optionalPawn, optionalTask };

        var schemas = new Dictionary<int, MapGameEventFieldSchema[]>();
        AddFirstSchemas();
        AddRemainingSchemas();
        return schemas;

        void AddFirstSchemas()
        {
            schemas[1] =
            [
                optionalLongId,
                varInt,
                new(MapGameEventFieldType.OptionalState),
                optionalPawn,
                boolean,
            ];
            schemas[2] =
            [
                varInt,
                varInt,
                varInt,
                optionalPawn,
                optionalVarIntArray,
                optionalVarIntArray,
                optionalTask,
                optionalTask,
                optionalTaskCollection,
            ];
            schemas[4] = pawnAndTask;
            schemas[5] = [logicLong, varInt, varInt];
            schemas[6] = pawnAndTask;
            schemas[7] = pawnAndTask;
            schemas[8] = pawnAndTask;
            schemas[9] = pawnAndTask;
            schemas[10] = pawnAndTask;
            schemas[11] = pawnAndTask;
            schemas[12] = pawnAndTask;
            schemas[13] = pawnAndTask;
            schemas[14] = [optionalPawn, optionalTask, varInt];
            schemas[15] = [optionalPawn, optionalTask, varInt];
            schemas[16] = pawnAndTask;
            schemas[17] = [optionalPawn, optionalTask, optionalVarIntArray];
            schemas[18] =
            [
                optionalLongId,
                optionalPawn,
                new(MapGameEventFieldType.DataReference, 219),
                varInt,
                varInt,
            ];
            schemas[19] = pawnAndTask;
            schemas[20] = pawnAndTask;
        }

        void AddRemainingSchemas()
        {
            schemas[21] = [optionalPawn, optionalTask, optionalVarIntArray];
            schemas[22] = [optionalLongId, varInt, dataReference, byteField];
            schemas[23] = [optionalLongId, varInt, dataReference];
            schemas[24] = [optionalLongId, varInt, dataReference];
            schemas[25] = [optionalLongId, varInt, new(MapGameEventFieldType.DataReference, 162)];
            schemas[26] = [varInt, boolean];
            schemas[27] =
            [
                logicLong,
                varInt,
                varInt,
                new(MapGameEventFieldType.DataReference, 226),
            ];
            schemas[28] = [optionalPawn];
            schemas[29] =
            [
                varInt,
                optionalPawn,
                optionalTask,
                new(MapGameEventFieldType.OptionalDumpTaskState),
            ];
            schemas[30] = [optionalPawn];
            schemas[31] = [optionalPawn];
            schemas[32] = [optionalPawn, optionalTask, optionalVarIntArray];
            schemas[33] = [varInt, optionalPawn];
            schemas[34] = pawnAndTask;
            schemas[35] = [optionalPawn, optionalTask, varInt];
            schemas[36] = pawnAndTask;
            schemas[37] =
            [
                optionalLongId,
                optionalPawn,
                new(MapGameEventFieldType.DataReference, 219),
                varInt,
                varInt,
            ];
            schemas[38] = [optionalPawn, optionalTaskCollection];
            schemas[39] = [varInt, new(MapGameEventFieldType.OptionalProfileData)];
            schemas[40] = [varInt, new(MapGameEventFieldType.DataReference, 260)];
        }
    }
}
