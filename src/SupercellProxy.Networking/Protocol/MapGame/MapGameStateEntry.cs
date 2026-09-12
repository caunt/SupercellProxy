using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>Native data-reference entry inside a map-game state.</para>
/// </summary>
public sealed record MapGameStateEntry
{
    /// <summary>
    /// Initializes a new <see cref="MapGameStateEntry"/> instance.
    /// </summary>
    public MapGameStateEntry(
        int unknownGlobalIdentifier,
        int unknown0,
        LongIdentifier? unknownLongIdentifier,
        int unknown1,
        int unknown2,
        ReadOnlyMemory<LongIdentifier> unknownLongIdentifiers
    )
    {
        UnknownGlobalIdentifier = unknownGlobalIdentifier;
        Unknown0 = unknown0;
        UnknownLongIdentifier = unknownLongIdentifier;
        Unknown1 = unknown1;
        Unknown2 = unknown2;
        UnknownLongIdentifiers = unknownLongIdentifiers.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown2</c> value.
    /// </summary>
    public int Unknown2 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId")]
    public int UnknownGlobalIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongId")]
    public LongIdentifier? UnknownLongIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownLongIds</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownLongIds")]
    public ReadOnlyMemory<LongIdentifier> UnknownLongIdentifiers { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameStateEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new MapGameStateEntry(
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
        stream.WriteVariableInt(UnknownGlobalIdentifier);
        stream.WriteVariableInt(Unknown0);
        MapGameFieldCodec.WriteOptionalLongIdentifier(stream, UnknownLongIdentifier);
        stream.WriteVariableInt(Unknown1);
        stream.WriteVariableInt(Unknown2);
        MapGameFieldCodec.WriteLongIdentifiers(stream, UnknownLongIdentifiers.Span);
    }
}
