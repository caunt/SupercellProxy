using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Native variable-long key and task collection inside a map-game state.</para>
/// </summary>
public sealed record MapGameTaskGroup
{
    /// <summary>
    /// Initializes a new <see cref="MapGameTaskGroup"/> instance.
    /// </summary>
    public MapGameTaskGroup(long unknown0, ReadOnlyMemory<MapGameTask> tasks)
    {
        Unknown0 = unknown0;
        Tasks = tasks.ToArray();
    }

    /// <summary>
    /// Gets the <c>Unknown0</c> value.
    /// </summary>
    public long Unknown0 { get; }

    /// <summary>
    /// Gets the <c>Tasks</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }

    internal static MapGameTaskGroup Decode(
        MessageStream stream,
        ICommandDataResolver? dataResolver
    )
    {
        var unknown0 = stream.ReadVarLong();
        var taskCount = MapGameWire.ReadCount(stream, "task-group task");
        var tasks = new MapGameTask[taskCount];

        for (var i = 0; i < tasks.Length; i++)
            tasks[i] = MapGameTask.Decode(stream, dataResolver);

        return new MapGameTaskGroup(unknown0, tasks);
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarLong(Unknown0);
        stream.WriteVarInt(Tasks.Length);

        foreach (var task in Tasks.Span)
            task.Encode(stream);
    }
}
