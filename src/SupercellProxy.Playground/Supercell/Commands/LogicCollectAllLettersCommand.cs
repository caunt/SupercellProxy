using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Collects all eligible letters.
/// </summary>
public sealed record LogicCollectAllLettersCommand : LogicCommand
{
    public const int CommandType = 672;

    public LogicCollectAllLettersCommand(int executeSubTick = -1, LogicCommandData? debugData0 = null, LogicCommandData? debugData1 = null)
        : base(executeSubTick, debugData0, debugData1)
    {
    }

    public override int Type => CommandType;

    internal static LogicCollectAllLettersCommand Decode(SupercellStream stream, LogicEnvironment environment)
    {
        var fields = DecodeLogicCommand(stream, environment);
        return new LogicCollectAllLettersCommand(fields.ExecuteSubTick, fields.DebugData0, fields.DebugData1);
    }

    internal override void EncodeBody(SupercellStream stream, LogicEnvironment environment)
    {
        EncodeLogicCommand(stream, environment);
    }
}
