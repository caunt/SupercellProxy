using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>
/// Represents <c language="csharp">MailEntry</c>.
/// </summary>
public sealed record MailEntry : ICodecProvider<MailEntry>
{
    internal const int MinimumEncodedSize = 60;

    /// <inheritdoc />
    public static ICodec<MailEntry> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static entry => entry.RewardState,
        Codecs.VarInt,
        static entry => entry.Category,
        Codecs.Int64,
        static entry => entry.Id,
        Sender.Codec,
        static entry => new Sender(entry.SenderAvatarName, entry.SenderAvatarIdHigh, entry.SenderAvatarIdLow),
        Recipient.Codec,
        static entry => new Recipient(entry.RecipientAvatarIdHigh, entry.RecipientAvatarIdLow),
        Codecs.VarInt,
        static entry => entry.Unknown7,
        Text.Codec,
        static entry => new Text(entry.Subject, entry.Body),
        Codecs.VarInt,
        static entry => entry.Unknown8,
        LinkedAccounts.Codec,
        static entry => new LinkedAccounts(entry.FacebookId, entry.GameCenterId),
        UnknownValues.Codec,
        static entry => new UnknownValues(entry.Unknown9, entry.Unknown10, entry.Unknown11, entry.Unknown12, entry.Unknown13, entry.Unknown14),
        CustomText.Codec,
        static entry => new CustomText(entry.CustomSubject, entry.CustomBody),
        Reward.Codec,
        static entry => new Reward(entry.RewardGlobalId, entry.RewardCount),
        Codecs.OptionalString,
        static entry => entry.UnknownString0,
        Codecs.OptionalString,
        static entry => entry.UnknownString1,
        static (
            rewardState,
            category,
            id,
            sender,
            recipient,
            unknown7,
            text,
            unknown8,
            linkedAccounts,
            unknownValues,
            customText,
            reward,
            unknownString0,
            unknownString1
        ) => new MailEntry
        {
            RewardState = rewardState,
            Category = category,
            Id = id,
            SenderAvatarName = sender.SenderAvatarName,
            SenderAvatarIdHigh = sender.SenderAvatarIdHigh,
            SenderAvatarIdLow = sender.SenderAvatarIdLow,
            RecipientAvatarIdHigh = recipient.RecipientAvatarIdHigh,
            RecipientAvatarIdLow = recipient.RecipientAvatarIdLow,
            Unknown7 = unknown7,
            Subject = text.Subject,
            Body = text.Body,
            Unknown8 = unknown8,
            FacebookId = linkedAccounts.FacebookId,
            GameCenterId = linkedAccounts.GameCenterId,
            Unknown9 = unknownValues.Unknown9,
            Unknown10 = unknownValues.Unknown10,
            Unknown11 = unknownValues.Unknown11,
            Unknown12 = unknownValues.Unknown12,
            Unknown13 = unknownValues.Unknown13,
            Unknown14 = unknownValues.Unknown14,
            CustomSubject = customText.CustomSubject,
            CustomBody = customText.CustomBody,
            RewardGlobalId = reward.RewardGlobalId,
            RewardCount = reward.RewardCount,
            UnknownString0 = unknownString0,
            UnknownString1 = unknownString1,
        }
    );

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
    /// Gets the high word of the avatar id the letter was delivered to.
    /// </summary>
    public int RecipientAvatarIdHigh { get; init; }

    /// <summary>
    /// Gets the low word of the avatar id the letter was delivered to.
    /// </summary>
    public int RecipientAvatarIdLow { get; init; }

    /// <summary>
    /// Gets the amount of the mass-gift reward.
    /// </summary>
    public int RewardCount { get; init; }

    /// <summary>
    /// Gets the reward a mass-gift letter grants, such as a puzzle piece or decoration.
    /// </summary>
    public int RewardGlobalId { get; init; }

    /// <summary>
    /// Gets the letter's collection and reward state.
    /// </summary>
    public int RewardState { get; init; }

    /// <summary>
    /// Gets the high word of the sender avatar id; native letters show this avatar's portrait and name.
    /// </summary>
    public int SenderAvatarIdHigh { get; init; }

    /// <summary>
    /// Gets the low word of the sender avatar id.
    /// </summary>
    public int SenderAvatarIdLow { get; init; }

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

    // The custom subject and body.
    private readonly record struct CustomText(string? CustomSubject, string? CustomBody)
    {
        public static ICodec<CustomText> Codec { get; } = Codecs.Composite(
            Codecs.OptionalString,
            static segment => segment.CustomSubject,
            Codecs.OptionalString,
            static segment => segment.CustomBody,
            static (customSubject, customBody) => new CustomText(customSubject, customBody)
        );
    }

    // The sender linked account ids.
    private readonly record struct LinkedAccounts(string? FacebookId, string? GameCenterId)
    {
        public static ICodec<LinkedAccounts> Codec { get; } = Codecs.Composite(
            Codecs.OptionalString,
            static segment => segment.FacebookId,
            Codecs.OptionalString,
            static segment => segment.GameCenterId,
            static (facebookId, gameCenterId) => new LinkedAccounts(facebookId, gameCenterId)
        );
    }

    // The recipient avatar id.
    private readonly record struct Recipient(int RecipientAvatarIdHigh, int RecipientAvatarIdLow)
    {
        public static ICodec<Recipient> Codec { get; } = Codecs.Composite(
            Codecs.VarInt,
            static segment => segment.RecipientAvatarIdHigh,
            Codecs.VarInt,
            static segment => segment.RecipientAvatarIdLow,
            static (recipientAvatarIdHigh, recipientAvatarIdLow) => new Recipient(recipientAvatarIdHigh, recipientAvatarIdLow)
        );
    }

    // The attached reward.
    private readonly record struct Reward(int RewardGlobalId, int RewardCount)
    {
        public static ICodec<Reward> Codec { get; } = Codecs.Composite(
            Codecs.VarInt,
            static segment => segment.RewardGlobalId,
            Codecs.VarInt,
            static segment => segment.RewardCount,
            static (rewardGlobalId, rewardCount) => new Reward(rewardGlobalId, rewardCount)
        );
    }

    // The sender name and avatar id.
    private readonly record struct Sender(string? SenderAvatarName, int SenderAvatarIdHigh, int SenderAvatarIdLow)
    {
        public static ICodec<Sender> Codec { get; } = Codecs.Composite(
            Codecs.OptionalString,
            static segment => segment.SenderAvatarName,
            Codecs.VarInt,
            static segment => segment.SenderAvatarIdHigh,
            Codecs.VarInt,
            static segment => segment.SenderAvatarIdLow,
            static (senderAvatarName, senderAvatarIdHigh, senderAvatarIdLow) => new Sender(senderAvatarName, senderAvatarIdHigh, senderAvatarIdLow)
        );
    }

    // The letter subject and body.
    private readonly record struct Text(string? Subject, string? Body)
    {
        public static ICodec<Text> Codec { get; } = Codecs.Composite(
            Codecs.OptionalString,
            static segment => segment.Subject,
            Codecs.OptionalString,
            static segment => segment.Body,
            static (subject, body) => new Text(subject, body)
        );
    }

    // The six unnamed values native reads after the linked ids.
    private readonly record struct UnknownValues(int Unknown9, int Unknown10, int Unknown11, int Unknown12, int Unknown13, int Unknown14)
    {
        public static ICodec<UnknownValues> Codec { get; } = Codecs.Composite(
            Codecs.VarInt,
            static segment => segment.Unknown9,
            Codecs.VarInt,
            static segment => segment.Unknown10,
            Codecs.VarInt,
            static segment => segment.Unknown11,
            Codecs.VarInt,
            static segment => segment.Unknown12,
            Codecs.VarInt,
            static segment => segment.Unknown13,
            Codecs.VarInt,
            static segment => segment.Unknown14,
            static (unknown9, unknown10, unknown11, unknown12, unknown13, unknown14) => new UnknownValues(unknown9, unknown10, unknown11, unknown12, unknown13, unknown14)
        );
    }
}
