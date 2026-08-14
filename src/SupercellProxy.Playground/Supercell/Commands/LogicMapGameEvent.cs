using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// One polymorphic native map-game event carried by server command 274.
/// </summary>
public sealed record LogicMapGameEvent
{
    private static readonly Dictionary<int, LogicMapGameEventFieldSchema[]> _schemas = CreateSchemas();

    public LogicMapGameEvent(int type, ReadOnlyMemory<LogicMapGameEventField> fields)
    {
        if (!_schemas.TryGetValue(type, out var schemas))
            throw new NotSupportedException($"Map-game event type {type} is not supported by the native 1.72.84 factory.");

        if (fields.Length != schemas.Length)
            throw new InvalidDataException($"Map-game event type {type} has an invalid field count: {fields.Length}.");

        for (var i = 0; i < schemas.Length; i++)
        {
            if (!schemas[i].IsValid(fields.Span[i]))
                throw new InvalidDataException($"Map-game event type {type} field {i} does not match the native schema.");
        }

        Type = type;
        Fields = fields.ToArray();
    }

    public int Type { get; }
    public ReadOnlyMemory<LogicMapGameEventField> Fields { get; }

    internal static LogicMapGameEvent Decode(SupercellStream stream, ILogicCommandDataResolver? dataResolver)
    {
        var type = stream.ReadVarInt();

        if (!_schemas.TryGetValue(type, out var schemas))
            throw new NotSupportedException($"Map-game event type {type} is not supported by the native 1.72.84 factory.");

        var fields = new LogicMapGameEventField[schemas.Length];

        for (var i = 0; i < fields.Length; i++)
            fields[i] = schemas[i].Decode(stream, dataResolver);

        return new LogicMapGameEvent(type, fields);
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Type);

        foreach (var field in Fields.Span)
            field.Encode(stream);
    }

    private static Dictionary<int, LogicMapGameEventFieldSchema[]> CreateSchemas()
    {
        var varInt = new LogicMapGameEventFieldSchema(LogicMapGameEventFieldType.VarInt);
        var boolean = new LogicMapGameEventFieldSchema(LogicMapGameEventFieldType.Boolean);
        var byteField = new LogicMapGameEventFieldSchema(LogicMapGameEventFieldType.Byte);
        var logicLong = new LogicMapGameEventFieldSchema(LogicMapGameEventFieldType.LogicLong);
        var optionalLogicLong = new LogicMapGameEventFieldSchema(LogicMapGameEventFieldType.OptionalLogicLong);
        var dataReference = new LogicMapGameEventFieldSchema(LogicMapGameEventFieldType.DataReference);
        var optionalPawn = new LogicMapGameEventFieldSchema(LogicMapGameEventFieldType.OptionalPawn);
        var optionalTask = new LogicMapGameEventFieldSchema(LogicMapGameEventFieldType.OptionalTask);
        var optionalTaskCollection = new LogicMapGameEventFieldSchema(LogicMapGameEventFieldType.OptionalTaskCollection);
        var optionalVarIntArray = new LogicMapGameEventFieldSchema(LogicMapGameEventFieldType.OptionalVarIntArray);
        var pawnAndTask = new[] { optionalPawn, optionalTask };

        return new Dictionary<int, LogicMapGameEventFieldSchema[]>
        {
            [1] = [optionalLogicLong, varInt, new(LogicMapGameEventFieldType.OptionalState), optionalPawn, boolean],
            [2] = [varInt, varInt, varInt, optionalPawn, optionalVarIntArray, optionalVarIntArray, optionalTask, optionalTask, optionalTaskCollection],
            [4] = pawnAndTask,
            [5] = [logicLong, varInt, varInt],
            [6] = pawnAndTask,
            [7] = pawnAndTask,
            [8] = pawnAndTask,
            [9] = pawnAndTask,
            [10] = pawnAndTask,
            [11] = pawnAndTask,
            [12] = pawnAndTask,
            [13] = pawnAndTask,
            [14] = [optionalPawn, optionalTask, varInt],
            [15] = [optionalPawn, optionalTask, varInt],
            [16] = pawnAndTask,
            [17] = [optionalPawn, optionalTask, optionalVarIntArray],
            [18] = [optionalLogicLong, optionalPawn, new(LogicMapGameEventFieldType.DataReference, 219), varInt, varInt],
            [19] = pawnAndTask,
            [20] = pawnAndTask,
            [21] = [optionalPawn, optionalTask, optionalVarIntArray],
            [22] = [optionalLogicLong, varInt, dataReference, byteField],
            [23] = [optionalLogicLong, varInt, dataReference],
            [24] = [optionalLogicLong, varInt, dataReference],
            [25] = [optionalLogicLong, varInt, new(LogicMapGameEventFieldType.DataReference, 162)],
            [26] = [varInt, boolean],
            [27] = [logicLong, varInt, varInt, new(LogicMapGameEventFieldType.DataReference, 226)],
            [28] = [optionalPawn],
            [29] = [varInt, optionalPawn, optionalTask, new(LogicMapGameEventFieldType.OptionalDumpTaskState)],
            [30] = [optionalPawn],
            [31] = [optionalPawn],
            [32] = [optionalPawn, optionalTask, optionalVarIntArray],
            [33] = [varInt, optionalPawn],
            [34] = pawnAndTask,
            [35] = [optionalPawn, optionalTask, varInt],
            [36] = pawnAndTask,
            [37] = [optionalLogicLong, optionalPawn, new(LogicMapGameEventFieldType.DataReference, 219), varInt, varInt],
            [38] = [optionalPawn, optionalTaskCollection],
            [39] = [varInt, new(LogicMapGameEventFieldType.OptionalProfileData)],
            [40] = [varInt, new(LogicMapGameEventFieldType.DataReference, 260)]
        };
    }
}
