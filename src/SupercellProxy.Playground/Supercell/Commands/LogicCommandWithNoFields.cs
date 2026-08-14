using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// A command whose native class adds no fields to <see cref="LogicCommand"/>.
/// </summary>
public sealed record LogicCommandWithNoFields : LogicCommand
{
    internal static readonly int[] CommandTypes =
    [
        84, 97, 391,
        503, 505, 507, 508, 510, 513, 515, 517, 524, 526, 527, 528, 529, 530, 533, 536, 537, 539,
        541, 546, 547, 548, 551, 553, 554, 555, 557, 562, 564, 566, 571, 572, 575, 578, 580, 582, 583,
        587, 590, 592, 593, 595, 596, 598, 604, 613, 614, 615, 616, 622, 628, 630, 631, 633, 635, 637,
        639, 640, 644, 645, 647, 652, 653, 659, 660, 662, 663, 664, 668, 671, 674, 676, 677, 678, 681,
        682, 683, 684, 685, 688, 694, 695, 697, 698, 699
    ];

    public LogicCommandWithNoFields(int type, int executeSubTick = -1, LogicCommandData? debugData0 = null, LogicCommandData? debugData1 = null)
        : base(executeSubTick, debugData0, debugData1)
    {
        Type = type;
    }

    public override int Type { get; }

    internal static LogicCommandWithNoFields Decode(int type, SupercellStream stream, LogicEnvironment environment)
    {
        var fields = DecodeLogicCommand(stream, environment);
        return new LogicCommandWithNoFields(type, fields.ExecuteSubTick, fields.DebugData0, fields.DebugData1);
    }

    internal override void EncodeBody(SupercellStream stream, LogicEnvironment environment)
    {
        EncodeLogicCommand(stream, environment);
    }
}
