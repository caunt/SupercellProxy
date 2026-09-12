using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents <c language="csharp">AvatarCollectionSection</c>.
/// </summary>
public sealed record AvatarCollectionSection
{

    /// <summary>
    /// Gets or sets the <c language="csharp">FixedValues</c> value.
    /// </summary>
    public KeyValuePair<int, int>[] FixedValues { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Optional</c> value.
    /// </summary>
    public AvatarOptionalCollection? Optional { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Pairs</c> value.
    /// </summary>
    public KeyValuePair<int, int>[] Pairs { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownValues0</c> value.
    /// </summary>
    public int[] UnknownValues0 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownValues1</c> value.
    /// </summary>
    public int[] UnknownValues1 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Strings</c> value.
    /// </summary>
    public KeyValuePair<int, string?>[] Strings { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries0</c> value.
    /// </summary>
    public AvatarCollectionEntry[] UnknownEntries0 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownEntries1</c> value.
    /// </summary>
    public AvatarCollectionEntry[] UnknownEntries1 { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Triples</c> value.
    /// </summary>
    public (int Unknown0, int Unknown1, int Unknown2)[] Triples { get; init; } = [];
    /// <summary>
    /// Gets or sets the <c language="csharp">Version</c> value.
    /// </summary>
    public int Version { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarCollectionSection Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int version = stream.ReadVariableInt();
        AvatarOptionalCollection? optional = stream.ReadBoolean() ? AvatarOptionalCollection.Decode(stream) : null;

        return new AvatarCollectionSection
        {
            Version = version,
            Optional = optional,
            FixedValues = stream.ReadArray(static valueStream => new KeyValuePair<int, int>(valueStream.ReadVariableInt(), valueStream.ReadInt32())),
            Pairs = stream.ReadArray(static valueStream => new KeyValuePair<int, int>(valueStream.ReadVariableInt(), valueStream.ReadVariableInt())),
            UnknownValues0 = stream.ReadArray(static valueStream => valueStream.ReadVariableInt()),
            UnknownValues1 = stream.ReadArray(static valueStream => valueStream.ReadVariableInt()),
            Strings = stream.ReadArray(static valueStream => new KeyValuePair<int, string?>(valueStream.ReadVariableInt(), valueStream.ReadOptionalString())),
            UnknownEntries0 = stream.ReadArray(AvatarCollectionEntry.Decode),
            UnknownEntries1 = stream.ReadArray(AvatarCollectionEntry.Decode),
            Triples = stream.ReadArray(static valueStream => (valueStream.ReadVariableInt(), valueStream.ReadVariableInt(), valueStream.ReadVariableInt())),
        };
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Version);
        stream.WriteBoolean(Optional is not null);
        Optional?.Encode(stream);
        stream.WriteArray(FixedValues, static (valueStream, value) => { valueStream.WriteVariableInt(value.Key); valueStream.WriteInt32(value.Value); });
        stream.WriteArray(Pairs, static (valueStream, value) => { valueStream.WriteVariableInt(value.Key); valueStream.WriteVariableInt(value.Value); });
        stream.WriteArray(UnknownValues0, static (valueStream, value) => valueStream.WriteVariableInt(value));
        stream.WriteArray(UnknownValues1, static (valueStream, value) => valueStream.WriteVariableInt(value));
        stream.WriteArray(
            Strings,
            static (valueStream, value) =>
            {
                valueStream.WriteVariableInt(value.Key);
                valueStream.WriteOptionalString(value.Value);
            }
        );
        stream.WriteArray(UnknownEntries0, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteArray(UnknownEntries1, static (valueStream, value) => value.Encode(valueStream));
        stream.WriteArray(
            Triples,
            static (valueStream, value) =>
            {
                valueStream.WriteVariableInt(value.Unknown0);
                valueStream.WriteVariableInt(value.Unknown1);
                valueStream.WriteVariableInt(value.Unknown2);
            }
        );
    }
}
