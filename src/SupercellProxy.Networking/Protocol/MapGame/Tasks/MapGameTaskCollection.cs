using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>A number followed by Valley tasks, as carried by command 321.</summary>
public sealed record MapGameTaskCollection : ICodecProvider<MapGameTaskCollection>
{
    /// <summary>Creates a collection from its decoded fields.</summary>
    /// <param name="unknown0">Leading number; its meaning is not identified.</param>
    /// <param name="tasks">Tasks in the collection.</param>
    public MapGameTaskCollection(int unknown0, ReadOnlyMemory<MapGameTask> tasks)
    {
        Unknown0 = unknown0;
        Tasks = tasks.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGameTaskCollection> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static collection => collection.Unknown0,
        Codecs.Memory(MapGameTask.Codec),
        static collection => collection.Tasks,
        static (unknown0, tasks) => new MapGameTaskCollection(unknown0, tasks)
    );

    /// <summary>Tasks in the collection.</summary>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }

    /// <summary>Leading number of the collection; its meaning is not identified.</summary>
    public int Unknown0 { get; }
}
