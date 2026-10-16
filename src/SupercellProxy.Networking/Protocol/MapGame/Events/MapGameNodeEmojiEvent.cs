using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Events;

/// <summary>Event types 23 and 24: an emoji was removed from or collected on a node.</summary>
/// <param name="Type">Native event number: 23 when the emoji was removed, 24 when it was collected.</param>
/// <param name="AvatarId">The emoji owner.</param>
/// <param name="NodeId">Valley board location the emoji was on.</param>
/// <param name="EmojiGlobalId">The emoji data global id.</param>
public sealed record MapGameNodeEmojiEvent(int Type, LongId? AvatarId, int NodeId, int EmojiGlobalId) : MapGameEvent(Type)
{
    /// <summary>
    /// Gets the codec for the event type <paramref name="type"/>.
    /// </summary>
    internal static ICodec<MapGameNodeEmojiEvent> CodecFor(int type)
    {
        return Codecs.Composite(
            Codecs.OptionalLongId,
            static mapGameEvent => mapGameEvent.AvatarId,
            Codecs.VarInt,
            static mapGameEvent => mapGameEvent.NodeId,
            Codecs.VarInt,
            static mapGameEvent => mapGameEvent.EmojiGlobalId,
            (avatarId, nodeId, emojiGlobalId) => new MapGameNodeEmojiEvent(type, avatarId, nodeId, emojiGlobalId)
        );
    }
}
