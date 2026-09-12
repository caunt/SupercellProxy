using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// <para>Native value and task collection embedded in logic command 321.</para>
/// </summary>
public sealed record MapGameTaskCollection
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
    /// Gets the <c language="csharp">Tasks</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameTaskCollection Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int unknown0 = stream.ReadVariableInt();
        int taskCount = MapGameFieldCodec.ReadCount(stream, name: "task");
        MapGameTask[] tasks = new MapGameTask[taskCount];

        for (int index = 0; index < tasks.Length; index++)
            tasks[index] = MapGameTask.Decode(stream, dataResolver);

        return new MapGameTaskCollection(unknown0, tasks);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Tasks.Length);

        foreach (MapGameTask task in Tasks.Span)
            task.Encode(stream);
    }
}
