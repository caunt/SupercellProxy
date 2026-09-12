using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>
/// Represents <c language="csharp">MailEntry</c>.
/// </summary>
public sealed record MailEntry
{

    /// <summary>
    /// Gets or sets the <c language="csharp">Body</c> value.
    /// </summary>
    public string? Body { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CustomBody</c> value.
    /// </summary>
    public string? CustomBody { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CustomSubject</c> value.
    /// </summary>
    public string? CustomSubject { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">FacebookId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("FacebookId")]
    public string? FacebookIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GameCenterId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GameCenterId")]
    public string? GameCenterIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">SenderAvatarName</c> value.
    /// </summary>
    public string? SenderAvatarName { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Subject</c> value.
    /// </summary>
    public string? Subject { get; init; }
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
    /// Gets or sets the <c language="csharp">Unknown15</c> value.
    /// </summary>
    public int Unknown15 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown16</c> value.
    /// </summary>
    public int Unknown16 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown2</c> value.
    /// </summary>
    public long Unknown2 { get; init; }

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
    public string? UnknownString0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString1</c> value.
    /// </summary>
    public string? UnknownString1 { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MailEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new()
        {
            Unknown0 = stream.ReadVariableInt(),
            Unknown1 = stream.ReadVariableInt(),
            Unknown2 = stream.ReadInt64(),
            SenderAvatarName = stream.ReadOptionalString(),
            Unknown3 = stream.ReadVariableInt(),
            Unknown4 = stream.ReadVariableInt(),
            Unknown5 = stream.ReadVariableInt(),
            Unknown6 = stream.ReadVariableInt(),
            Unknown7 = stream.ReadVariableInt(),
            Subject = stream.ReadOptionalString(),
            Body = stream.ReadOptionalString(),
            Unknown8 = stream.ReadVariableInt(),
            FacebookIdentifier = stream.ReadOptionalString(),
            GameCenterIdentifier = stream.ReadOptionalString(),
            Unknown9 = stream.ReadVariableInt(),
            Unknown10 = stream.ReadVariableInt(),
            Unknown11 = stream.ReadVariableInt(),
            Unknown12 = stream.ReadVariableInt(),
            Unknown13 = stream.ReadVariableInt(),
            Unknown14 = stream.ReadVariableInt(),
            CustomSubject = stream.ReadOptionalString(),
            CustomBody = stream.ReadOptionalString(),
            Unknown15 = stream.ReadVariableInt(),
            Unknown16 = stream.ReadVariableInt(),
            UnknownString0 = stream.ReadOptionalString(),
            UnknownString1 = stream.ReadOptionalString(),
        };
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteInt64(Unknown2);
        stream.WriteOptionalString(SenderAvatarName);
        stream.WriteVariableInt(Unknown3);
        stream.WriteVariableInt(Unknown4);
        stream.WriteVariableInt(Unknown5);
        stream.WriteVariableInt(Unknown6);
        stream.WriteVariableInt(Unknown7);
        stream.WriteOptionalString(Subject);
        stream.WriteOptionalString(Body);
        stream.WriteVariableInt(Unknown8);
        stream.WriteOptionalString(FacebookIdentifier);
        stream.WriteOptionalString(GameCenterIdentifier);
        stream.WriteVariableInt(Unknown9);
        stream.WriteVariableInt(Unknown10);
        stream.WriteVariableInt(Unknown11);
        stream.WriteVariableInt(Unknown12);
        stream.WriteVariableInt(Unknown13);
        stream.WriteVariableInt(Unknown14);
        stream.WriteOptionalString(CustomSubject);
        stream.WriteOptionalString(CustomBody);
        stream.WriteVariableInt(Unknown15);
        stream.WriteVariableInt(Unknown16);
        stream.WriteOptionalString(UnknownString0);
        stream.WriteOptionalString(UnknownString1);
    }
}
