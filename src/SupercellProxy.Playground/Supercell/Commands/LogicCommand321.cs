using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Logic command 321. The native class and semantic field names are not present in the stripped client.
/// </summary>
public sealed record LogicCommand321 : LogicCommand
{
    public const int CommandType = 321;

    public LogicCommand321(
        LogicMapGamePawn? pawn,
        LogicMapGameTaskCollection? taskCollection,
        int executeSubTick = -1,
        LogicCommandData? debugData0 = null,
        LogicCommandData? debugData1 = null)
        : base(executeSubTick, debugData0, debugData1)
    {
        Pawn = pawn;
        TaskCollection = taskCollection;
    }

    public override int Type => CommandType;
    public LogicMapGamePawn? Pawn { get; }
    public LogicMapGameTaskCollection? TaskCollection { get; }

    internal static LogicCommand321 Decode(SupercellStream stream, LogicEnvironment environment, ILogicCommandDataResolver? dataResolver)
    {
        var commandFields = DecodeLogicCommand(stream, environment);
        var pawn = stream.ReadBoolean() ? LogicMapGamePawn.Decode(stream) : null;
        var taskCollection = stream.ReadBoolean() ? LogicMapGameTaskCollection.Decode(stream, dataResolver) : null;

        return new LogicCommand321(
            pawn,
            taskCollection,
            commandFields.ExecuteSubTick,
            commandFields.DebugData0,
            commandFields.DebugData1);
    }

    internal override void EncodeBody(SupercellStream stream, LogicEnvironment environment)
    {
        EncodeLogicCommand(stream, environment);
        stream.WriteBoolean(Pawn is not null);
        Pawn?.Encode(stream);
        stream.WriteBoolean(TaskCollection is not null);
        TaskCollection?.Encode(stream);
    }
}
