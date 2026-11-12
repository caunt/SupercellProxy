using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>A number followed by Valley tasks, as carried by command 321.</summary>
public sealed record MapGameTaskCollection : ICodecProvider<MapGameTaskCollection>
{
    /// <summary>Initializes a new instance of the <see cref="MapGameTaskCollection"/> record. Creates a collection from its decoded fields.</summary>
    /// <param name="unknown0">Leading number; its meaning is not identified.</param>
    /// <param name="tasks">Tasks in the collection.</param>
    public MapGameTaskCollection(int unknown0, ReadOnlyMemory<MapGameTask> tasks)
    {
        Unknown0 = unknown0;
        Tasks = tasks.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGameTaskCollection> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static collection => collection.Unknown0,
        Codecs.Memory(MapGameTask.Codec),
        static collection => collection.Tasks,
        static (unknown0, tasks) => new MapGameTaskCollection(unknown0, tasks));

    /// <summary>Gets the tasks in the collection.</summary>
    /// <value>The tasks in wire order, read after <see cref="Unknown0"/>; empty when the collection carries none.</value>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }

    /// <summary>Gets the leading number of the collection; its meaning is not identified.</summary>
    /// <value>A variable-length integer at wire position 0, kept so the collection re-encodes unchanged.</value>
    public int Unknown0 { get; }
}
