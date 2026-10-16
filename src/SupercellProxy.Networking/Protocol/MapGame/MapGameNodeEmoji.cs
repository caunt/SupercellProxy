using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>An emoji a player left on a Valley board node, with its expiry countdown and the players who collected it.</summary>
public sealed record MapGameNodeEmoji : ICodecProvider<MapGameNodeEmoji>
{
    /// <summary>Creates an emoji from its decoded fields.</summary>
    /// <param name="emojiGlobalId">Global id of the emoji's table entry.</param>
    /// <param name="nodeId">Board node the emoji sits on.</param>
    /// <param name="avatarId">Account of the player who left it; null when not sent.</param>
    /// <param name="expirationTime">Expiry setting; -1 for an emoji that never expires.</param>
    /// <param name="ticksRemaining">Updates left until the emoji disappears.</param>
    /// <param name="collectorAvatarIds">Accounts of the players who already collected it.</param>
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

    /// <summary>Account of the player who left the emoji; null when not sent.</summary>
    public LongId? AvatarId { get; }

    /// <summary>Accounts of the players who already collected the emoji; each player collects it once.</summary>
    public ReadOnlyMemory<LongId> CollectorAvatarIds { get; }

    /// <summary>Global id of the emoji's table entry.</summary>
    public int EmojiGlobalId { get; }

    /// <summary>Expiry setting of the emoji; -1 for an emoji that never expires.</summary>
    public int ExpirationTime { get; init; }

    /// <summary>Board node the emoji sits on.</summary>
    public int NodeId { get; init; }

    /// <summary>Updates left until the emoji disappears; it counts down only when the emoji expires.</summary>
    public int TicksRemaining { get; init; }
}
