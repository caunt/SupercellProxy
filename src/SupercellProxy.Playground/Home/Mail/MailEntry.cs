using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents <c language="csharp">MailEntry</c>.
/// </summary>
internal sealed record MailEntry
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown2</c> value.
    /// </summary>
    public long Unknown2 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">SenderAvatarName</c> value.
    /// </summary>
    public string? SenderAvatarName { get; init; }

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
    /// Gets or sets the <c language="csharp">Subject</c> value.
    /// </summary>
    public string? Subject { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Body</c> value.
    /// </summary>
    public string? Body { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown8</c> value.
    /// </summary>
    public int Unknown8 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FacebookId</c> value.
    /// </summary>
    public string? FacebookId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GameCenterId</c> value.
    /// </summary>
    public string? GameCenterId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown9</c> value.
    /// </summary>
    public int Unknown9 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown10</c> value.
    /// </summary>
    public int Unknown10 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown11</c> value.
    /// </summary>
    public int Unknown11 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown12</c> value.
    /// </summary>
    public int Unknown12 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown13</c> value.
    /// </summary>
    public int Unknown13 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown14</c> value.
    /// </summary>
    public int Unknown14 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CustomSubject</c> value.
    /// </summary>
    public string? CustomSubject { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CustomBody</c> value.
    /// </summary>
    public string? CustomBody { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown15</c> value.
    /// </summary>
    public int Unknown15 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown16</c> value.
    /// </summary>
    public int Unknown16 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString0</c> value.
    /// </summary>
    public string? UnknownString0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString1</c> value.
    /// </summary>
    public string? UnknownString1 { get; init; }

    internal static MailEntry Decode(MessageStream stream) =>
        new()
        {
            Unknown0 = stream.ReadVarInt(),
            Unknown1 = stream.ReadVarInt(),
            Unknown2 = stream.ReadInt64(),
            SenderAvatarName = stream.ReadOptionalString(),
            Unknown3 = stream.ReadVarInt(),
            Unknown4 = stream.ReadVarInt(),
            Unknown5 = stream.ReadVarInt(),
            Unknown6 = stream.ReadVarInt(),
            Unknown7 = stream.ReadVarInt(),
            Subject = stream.ReadOptionalString(),
            Body = stream.ReadOptionalString(),
            Unknown8 = stream.ReadVarInt(),
            FacebookId = stream.ReadOptionalString(),
            GameCenterId = stream.ReadOptionalString(),
            Unknown9 = stream.ReadVarInt(),
            Unknown10 = stream.ReadVarInt(),
            Unknown11 = stream.ReadVarInt(),
            Unknown12 = stream.ReadVarInt(),
            Unknown13 = stream.ReadVarInt(),
            Unknown14 = stream.ReadVarInt(),
            CustomSubject = stream.ReadOptionalString(),
            CustomBody = stream.ReadOptionalString(),
            Unknown15 = stream.ReadVarInt(),
            Unknown16 = stream.ReadVarInt(),
            UnknownString0 = stream.ReadOptionalString(),
            UnknownString1 = stream.ReadOptionalString(),
        };

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteInt64(Unknown2);
        stream.WriteOptionalString(SenderAvatarName);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);
        stream.WriteVarInt(Unknown5);
        stream.WriteVarInt(Unknown6);
        stream.WriteVarInt(Unknown7);
        stream.WriteOptionalString(Subject);
        stream.WriteOptionalString(Body);
        stream.WriteVarInt(Unknown8);
        stream.WriteOptionalString(FacebookId);
        stream.WriteOptionalString(GameCenterId);
        stream.WriteVarInt(Unknown9);
        stream.WriteVarInt(Unknown10);
        stream.WriteVarInt(Unknown11);
        stream.WriteVarInt(Unknown12);
        stream.WriteVarInt(Unknown13);
        stream.WriteVarInt(Unknown14);
        stream.WriteOptionalString(CustomSubject);
        stream.WriteOptionalString(CustomBody);
        stream.WriteVarInt(Unknown15);
        stream.WriteVarInt(Unknown16);
        stream.WriteOptionalString(UnknownString0);
        stream.WriteOptionalString(UnknownString1);
    }
}
