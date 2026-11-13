using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>An emoji a player left on a Valley board node, with its expiry countdown and the players who collected it.</summary>
public sealed record MapGameNodeEmoji : ICodecProvider<MapGameNodeEmoji>
{
    /// <summary>Initializes a new instance of the <see cref="MapGameNodeEmoji"/> record. Creates an emoji from its decoded fields.</summary>
    /// <param name="emojiGlobalId">Global id of the emoji's table entry.</param>
    /// <param name="nodeId">Board node the emoji sits on.</param>
    /// <param name="avatarId">Account of the player who left it; <see langword="null"/> when not sent.</param>
    /// <param name="expirationTime">Expiry setting; -1 for an emoji that never expires.</param>
    /// <param name="ticksRemaining">Updates left until the emoji disappears.</param>
    /// <param name="collectorAvatarIds">Accounts of the players who already collected it.</param>
    public MapGameNodeEmoji(int emojiGlobalId,
        int nodeId,
        LongId? avatarId,
        int expirationTime,
        int ticksRemaining,
        ReadOnlyMemory<LongId> collectorAvatarIds)
    {
        // What the emoji is, where it sits and who left it.
        EmojiGlobalId = emojiGlobalId;
        NodeId = nodeId;
        AvatarId = avatarId;

        // How long it stays, plus a private copy of who already collected it.
        ExpirationTime = expirationTime;
        TicksRemaining = ticksRemaining;
        CollectorAvatarIds = collectorAvatarIds.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGameNodeEmoji> Codec { get; } = Codecs.Composite(Codecs.VarInt,
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
        static (emojiGlobalId, nodeId, avatarId, expirationTime, ticksRemaining, collectorAvatarIds) => new MapGameNodeEmoji(emojiGlobalId, nodeId, avatarId, expirationTime, ticksRemaining, collectorAvatarIds));

    /// <summary>Gets the account of the player who left the emoji; <see langword="null"/> when not sent.</summary>
    /// <value>The placing player's account id, sent after <see cref="NodeId"/>; <see langword="null"/> when the wire marks it absent.</value>
    public LongId? AvatarId { get; }

    /// <summary>Gets the accounts of the players who already collected the emoji.</summary>
    /// <value>Account ids in the order the players collected the emoji, without repeats. Empty when nobody has collected it.</value>
    public ReadOnlyMemory<LongId> CollectorAvatarIds { get; }

    /// <summary>Gets the global id of the emoji's table entry.</summary>
    /// <value>The global id of the emoji's row in the Valley emoji table, which says what the emoji is.</value>
    public int EmojiGlobalId { get; }

    /// <summary>Gets or sets the expiry setting of the emoji; -1 for an emoji that never expires.</summary>
    /// <value>The expiry value sent with the emoji; any value other than -1 marks an emoji that expires.</value>
    public int ExpirationTime { get; init; }

    /// <summary>Gets or sets the board node the emoji sits on.</summary>
    /// <value>The id of a node of the season's Valley map, sent right after <see cref="EmojiGlobalId"/>.</value>
    public int NodeId { get; init; }

    /// <summary>Gets or sets the updates left until the emoji disappears.</summary>
    /// <value>A number of board updates.</value>
    public int TicksRemaining { get; init; }
}
