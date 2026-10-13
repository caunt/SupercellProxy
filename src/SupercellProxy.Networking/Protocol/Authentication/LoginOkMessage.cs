using SupercellProxy.Networking.Protocol.Accounts.Permissions;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;


namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Represents the <c language="csharp">LoginOkMessage</c> protocol message.
/// </summary>
public sealed record LoginOkMessage : CodecMessage<LoginOkMessage>, ICodecProvider<LoginOkMessage>
{
    /// <summary>
    /// Gets the wire codec.
    /// </summary>
    public static ICodec<LoginOkMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.SessionCount,
        Codecs.VarInt,
        static message => message.PlayTimeSeconds,
        Codecs.VarInt,
        static message => message.LoginVersion,
        Codecs.VarInt,
        static message => message.ServerBuild,
        Codecs.Boolean,
        static message => message.Unknown1,
        Codecs.LongId,
        static message => message.AccountId,
        Codecs.LongId,
        static message => message.HomeId,
        Codecs.String,
        static message => message.ServerTime,
        Codecs.String,
        static message => message.AccountCreatedDate,
        Codecs.String,
        static message => message.PassToken,
        LinkedAccountIds.Codec,
        static message => new LinkedAccountIds(message.FacebookId, message.GamecenterId, message.GoogleServiceId, message.AmazonId),
        Codecs.String,
        static message => message.LoginCountry,
        Codecs.String,
        static message => message.EventAssetsAddress,
        Codecs.NullableMemory(Codecs.OptionalString),
        static message => message.ContentUrls,
        TrailingBlocks.Codec,
        static message => new TrailingBlocks(
            message.SessionData,
            new Presence<string?>(message.HasTrailingText, message.TrailingText),
            message.OptionalTexts,
            new Presence<PlayerPermissionSet?>(message.HasPermissionsBlock, message.Permissions)
        ),
        static (
            sessionCount,
            playTimeSeconds,
            loginVersion,
            serverBuild,
            unknown1,
            accountId,
            homeId,
            serverTime,
            accountCreatedDate,
            passToken,
            linkedAccountIds,
            loginCountry,
            eventAssetsAddress,
            contentUrls,
            trailingBlocks
        ) => new LoginOkMessage
        {
            SessionCount = sessionCount,
            PlayTimeSeconds = playTimeSeconds,
            LoginVersion = loginVersion,
            ServerBuild = serverBuild,
            Unknown1 = unknown1,
            AccountId = accountId,
            HomeId = homeId,
            ServerTime = serverTime,
            AccountCreatedDate = accountCreatedDate,
            PassToken = passToken,
            FacebookId = linkedAccountIds.FacebookId,
            GamecenterId = linkedAccountIds.GamecenterId,
            GoogleServiceId = linkedAccountIds.GoogleServiceId,
            AmazonId = linkedAccountIds.AmazonId,
            LoginCountry = loginCountry,
            EventAssetsAddress = eventAssetsAddress,
            ContentUrls = contentUrls,
            SessionData = trailingBlocks.SessionData,
            HasTrailingText = trailingBlocks.Text.IsPresent,
            TrailingText = trailingBlocks.Text.Value,
            OptionalTexts = trailingBlocks.OptionalTexts,
            HasPermissionsBlock = trailingBlocks.Permissions.IsPresent,
            Permissions = trailingBlocks.Permissions.Value,
        }
    );

    /// <summary>
    /// Gets or sets the <c language="csharp">AccountCreatedDate</c> value.
    /// </summary>
    public required string AccountCreatedDate { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AccountId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AccountId")]
    public required LongId AccountId { get; init; }

    /// <summary>Gets the Amazon id ("amazon id").</summary>
    public required string? AmazonId { get; init; }

    /// <summary>Gets the content URLs; native reads a count of zero or less as no list.</summary>
    public required ReadOnlyMemory<string?>? ContentUrls { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EventAssetsUrl</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("EventAssetsUrl")]
    public required string EventAssetsAddress { get; init; }

    /// <summary>Gets the Facebook id ("facebook id").</summary>
    public required string? FacebookId { get; init; }
    /// <summary>Gets the Game Center id ("gamecenter id").</summary>
    public required string? GamecenterId { get; init; }

    /// <summary>Gets the Google Play Games id ("google service id").</summary>
    public required string? GoogleServiceId { get; init; }

    /// <summary>Gets whether the fourth trailing block was sent, since its permission set is itself optional.</summary>
    public bool HasPermissionsBlock { get; init; }

    /// <summary>Gets whether the second trailing block was sent, since its text may itself be null.</summary>
    public bool HasTrailingText { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HomeId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("HomeId")]
    public required LongId HomeId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LoginCountry</c> value.
    /// </summary>
    public required string LoginCountry { get; init; }


    /// <summary>
    /// Gets or sets the <c language="csharp">LoginVersion</c> value.
    /// </summary>
    public required int LoginVersion { get; init; }

    /// <summary>Gets the third trailing block, present only when the server sent more bytes.</summary>
    public LoginOkOptionalTexts? OptionalTexts { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PassToken</c> value.
    /// </summary>
    public required string PassToken { get; init; }

    /// <summary>Gets the fourth trailing block's permission set, when sent.</summary>
    public PlayerPermissionSet? Permissions { get; init; }

    /// <summary>
    /// Gets the account's total online play time in seconds; between captured logins it never grows faster than the wall clock.
    /// </summary>
    public required int PlayTimeSeconds { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ServerBuild</c> value.
    /// </summary>
    public required int ServerBuild { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ServerTime</c> value.
    /// </summary>
    public required string ServerTime { get; init; }

    /// <summary>
    /// Gets the account's login count, which native logs as <c>sessionCount</c>; it grows by exactly one per captured login.
    /// </summary>
    public required int SessionCount { get; init; }

    /// <summary>Gets the first trailing block, present only when the server sent more bytes.</summary>
    public LoginOkSessionData? SessionData { get; init; }

    /// <summary>Gets the second trailing block's text, present only when the server sent more bytes.</summary>
    public string? TrailingText { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public required bool Unknown1 { get; init; }

    // The linked third-party account ids, read as one group to keep the field list within a composite.
    private readonly record struct LinkedAccountIds(string? FacebookId, string? GamecenterId, string? GoogleServiceId, string? AmazonId)
    {
        public static ICodec<LinkedAccountIds> Codec { get; } = Codecs.Composite(
            Codecs.OptionalString,
            static ids => ids.FacebookId,
            Codecs.OptionalString,
            static ids => ids.GamecenterId,
            Codecs.OptionalString,
            static ids => ids.GoogleServiceId,
            Codecs.OptionalString,
            static ids => ids.AmazonId,
            static (facebookId, gamecenterId, googleServiceId, amazonId) => new LinkedAccountIds(facebookId, gamecenterId, googleServiceId, amazonId)
        );
    }

    // Native reads each block only while payload bytes remain, so older servers end the message earlier.
    private readonly record struct TrailingBlocks(
        LoginOkSessionData? SessionData,
        Presence<string?> Text,
        LoginOkOptionalTexts? OptionalTexts,
        Presence<PlayerPermissionSet?> Permissions
    )
    {
        public static ICodec<TrailingBlocks> Codec { get; } = Codecs.Composite(
            Codecs.Trailing(LoginOkSessionData.Codec),
            static blocks => blocks.SessionData,
            Codecs.Trailing(Codecs.Present(Codecs.OptionalString), absent: default, static text => text.IsPresent),
            static blocks => blocks.Text,
            Codecs.Trailing(LoginOkOptionalTexts.Codec),
            static blocks => blocks.OptionalTexts,
            Codecs.Trailing(Codecs.Present(Codecs.Optional(PlayerPermissionSet.Codec)), absent: default, static permissions => permissions.IsPresent),
            static blocks => blocks.Permissions,
            static (sessionData, text, optionalTexts, permissions) => new TrailingBlocks(sessionData, text, optionalTexts, permissions)
        );
    }
}
