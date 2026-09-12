using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

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
    /// Gets the <c language="csharp">Tasks</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public long Unknown0 { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameTaskGroup Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        long unknown0 = stream.ReadVariableLong();
        int taskCount = MapGameFieldCodec.ReadCount(stream, name: "task-group task");
        MapGameTask[] tasks = new MapGameTask[taskCount];

        for (int index = 0; index < tasks.Length; index++)
            tasks[index] = MapGameTask.Decode(stream, dataResolver);

        return new MapGameTaskGroup(unknown0, tasks);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableLong(Unknown0);
        stream.WriteVariableInt(Tasks.Length);

        foreach (MapGameTask task in Tasks.Span)
            task.Encode(stream);
    }
}
