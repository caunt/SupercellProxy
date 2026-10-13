using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// <para>Native fixed-width 64-bit key and task collection inside a map-game state.</para>
/// </summary>
public sealed record MapGameTaskGroup : ICodecProvider<MapGameTaskGroup>
{
    /// <summary>
    /// Initializes a new <see cref="MapGameTaskGroup"/> instance.
    /// </summary>
    public MapGameTaskGroup(long ownerKey, ReadOnlyMemory<MapGameTask> tasks)
    {
        OwnerKey = ownerKey;
        Tasks = tasks.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGameTaskGroup> Codec { get; } = Codecs.Composite(
        Codecs.Int64,
        static group => group.OwnerKey,
        Codecs.Memory(MapGameTask.Codec),
        static group => group.Tasks,
        static (ownerKey, tasks) => new MapGameTaskGroup(ownerKey, tasks)
    );

    /// <summary>
    /// Gets the <c language="csharp">OwnerKey</c> value.
    /// </summary>
    public long OwnerKey { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">Tasks</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }
}
