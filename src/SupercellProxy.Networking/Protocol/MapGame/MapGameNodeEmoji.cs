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
        int emojiGlobalIdentifier,
        int nodeIdentifier,
        LongIdentifier? avatarIdentifier,
        int expirationTime,
        int ticksRemaining,
        ReadOnlyMemory<LongIdentifier> collectorAvatarIdentifiers
    )
    {
        EmojiGlobalIdentifier = emojiGlobalIdentifier;
        NodeIdentifier = nodeIdentifier;
        AvatarIdentifier = avatarIdentifier;
        ExpirationTime = expirationTime;
        TicksRemaining = ticksRemaining;
        CollectorAvatarIdentifiers = collectorAvatarIdentifiers.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId")]
    public LongIdentifier? AvatarIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongIds</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongIds")]
    public ReadOnlyMemory<LongIdentifier> CollectorAvatarIdentifiers { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId")]
    public int EmojiGlobalIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">ExpirationTime</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown1")]
    public int ExpirationTime { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">NodeIdentifier</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown0")]
    public int NodeIdentifier { get; init; }

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
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            MapGameFieldCodec.ReadOptionalLongIdentifier(stream),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            MapGameFieldCodec.ReadLongIdentifiers(stream)
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(EmojiGlobalIdentifier);
        stream.WriteVariableInt(NodeIdentifier);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, AvatarIdentifier);
        stream.WriteVariableInt(ExpirationTime);
        stream.WriteVariableInt(TicksRemaining);
        MapGameFieldCodec.WriteLongIdentifiers(stream, CollectorAvatarIdentifiers.Span);
    }
}
