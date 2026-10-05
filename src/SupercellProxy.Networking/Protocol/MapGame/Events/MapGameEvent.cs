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
    /// <summary>Completes one personal Valley dump task and carries its authoritative pawn and task.</summary>
    public const int DumpTaskCompletedType = 4;

    /// <summary>Adds an emoji to a map node.</summary>
    public const int NodeEmojiAddedType = 22;

    /// <summary>Marks an existing node emoji as collected by a Valley participant.</summary>
    public const int NodeEmojiCollectedType = 24;

    /// <summary>Removes every node emoji matching the supplied avatar, node, and data filters.</summary>
    public const int NodeEmojiRemovedType = 23;

    /// <summary>Moves a Valley pawn and carries the authoritative movement projection.</summary>
    public const int PawnMovedType = 2;

    /// <summary>Adds or refreshes a Valley pawn's social profile.</summary>
    public const int PawnProfileUpdatedType = 30;

    /// <summary>Expires an existing personal task using the server's task state.</summary>
    public const int PawnTaskExpiredType = 6;

    /// <summary>Replaces a personal task at its map node.</summary>
    public const int PawnTaskUpdatedType = 8;

    /// <summary>Assigns an existing shared sanctuary-animal task to a Valley participant.</summary>
    public const int SanctuaryAnimalCollectedType = 32;

    /// <summary>Replaces a task in the shared task group.</summary>
    public const int SharedTaskUpdatedType = 11;

    /// <summary>Identifies a shared map-game state synchronization event.</summary>
    public const int StateSynchronizationType = 1;

    /// <summary>Removes an existing task from its owner or shared group.</summary>
    public const int TaskRemovedType = 7;

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
        int type = stream.ReadVarInt();

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
        stream.WriteVarInt(Type);

        foreach (MapGameEventField field in Fields.Span)
            field.Encode(stream);
    }

    private static Dictionary<int, MapGameEventFieldSchema[]> CreateSchemas()
    {
        MapGameEventFieldSchema varInt = new(MapGameEventFieldType.VarInt);
        MapGameEventFieldSchema boolean = new(MapGameEventFieldType.Boolean);
        MapGameEventFieldSchema int32Field = new(MapGameEventFieldType.Int32);
        MapGameEventFieldSchema logicLong = new(MapGameEventFieldType.LongId);
        MapGameEventFieldSchema optionalLongId = new(MapGameEventFieldType.OptionalLongId);
        MapGameEventFieldSchema dataReference = new(MapGameEventFieldType.DataReference);
        MapGameEventFieldSchema optionalPawn = new(MapGameEventFieldType.OptionalPawn);
        MapGameEventFieldSchema optionalTask = new(MapGameEventFieldType.OptionalTask);

        MapGameEventFieldSchema optionalTaskCollection = new(MapGameEventFieldType.OptionalTaskCollection);

        MapGameEventFieldSchema optionalVarIntArray = new(MapGameEventFieldType.OptionalVarIntArray);

        MapGameEventFieldSchema[] pawnAndTask = [optionalPawn, optionalTask];

        Dictionary<int, MapGameEventFieldSchema[]> schemas = [];
        AddFirstSchemas();
        AddRemainingSchemas();

        return schemas;

        void AddFirstSchemas()
        {
            schemas[StateSynchronizationType] =
            [
                optionalLongId,
                varInt,
                new(MapGameEventFieldType.OptionalState),
                optionalPawn,
                boolean,
            ];
            schemas[PawnMovedType] =
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
            schemas[DumpTaskCompletedType] = pawnAndTask;
            schemas[key: 5] = [logicLong, varInt, varInt];
            schemas[PawnTaskExpiredType] = pawnAndTask;
            schemas[TaskRemovedType] = pawnAndTask;
            schemas[PawnTaskUpdatedType] = pawnAndTask;
            schemas[key: 9] = pawnAndTask;
            schemas[key: 10] = pawnAndTask;
            schemas[SharedTaskUpdatedType] = pawnAndTask;
            schemas[key: 12] = pawnAndTask;
            schemas[key: 13] = pawnAndTask;
            schemas[key: 14] = [optionalPawn, optionalTask, varInt];
            schemas[key: 15] = [optionalPawn, optionalTask, varInt];
            schemas[key: 16] = pawnAndTask;
            schemas[key: 17] = [optionalPawn, optionalTask, optionalVarIntArray];
            schemas[key: 18] =
            [
                optionalLongId,
                optionalPawn,
                new(MapGameEventFieldType.DataReference, ExpectedTableId: 219),
                varInt,
                varInt,
            ];
            schemas[key: 19] = pawnAndTask;
            schemas[key: 20] = pawnAndTask;
        }

        void AddRemainingSchemas()
        {
            schemas[key: 21] = [optionalPawn, optionalTask, optionalVarIntArray];
            schemas[NodeEmojiAddedType] = [optionalLongId, varInt, dataReference, int32Field];
            schemas[NodeEmojiRemovedType] = [optionalLongId, varInt, dataReference];
            schemas[NodeEmojiCollectedType] = [optionalLongId, varInt, dataReference];
            schemas[key: 25] = [optionalLongId, varInt, new(MapGameEventFieldType.DataReference, ExpectedTableId: 162)];
            schemas[key: 26] = [varInt, boolean];
            schemas[key: 27] =
            [
                logicLong,
                varInt,
                varInt,
                new(MapGameEventFieldType.DataReference, ExpectedTableId: 226),
            ];
            schemas[key: 28] = [optionalPawn];
            schemas[key: 29] =
            [
                varInt,
                optionalPawn,
                optionalTask,
                new(MapGameEventFieldType.OptionalDumpTaskState),
            ];
            schemas[PawnProfileUpdatedType] = [optionalPawn];
            schemas[key: 31] = [optionalPawn];
            schemas[SanctuaryAnimalCollectedType] = [optionalPawn, optionalTask, optionalVarIntArray];
            schemas[key: 33] = [varInt, optionalPawn];
            schemas[key: 34] = pawnAndTask;
            schemas[key: 35] = [optionalPawn, optionalTask, varInt];
            schemas[key: 36] = pawnAndTask;
            schemas[key: 37] =
            [
                optionalLongId,
                optionalPawn,
                new(MapGameEventFieldType.DataReference, ExpectedTableId: 219),
                varInt,
                varInt,
            ];
            schemas[key: 38] = [optionalPawn, optionalTaskCollection];
            schemas[key: 39] = [varInt, new(MapGameEventFieldType.OptionalProfileData)];
            schemas[key: 40] = [varInt, new(MapGameEventFieldType.DataReference, ExpectedTableId: 260)];
        }
    }
}
