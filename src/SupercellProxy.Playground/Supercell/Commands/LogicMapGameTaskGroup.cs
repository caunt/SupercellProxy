using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Native variable-long key and task collection inside a map-game state.
/// </summary>
public sealed record LogicMapGameTaskGroup
{
    public LogicMapGameTaskGroup(long unknown0, ReadOnlyMemory<LogicMapGameTask> tasks)
    {
        Unknown0 = unknown0;
        Tasks = tasks.ToArray();
    }

    public long Unknown0 { get; }
    public ReadOnlyMemory<LogicMapGameTask> Tasks { get; }

    internal static LogicMapGameTaskGroup Decode(SupercellStream stream, ILogicCommandDataResolver? dataResolver)
    {
        var unknown0 = stream.ReadVarLong();
        var taskCount = LogicMapGameWire.ReadCount(stream, "task-group task");
        var tasks = new LogicMapGameTask[taskCount];

        for (var i = 0; i < tasks.Length; i++)
            tasks[i] = LogicMapGameTask.Decode(stream, dataResolver);

        return new LogicMapGameTaskGroup(unknown0, tasks);
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarLong(Unknown0);
        stream.WriteVarInt(Tasks.Length);

        foreach (var task in Tasks.Span)
            task.Encode(stream);
    }
}
