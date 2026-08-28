using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Native value and task collection embedded in logic command 321.</para>
/// </summary>
internal sealed record MapGameTaskCollection
{
    /// <summary>
    /// Initializes a new <see cref="MapGameTaskCollection"/> instance.
    /// </summary>
    public MapGameTaskCollection(int unknown0, ReadOnlyMemory<MapGameTask> tasks)
    {
        Unknown0 = unknown0;
        Tasks = tasks.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Tasks</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }

    internal static MapGameTaskCollection Decode(
        MessageStream stream,
        ICommandDataResolver? dataResolver
    )
    {
        var unknown0 = stream.ReadVarInt();
        var taskCount = MapGameWire.ReadCount(stream, "task");
        var tasks = new MapGameTask[taskCount];

        for (var i = 0; i < tasks.Length; i++)
            tasks[i] = MapGameTask.Decode(stream, dataResolver);

        return new MapGameTaskCollection(unknown0, tasks);
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Tasks.Length);

        foreach (var task in Tasks.Span)
            task.Encode(stream);
    }
}
