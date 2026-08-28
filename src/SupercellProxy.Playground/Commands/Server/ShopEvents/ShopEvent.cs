using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>One shop event in server command 355.</para>
/// </summary>
internal sealed record ShopEvent
{
    /// <summary>
    /// Gets or sets the <c language="csharp">BinaryData</c> value.
    /// </summary>
    public Memory<byte>? BinaryData { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TextData</c> value.
    /// </summary>
    public string TextData { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the <c language="csharp">EventId</c> value.
    /// </summary>
    public int EventId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString0</c> value.
    /// </summary>
    public string UnknownString0 { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the <c language="csharp">EventType</c> value.
    /// </summary>
    public int EventType { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; init; }

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
    /// Gets or sets the <c language="csharp">Unknown10</c> value.
    /// </summary>
    public int Unknown10 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString1</c> value.
    /// </summary>
    public string UnknownString1 { get; init; } = string.Empty;

    internal static ShopEvent Decode(MessageStream stream)
    {
        var usesBinaryData = stream.ReadBoolean();
        var binaryData = usesBinaryData ? stream.ReadByteArray() : null;
        var textData = usesBinaryData ? string.Empty : stream.ReadString();

        return new ShopEvent
        {
            BinaryData = binaryData,
            TextData = textData,
            EventId = stream.ReadVarInt(),
            Unknown0 = stream.ReadVarInt(),
            UnknownString0 = stream.ReadString(),
            EventType = stream.ReadVarInt(),
            Unknown1 = stream.ReadVarInt(),
            Unknown2 = stream.ReadVarInt(),
            Unknown3 = stream.ReadVarInt(),
            Unknown4 = stream.ReadVarInt(),
            Unknown5 = stream.ReadVarInt(),
            Unknown6 = stream.ReadVarInt(),
            Unknown7 = stream.ReadVarInt(),
            Unknown8 = stream.ReadVarInt(),
            Unknown9 = stream.ReadVarInt(),
            Unknown10 = stream.ReadVarInt(),
            UnknownString1 = stream.ReadString(),
        };
    }

    internal void Encode(MessageStream stream)
    {
        stream.WriteBoolean(BinaryData is not null);

        if (BinaryData is not null)
            stream.WriteByteArray(BinaryData.Value.Span);
        else
            stream.WriteString(TextData);

        stream.WriteVarInt(EventId);
        stream.WriteVarInt(Unknown0);
        stream.WriteString(UnknownString0);
        stream.WriteVarInt(EventType);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);
        stream.WriteVarInt(Unknown5);
        stream.WriteVarInt(Unknown6);
        stream.WriteVarInt(Unknown7);
        stream.WriteVarInt(Unknown8);
        stream.WriteVarInt(Unknown9);
        stream.WriteVarInt(Unknown10);
        stream.WriteString(UnknownString1);
    }
}
