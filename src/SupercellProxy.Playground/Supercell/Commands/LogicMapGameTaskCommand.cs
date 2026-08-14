using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Commands whose native 1.72.84 bodies contain the shared optional map-game task structure.
/// </summary>
public sealed record LogicMapGameTaskCommand : LogicCommand
{
    public static readonly int[] CommandTypes = [278, 279, 280, 281, 282, 283, 284, 290, 291, 295, 310, 312, 314];
    private static readonly HashSet<int> _typesWithOptionalValues = [284, 291, 310];

    public LogicMapGameTaskCommand(
        int type,
        LogicMapGameTask? task,
        ReadOnlyMemory<int>? optionalValues = null,
        int executeSubTick = -1,
        LogicCommandData? debugData0 = null,
        LogicCommandData? debugData1 = null)
        : base(executeSubTick, debugData0, debugData1)
    {
        if (!CommandTypes.Contains(type))
            throw new NotSupportedException($"Logic command type {type} does not use the map-game task schema.");

        if (!_typesWithOptionalValues.Contains(type) && optionalValues is not null)
            throw new InvalidDataException($"Logic command type {type} has no optional value array.");

        Type = type;
        Task = task;
        OptionalValues = optionalValues?.ToArray();
    }

    public override int Type { get; }
    public LogicMapGameTask? Task { get; }
    public ReadOnlyMemory<int>? OptionalValues { get; }

    internal static LogicMapGameTaskCommand Decode(int type, SupercellStream stream, LogicEnvironment environment, ILogicCommandDataResolver? dataResolver)
    {
        var commandFields = DecodeLogicCommand(stream, environment);
        var task = stream.ReadBoolean() ? LogicMapGameTask.Decode(stream, dataResolver) : null;
        ReadOnlyMemory<int>? optionalValues = null;

        if (_typesWithOptionalValues.Contains(type) && stream.ReadBoolean())
            optionalValues = LogicCommandVarIntArrayField.DecodeValues(stream.ReadVarInt(), stream);

        return new LogicMapGameTaskCommand(
            type,
            task,
            optionalValues,
            commandFields.ExecuteSubTick,
            commandFields.DebugData0,
            commandFields.DebugData1);
    }

    internal override void EncodeBody(SupercellStream stream, LogicEnvironment environment)
    {
        EncodeLogicCommand(stream, environment);
        stream.WriteBoolean(Task is not null);
        Task?.Encode(stream);

        if (!_typesWithOptionalValues.Contains(Type))
            return;

        stream.WriteBoolean(OptionalValues is not null);

        if (OptionalValues is null)
            return;

        stream.WriteVarInt(OptionalValues.Value.Length);

        foreach (var value in OptionalValues.Value.Span)
            stream.WriteVarInt(value);
    }
}
