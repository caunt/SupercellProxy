using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>
/// <para>One shop event in server command 355.</para>
/// </summary>
public sealed record ShopEvent
{
    /// <summary>
    /// Gets or sets the <c language="csharp">BinaryData</c> value.
    /// </summary>
    public Memory<byte>? BinaryData { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EventId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("EventId")]
    public int EventIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EventType</c> value.
    /// </summary>
    public int EventType { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TextData</c> value.
    /// </summary>
    public string TextData { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown10</c> value.
    /// </summary>
    public int Unknown10 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown2</c> value.
    /// </summary>
    public int Unknown2 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown3</c> value.
    /// </summary>
    public int Unknown3 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown4</c> value.
    /// </summary>
    public int Unknown4 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown5</c> value.
    /// </summary>
    public int Unknown5 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown6</c> value.
    /// </summary>
    public int Unknown6 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown7</c> value.
    /// </summary>
    public int Unknown7 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown8</c> value.
    /// </summary>
    public int Unknown8 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown9</c> value.
    /// </summary>
    public int Unknown9 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString0</c> value.
    /// </summary>
    public string UnknownString0 { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString1</c> value.
    /// </summary>
    public string UnknownString1 { get; init; } = string.Empty;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static ShopEvent Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        bool usesBinaryData = stream.ReadBoolean();
        Memory<byte> binaryData = usesBinaryData ? stream.ReadByteArray() : null;
        string textData = usesBinaryData ? string.Empty : stream.ReadString();

        return new ShopEvent
        {
            BinaryData = binaryData,
            TextData = textData,
            EventIdentifier = stream.ReadVariableInt(),
            Unknown0 = stream.ReadVariableInt(),
            UnknownString0 = stream.ReadString(),
            EventType = stream.ReadVariableInt(),
            Unknown1 = stream.ReadVariableInt(),
            Unknown2 = stream.ReadVariableInt(),
            Unknown3 = stream.ReadVariableInt(),
            Unknown4 = stream.ReadVariableInt(),
            Unknown5 = stream.ReadVariableInt(),
            Unknown6 = stream.ReadVariableInt(),
            Unknown7 = stream.ReadVariableInt(),
            Unknown8 = stream.ReadVariableInt(),
            Unknown9 = stream.ReadVariableInt(),
            Unknown10 = stream.ReadVariableInt(),
            UnknownString1 = stream.ReadString(),
        };
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteBoolean(BinaryData is not null);

        if (BinaryData is not null)
            stream.WriteByteArray(BinaryData.Value.Span);
        else
            stream.WriteString(TextData);

        stream.WriteVariableInt(EventIdentifier);
        stream.WriteVariableInt(Unknown0);
        stream.WriteString(UnknownString0);
        stream.WriteVariableInt(EventType);
        stream.WriteVariableInt(Unknown1);
        stream.WriteVariableInt(Unknown2);
        stream.WriteVariableInt(Unknown3);
        stream.WriteVariableInt(Unknown4);
        stream.WriteVariableInt(Unknown5);
        stream.WriteVariableInt(Unknown6);
        stream.WriteVariableInt(Unknown7);
        stream.WriteVariableInt(Unknown8);
        stream.WriteVariableInt(Unknown9);
        stream.WriteVariableInt(Unknown10);
        stream.WriteString(UnknownString1);
    }
}
