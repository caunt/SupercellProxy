using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// <para>Native value and task collection embedded in logic command 321.</para>
/// </summary>
public sealed record MapGameTaskCollection : ICodecProvider<MapGameTaskCollection>
{
    /// <summary>
    /// Initializes a new <see cref="MapGameTaskCollection"/> instance.
    /// </summary>
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

    /// <summary>
    /// Gets the <c language="csharp">Tasks</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }
}
