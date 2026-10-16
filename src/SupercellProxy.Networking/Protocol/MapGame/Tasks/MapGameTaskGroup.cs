using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>The Valley tasks that belong to one player, or the tasks every player on the board shares.</summary>
public sealed record MapGameTaskGroup : ICodecProvider<MapGameTaskGroup>
{
    /// <summary>Creates a group from its decoded fields.</summary>
    /// <param name="ownerKey">Account id of the player the tasks belong to, or 4294967295 for the board's shared tasks.</param>
    /// <param name="tasks">Tasks in the group.</param>
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

    /// <summary>Account id of the player the tasks belong to, or 4294967295 for the board's shared tasks.</summary>
    public long OwnerKey { get; init; }

    /// <summary>Tasks in the group.</summary>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }
}
