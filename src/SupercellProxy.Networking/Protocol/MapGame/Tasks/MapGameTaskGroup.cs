using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// <para>Native fixed-width 64-bit key and task collection inside a map-game state.</para>
/// </summary>
public sealed record MapGameTaskGroup
{
    /// <summary>
    /// Initializes a new <see cref="MapGameTaskGroup"/> instance.
    /// </summary>
    public MapGameTaskGroup(long ownerKey, ReadOnlyMemory<MapGameTask> tasks)
    {
        OwnerKey = ownerKey;
        Tasks = tasks.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">OwnerKey</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown0")]
    public long OwnerKey { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">Tasks</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameTaskGroup Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        long ownerKey = stream.ReadInt64();
        int taskCount = MapGameFieldCodec.ReadCount(stream, name: "task-group task");
        MapGameTask[] tasks = new MapGameTask[taskCount];

        for (int index = 0; index < tasks.Length; index++)
            tasks[index] = MapGameTask.Decode(stream, dataResolver);

        return new MapGameTaskGroup(ownerKey, tasks);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteInt64(OwnerKey);
        stream.WriteVariableInt(Tasks.Length);

        foreach (MapGameTask task in Tasks.Span)
            task.Encode(stream);
    }
}
