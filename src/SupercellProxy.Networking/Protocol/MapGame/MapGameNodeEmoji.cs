using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>Node emoji, including its expiry timer and collector identities.</para>
/// </summary>
public sealed record MapGameNodeEmoji : ICodecProvider<MapGameNodeEmoji>
{
    /// <summary>
    /// Initializes a new <see cref="MapGameNodeEmoji"/> instance.
    /// </summary>
    public MapGameNodeEmoji(
        int emojiGlobalId,
        int nodeId,
        LongId? avatarId,
        int expirationTime,
        int ticksRemaining,
        ReadOnlyMemory<LongId> collectorAvatarIds
    )
    {
        EmojiGlobalId = emojiGlobalId;
        NodeId = nodeId;
        AvatarId = avatarId;
        ExpirationTime = expirationTime;
        TicksRemaining = ticksRemaining;
        CollectorAvatarIds = collectorAvatarIds.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGameNodeEmoji> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.EmojiGlobalId,
        Codecs.VarInt,
        static value => value.NodeId,
        Codecs.OptionalLongId,
        static value => value.AvatarId,
        Codecs.VarInt,
        static value => value.ExpirationTime,
        Codecs.VarInt,
        static value => value.TicksRemaining,
        Codecs.Memory(Codecs.LongId),
        static value => value.CollectorAvatarIds,
        static (emojiGlobalId, nodeId, avatarId, expirationTime, ticksRemaining, collectorAvatarIds) => new MapGameNodeEmoji(emojiGlobalId, nodeId, avatarId, expirationTime, ticksRemaining, collectorAvatarIds)
    );

    /// <summary>
    /// Gets the <c language="csharp">AvatarId</c> value.
    /// </summary>
    public LongId? AvatarId { get; }

    /// <summary>
    /// Gets the <c language="csharp">CollectorAvatarIds</c> value.
    /// </summary>
    public ReadOnlyMemory<LongId> CollectorAvatarIds { get; }

    /// <summary>
    /// Gets the <c language="csharp">EmojiGlobalId</c> value.
    /// </summary>
    public int EmojiGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExpirationTime</c> value.
    /// </summary>
    public int ExpirationTime { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">NodeId</c> value.
    /// </summary>
    public int NodeId { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">TicksRemaining</c> value.
    /// </summary>
    public int TicksRemaining { get; init; }
}
