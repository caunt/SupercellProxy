using System.Text.Json;

using SupercellProxy.Networking.Json;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Represents <c language="csharp">AvatarEncodedCollectionEntry</c>.
/// </summary>
public sealed record AvatarEncodedCollectionEntry
{

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedData</c> value.
    /// </summary>
    public Memory<byte>? CompressedData { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Text</c> value.
    /// </summary>
    public string? Text { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString0</c> value.
    /// </summary>
    public string? UnknownString0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString1</c> value.
    /// </summary>
    public string? UnknownString1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownValues</c> value.
    /// </summary>
    public int[] UnknownValues { get; init; } = [];
    /// <summary>
    /// Gets or sets the <c language="csharp">UsesCompressedData</c> value.
    /// </summary>
    public bool UsesCompressedData { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static AvatarEncodedCollectionEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        bool usesCompressedData = stream.ReadBoolean();

        return new AvatarEncodedCollectionEntry
        {
            UsesCompressedData = usesCompressedData,
            Text = usesCompressedData ? null : stream.ReadOptionalString(),
            CompressedData = usesCompressedData ? stream.ReadOptionalByteArray() : null,
            Unknown0 = stream.ReadVariableInt(),
            Unknown1 = stream.ReadVariableInt(),
            UnknownString0 = stream.ReadOptionalString(),
            UnknownValues = stream.ReadVariableIntArray(count: 11),
            UnknownString1 = stream.ReadOptionalString(),
        };
    }

    /// <summary>Deserializes the retained event document into its concrete configuration contract.</summary>
    public TValue Decode<TValue>() where TValue : class
    {
        return CompressedData is { } bytes
            ? CompressedDocument.Deserialize<TValue>(bytes)
            : JsonSerializer.Deserialize<TValue>(Text ?? "{}")
                ?? throw new InvalidDataException(message: "The event document is null.");
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (UnknownValues.Length is not 11)
            throw new InvalidOperationException(message: "Unexpected manager field count.");

        stream.WriteBoolean(UsesCompressedData);

        if (UsesCompressedData)
            stream.WriteOptionalByteArray(CompressedData);
        else
            stream.WriteOptionalString(Text);

        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteOptionalString(UnknownString0);

        foreach (int value in UnknownValues)
            stream.WriteVariableInt(value);

        stream.WriteOptionalString(UnknownString1);
    }
}
