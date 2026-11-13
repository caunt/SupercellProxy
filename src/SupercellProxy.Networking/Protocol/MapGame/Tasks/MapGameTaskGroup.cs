using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>The Valley tasks that belong to one player, or the tasks every player on the board shares.</summary>
public sealed record MapGameTaskGroup : ICodecProvider<MapGameTaskGroup>
{
    /// <summary>Initializes a new instance of the <see cref="MapGameTaskGroup"/> record. Creates a group from its decoded fields.</summary>
    /// <param name="ownerKey">Account id of the player the tasks belong to, or 4294967295 for the board's shared tasks.</param>
    /// <param name="tasks">Tasks in the group.</param>
    public MapGameTaskGroup(long ownerKey, ReadOnlyMemory<MapGameTask> tasks)
    {
        OwnerKey = ownerKey;
        Tasks = tasks.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGameTaskGroup> Codec { get; } = Codecs.Composite(Codecs.BigEndianInt64,
        static group => group.OwnerKey,
        Codecs.Memory(MapGameTask.Codec),
        static group => group.Tasks,
        static (ownerKey, tasks) => new MapGameTaskGroup(ownerKey, tasks));

    /// <summary>Gets or sets the account id of the player the tasks belong to, or 4294967295 for the board's shared tasks.</summary>
    /// <value>A 64-bit account id sent as a fixed eight-byte integer; the value <see cref="uint.MaxValue"/> marks the group every player on the board shares.</value>
    public long OwnerKey { get; init; }

    /// <summary>Gets the tasks in the group.</summary>
    /// <value>The owner's tasks in wire order, read after <see cref="OwnerKey"/>; empty when the owner has none.</value>
    public ReadOnlyMemory<MapGameTask> Tasks { get; }
}
