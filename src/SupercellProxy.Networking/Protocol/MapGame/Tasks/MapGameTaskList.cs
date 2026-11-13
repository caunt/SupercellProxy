using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>A number followed by Valley tasks, as carried by command 321.</summary>
public sealed record MapGameTaskList : ICodecProvider<MapGameTaskList>
{
    /// <summary>Initializes a new instance of the <see cref="MapGameTaskList"/> record. Creates a list from its decoded fields.</summary>
    /// <param name="unknown0">Leading number; its meaning is not identified.</param>
    /// <param name="tasks">Tasks in the list.</param>
    public MapGameTaskList(int unknown0, ReadOnlyMemory<MapGameTask> tasks)
    {
        Unknown0 = unknown0;
        Tasks = tasks.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGameTaskList> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static list => list.Unknown0,
        Codecs.Memory(MapGameTask.Codec),
        static list => list.Tasks,
        static (unknown0, tasks) => new MapGameTaskList(unknown0, tasks));

    /// <summary>Gets the tasks in the list.</summary>
    /// <value>The tasks in wire order, read after <see cref="Unknown0"/>; empty when the list carries none.</value>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }

    /// <summary>Gets the leading number of the list; its meaning is not identified.</summary>
    /// <value>A variable-length integer at wire position 0, kept so the list re-encodes unchanged.</value>
    public int Unknown0 { get; }
}
