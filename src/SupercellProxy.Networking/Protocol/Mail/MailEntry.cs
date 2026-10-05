using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>
/// Represents <c language="csharp">MailEntry</c>.
/// </summary>
public sealed record MailEntry
{
    internal const int MinimumEncodedSize = 60;

    /// <summary>
    /// Gets or sets the <c language="csharp">Body</c> value.
    /// </summary>
    public string? Body { get; init; }

    /// <summary>
    /// Gets the letter category.
    /// </summary>
    public int Category { get; init; }

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
    public string? FacebookId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">GameCenterId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("GameCenterId")]
    public string? GameCenterId { get; init; }

    /// <summary>
    /// Gets the id used to remove the letter from the avatar's mail list.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets the letter's collection and reward state.
    /// </summary>
    public int RewardState { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">SenderAvatarName</c> value.
    /// </summary>
    public string? SenderAvatarName { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Subject</c> value.
    /// </summary>
    public string? Subject { get; init; }

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
            RewardState = stream.ReadVarInt(),
            Category = stream.ReadVarInt(),
            Id = stream.ReadInt64(),
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
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(RewardState);
        stream.WriteVarInt(Category);
        stream.WriteInt64(Id);
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
