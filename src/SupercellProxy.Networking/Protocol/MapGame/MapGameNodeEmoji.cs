using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>Node emoji, including its expiry timer and collector identities.</para>
/// </summary>
public sealed record MapGameNodeEmoji
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

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId")]
    public LongId? AvatarId { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongIds</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongIds")]
    public ReadOnlyMemory<LongId> CollectorAvatarIds { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId")]
    public int EmojiGlobalId { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExpirationTime</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown1")]
    public int ExpirationTime { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">NodeId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown0")]
    public int NodeId { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">TicksRemaining</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown2")]
    public int TicksRemaining { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameNodeEmoji Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameNodeEmoji(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            MapGameFieldCodec.ReadOptionalLongId(stream),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            MapGameFieldCodec.ReadLongIds(stream)
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(EmojiGlobalId);
        stream.WriteVarInt(NodeId);
        MapGameFieldCodec.WriteOptionalLongId(stream, AvatarId);
        stream.WriteVarInt(ExpirationTime);
        stream.WriteVarInt(TicksRemaining);
        MapGameFieldCodec.WriteLongIds(stream, CollectorAvatarIds.Span);
    }
}
