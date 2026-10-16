using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>Event type 22: an emoji was placed on a node.</summary>
/// <param name="AvatarId">The avatar that placed the emoji.</param>
/// <param name="NodeId">Valley board location the emoji was placed on.</param>
/// <param name="EmojiGlobalId">The emoji data global id.</param>
/// <param name="Duration">The emoji lifetime native reads as a full 32-bit value.</param>
public sealed record MapGameNodeEmojiAddedEvent(LongId? AvatarId, int NodeId, int EmojiGlobalId, int Duration) : MapGameEvent(MapGameEvent.NodeEmojiAddedType), ICodecProvider<MapGameNodeEmojiAddedEvent>
{
    /// <inheritdoc />
    public static new ICodec<MapGameNodeEmojiAddedEvent> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static mapGameEvent => mapGameEvent.AvatarId,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.NodeId,
        Codecs.VarInt,
        static mapGameEvent => mapGameEvent.EmojiGlobalId,
        Codecs.Int32,
        static mapGameEvent => mapGameEvent.Duration,
        static (avatarId, nodeId, emojiGlobalId, duration) => new MapGameNodeEmojiAddedEvent(avatarId, nodeId, emojiGlobalId, duration)
    );
}
