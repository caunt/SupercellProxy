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
    /// <summary>Records a participant collecting a chicken task.</summary>
    public const int ChickenTaskCollectedType = 21;

    /// <summary>Records a participant's contribution to the shared chicken goal.</summary>
    public const int ChickensCollectedType = 18;

    /// <summary>Completes a personal Valley task, including dump and delivery tasks, with its authoritative pawn and task.</summary>
    public const int DumpTaskCompletedType = 4;

    /// <summary>Removes the participant's escaped sanctuary-animal tasks.</summary>
    public const int EscapedSanctuaryAnimalsRemovedType = 38;

    /// <summary>Updates the transient Valley flag of a linked Chronos event.</summary>
    public const int EventFlagUpdatedType = 26;

    /// <summary>Collects a free gas-station offer.</summary>
    public const int GasStationCollectedType = 34;

    /// <summary>Collects a paid gas-station offer.</summary>
    public const int GasStationPurchasedType = 35;

    /// <summary>Replaces a participant's gas-station offer.</summary>
    public const int GasStationUpdatedType = 36;

    /// <summary>Adds an emoji to a map node.</summary>
    public const int NodeEmojiAddedType = 22;

    /// <summary>Marks an existing node emoji as collected by a Valley participant.</summary>
    public const int NodeEmojiCollectedType = 24;

    /// <summary>Removes every node emoji matching the supplied avatar, node, and data filters.</summary>
    public const int NodeEmojiRemovedType = 23;

    /// <summary>Completes an obstacle and records its helper.</summary>
    public const int ObstacleCompletedType = 17;

    /// <summary>Requests another participant's help with an obstacle.</summary>
    public const int ObstacleHelpRequestedType = 16;

    /// <summary>Completes an obstacle using the transmitted diamond charge.</summary>
    public const int ObstaclePaidCompletionType = 15;

    /// <summary>Refreshes an existing pawn's level, name, options and neighborhood profile.</summary>
    public const int PawnDetailsUpdatedType = 28;

    /// <summary>Moves a Valley pawn and carries the authoritative movement projection.</summary>
    public const int PawnMovedType = 2;

    /// <summary>Sets or clears the neighborhood task attached to a pawn.</summary>
    public const int PawnNeighborhoodTaskUpdatedType = 25;

    /// <summary>Refreshes an existing pawn's neighborhood identity and emblem.</summary>
    public const int PawnNeighborhoodUpdatedType = 31;

    /// <summary>Changes a pawn's retained notification and value entries.</summary>
    public const int PawnNotificationUpdatedType = 27;

    /// <summary>Adds or refreshes a Valley pawn's social profile.</summary>
    public const int PawnProfileUpdatedType = 30;

    /// <summary>Expires an existing personal task using the server's task state.</summary>
    public const int PawnTaskExpiredType = 6;

    /// <summary>Replaces a personal task at its map node.</summary>
    public const int PawnTaskUpdatedType = 8;

    /// <summary>Assigns an existing shared sanctuary-animal task to a Valley participant.</summary>
    public const int SanctuaryAnimalCollectedType = 32;

    /// <summary>Records a participant's contribution to the shared sanctuary-animal goal.</summary>
    public const int SanctuaryAnimalsDeliveredType = 37;

    /// <summary>Offloads a participant's carried sanctuary animals.</summary>
    public const int SanctuaryAnimalsOffloadedType = 33;

    /// <summary>Confirms or releases a participant's shared-task submission.</summary>
    public const int SharedTaskCommitResultType = 29;

    /// <summary>Processes shared-task completion and participant rewards.</summary>
    public const int SharedTaskCompletedType = 13;

    /// <summary>Expires an existing task in the shared task group.</summary>
    public const int SharedTaskExpiredType = 9;

    /// <summary>Assigns a free participant slot in a shared task.</summary>
    public const int SharedTaskJoinedType = 12;

    /// <summary>Marks a shared dump task and records the participant who marked it.</summary>
    public const int SharedTaskMarkedType = 20;

    /// <summary>Removes an existing task from the shared task group.</summary>
    public const int SharedTaskRemovedType = 10;

    /// <summary>Replaces a task in the shared task group.</summary>
    public const int SharedTaskUpdatedType = 11;

    /// <summary>Identifies a shared map-game state synchronization event.</summary>
    public const int StateSynchronizationType = 1;

    /// <summary>Skips a task's remaining replacement cooldown.</summary>
    public const int TaskCooldownSkippedType = 14;

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
            schemas[SharedTaskExpiredType] = pawnAndTask;
            schemas[SharedTaskRemovedType] = pawnAndTask;
            schemas[SharedTaskUpdatedType] = pawnAndTask;
            schemas[SharedTaskJoinedType] = pawnAndTask;
            schemas[SharedTaskCompletedType] = pawnAndTask;
            schemas[TaskCooldownSkippedType] = [optionalPawn, optionalTask, varInt];
            schemas[ObstaclePaidCompletionType] = [optionalPawn, optionalTask, varInt];
            schemas[ObstacleHelpRequestedType] = pawnAndTask;
            schemas[ObstacleCompletedType] = [optionalPawn, optionalTask, optionalVarIntArray];
            schemas[ChickensCollectedType] =
            [
                optionalLongId,
                optionalPawn,
                new(MapGameEventFieldType.DataReference, ExpectedTableId: 219),
                varInt,
                varInt,
            ];
            schemas[key: 19] = pawnAndTask;
            schemas[SharedTaskMarkedType] = pawnAndTask;
        }

        void AddRemainingSchemas()
        {
            schemas[ChickenTaskCollectedType] = [optionalPawn, optionalTask, optionalVarIntArray];
            schemas[NodeEmojiAddedType] = [optionalLongId, varInt, dataReference, int32Field];
            schemas[NodeEmojiRemovedType] = [optionalLongId, varInt, dataReference];
            schemas[NodeEmojiCollectedType] = [optionalLongId, varInt, dataReference];
            schemas[PawnNeighborhoodTaskUpdatedType] = [optionalLongId, varInt, new(MapGameEventFieldType.DataReference, ExpectedTableId: 162)];
            schemas[EventFlagUpdatedType] = [varInt, boolean];
            schemas[PawnNotificationUpdatedType] =
            [
                logicLong,
                varInt,
                varInt,
                new(MapGameEventFieldType.DataReference, ExpectedTableId: 226),
            ];
            schemas[PawnDetailsUpdatedType] = [optionalPawn];
            schemas[SharedTaskCommitResultType] =
            [
                varInt,
                optionalPawn,
                optionalTask,
                new(MapGameEventFieldType.OptionalDumpTaskState),
            ];
            schemas[PawnProfileUpdatedType] = [optionalPawn];
            schemas[PawnNeighborhoodUpdatedType] = [optionalPawn];
            schemas[SanctuaryAnimalCollectedType] = [optionalPawn, optionalTask, optionalVarIntArray];
            schemas[SanctuaryAnimalsOffloadedType] = [varInt, optionalPawn];
            schemas[GasStationCollectedType] = pawnAndTask;
            schemas[GasStationPurchasedType] = [optionalPawn, optionalTask, varInt];
            schemas[GasStationUpdatedType] = pawnAndTask;
            schemas[SanctuaryAnimalsDeliveredType] =
            [
                optionalLongId,
                optionalPawn,
                new(MapGameEventFieldType.DataReference, ExpectedTableId: 219),
                varInt,
                varInt,
            ];
            schemas[EscapedSanctuaryAnimalsRemovedType] = [optionalPawn, optionalTaskCollection];
            schemas[key: 39] = [varInt, new(MapGameEventFieldType.OptionalProfileData)];
            schemas[key: 40] = [varInt, new(MapGameEventFieldType.DataReference, ExpectedTableId: 260)];
        }
    }
}
