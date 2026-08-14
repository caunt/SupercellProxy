using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Native value and task collection embedded in logic command 321.
/// </summary>
public sealed record LogicMapGameTaskCollection
{
    public LogicMapGameTaskCollection(int unknown0, ReadOnlyMemory<LogicMapGameTask> tasks)
    {
        Unknown0 = unknown0;
        Tasks = tasks.ToArray();
    }

    public int Unknown0 { get; }
    public ReadOnlyMemory<LogicMapGameTask> Tasks { get; }

    internal static LogicMapGameTaskCollection Decode(SupercellStream stream, ILogicCommandDataResolver? dataResolver)
    {
        var unknown0 = stream.ReadVarInt();
        var taskCount = LogicMapGameWire.ReadCount(stream, "task");
        var tasks = new LogicMapGameTask[taskCount];

        for (var i = 0; i < tasks.Length; i++)
            tasks[i] = LogicMapGameTask.Decode(stream, dataResolver);

        return new LogicMapGameTaskCollection(unknown0, tasks);
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Tasks.Length);

        foreach (var task in Tasks.Span)
            task.Encode(stream);
    }
}
