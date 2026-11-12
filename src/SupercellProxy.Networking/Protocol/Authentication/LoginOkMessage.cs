using System.Text.Json.Serialization;

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
    /// <value>
    /// A composite codec for the fixed fields in wire order, followed by four trailing blocks that are read only while payload bytes remain and
    /// written only when present.
    /// </value>
    public static ICodec<LoginOkMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt,
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
        static message => new TrailingBlocks(message.SessionData,
            new Presence<string?>(message.HasTrailingText, message.TrailingText),
            message.OptionalTexts,
            new Presence<PlayerPermissionSet?>(message.HasPermissionsBlock, message.Permissions)),
        static (sessionCount,
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
            trailingBlocks) => new LoginOkMessage
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
            });

    /// <summary>
    /// Gets or sets when the account was created, as Unix milliseconds written in text (for example <c>1598999996000</c>).
    /// </summary>
    /// <value>
    /// A decimal string written after <see cref="ServerTime"/>; it is kept as text and not parsed here.
    /// </value>
    public required string AccountCreatedDate { get; init; }

    /// <summary>
    /// Gets or sets the logged-in account. For a new anonymous account this is the id the client must store for future logins.
    /// </summary>
    /// <value>
    /// A high/low id pair; the managed client treats <see cref="LongId.Empty"/> as a failed login and rejects a different id than the saved
    /// anonymous credentials asked for.
    /// </value>
    [JsonPropertyName("AccountId")]
    public required LongId AccountId { get; init; }

    /// <summary>Gets or sets the Amazon user id linked to the account; <see langword="null"/> when none is linked.</summary>
    /// <value>
    /// The last of the four linked account ids, written as an optional string before <see cref="LoginCountry"/>.
    /// </value>
    public required string? AmazonId { get; init; }

    /// <summary>Gets or sets the content URLs; native reads a count of zero or less as no list.</summary>
    /// <value>
    /// A counted list of optional address strings written after <see cref="EventAssetsAddress"/>; <see langword="null"/> when the server sends a
    /// non-positive count, and written back with a negative count.
    /// </value>
    public required ReadOnlyMemory<string?>? ContentUrls { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EventAssetsUrl</c> value.
    /// </summary>
    /// <value>
    /// A string written after <see cref="LoginCountry"/> and serialized to JSON as <c>EventAssetsUrl</c>; by its native name an address for event
    /// assets, though nothing in this library downloads from it.
    /// </value>
    [JsonPropertyName("EventAssetsUrl")]
    public required string EventAssetsAddress { get; init; }

    /// <summary>Gets or sets the Facebook user id linked to the account; <see langword="null"/> when none is linked.</summary>
    /// <value>
    /// The first of the linked account ids, written as an optional string right after <see cref="PassToken"/>.
    /// </value>
    public required string? FacebookId { get; init; }
    /// <summary>Gets or sets the Game Center id ("gamecenter id").</summary>
    /// <value>
    /// The second of the linked account ids, as an optional string; <see langword="null"/> when no Game Center player is linked.
    /// </value>
    public required string? GamecenterId { get; init; }

    /// <summary>Gets or sets the Google Play Games id ("google service id").</summary>
    /// <value>
    /// The third of the linked account ids, as an optional string; <see langword="null"/> when no Google Play Games profile is linked.
    /// </value>
    public required string? GoogleServiceId { get; init; }

    /// <summary>Gets or sets whether the fourth trailing block was sent, since its permission set is itself optional.</summary>
    /// <value>
    /// <see langword="true"/> if the payload still had bytes for the permission block, even when its flag said no set follows, so the
    /// block is written back; otherwise, <see langword="false"/>.
    /// </value>
    public bool HasPermissionsBlock { get; init; }

    /// <summary>Gets or sets whether the second trailing block was sent, since its text may itself be <see langword="null"/>.</summary>
    /// <value>
    /// <see langword="true"/> if the payload still had bytes for the second block, so the block is written back; otherwise,
    /// <see langword="false"/>.
    /// </value>
    public bool HasTrailingText { get; init; }

    /// <summary>
    /// Gets or sets the id of the account's farm, which other players visit and which home messages refer to.
    /// </summary>
    /// <value>
    /// A high/low id pair written right after <see cref="AccountId"/>; it is a separate id space from the account id.
    /// </value>
    [JsonPropertyName("HomeId")]
    public required LongId HomeId { get; init; }

    /// <summary>
    /// Gets or sets the two-letter country code the server places this login in, such as <c>UA</c>.
    /// </summary>
    /// <value>
    /// An uppercase country code written after the linked account ids, derived by the server rather than sent by the client.
    /// </value>
    public required string LoginCountry { get; init; }


    /// <summary>
    /// Gets or sets the game release the server accepted, packed as major &lt;&lt; 20 | minor &lt;&lt; 10 | patch (1122390 is 1.72.86).
    /// </summary>
    /// <value>
    /// The packed release as a variable-length integer; it echoes the <see cref="LoginMessage.LoginVersion"/> the client sent.
    /// </value>
    public required int LoginVersion { get; init; }

    /// <summary>Gets or sets the third trailing block, present only when the server sent more bytes.</summary>
    /// <value>
    /// Four texts of unidentified meaning, each behind its own presence flag; <see langword="null"/> when the payload ends before the block.
    /// </value>
    public LoginOkOptionalTexts? OptionalTexts { get; init; }

    /// <summary>
    /// Gets or sets the secret the client must store with the account id and send in later logins to prove it owns the account.
    /// </summary>
    /// <value>
    /// An opaque token string that goes back in <see cref="LoginMessage.PassToken"/>; it is written after <see cref="AccountCreatedDate"/>.
    /// </value>
    public required string PassToken { get; init; }

    /// <summary>Gets or sets the fourth trailing block's permission set, when sent.</summary>
    /// <value>
    /// The permission state and per-feature restrictions behind their own presence flag; <see langword="null"/> when the block is missing or its
    /// flag is clear, which <see cref="HasPermissionsBlock"/> tells apart.
    /// </value>
    public PlayerPermissionSet? Permissions { get; init; }

    /// <summary>
    /// Gets or sets the account's total online play time in seconds; between captured logins it never grows faster than the wall clock.
    /// </summary>
    /// <value>
    /// A non-negative variable-length count of seconds summed over all of the account's sessions, written right after <see cref="SessionCount"/>.
    /// </value>
    public required int PlayTimeSeconds { get; init; }

    /// <summary>
    /// Gets or sets the number sent after the game release; captured logins carry 1, and its meaning is not identified.
    /// </summary>
    /// <value>
    /// A variable-length integer written after <see cref="LoginVersion"/>; despite its name it has not been tied to a server build.
    /// </value>
    public required int ServerBuild { get; init; }

    /// <summary>
    /// Gets or sets the server's clock at login, as Unix milliseconds written in text.
    /// </summary>
    /// <value>
    /// A decimal string such as <c>1598999996000</c>, written after <see cref="HomeId"/>; it is kept as text and not parsed here.
    /// </value>
    public required string ServerTime { get; init; }

    /// <summary>
    /// Gets or sets the account's login count, which native logs as <c>sessionCount</c>; it grows by exactly one per captured login.
    /// </summary>
    /// <value>
    /// A variable-length integer written as the first field of the message; it includes the login this reply answers.
    /// </value>
    public required int SessionCount { get; init; }

    /// <summary>Gets or sets the first trailing block, present only when the server sent more bytes.</summary>
    /// <value>
    /// The compressed document and two flags that follow <see cref="ContentUrls"/>; <see langword="null"/> when the payload ends before the block,
    /// as it does from older servers, and then the block is left out on write.
    /// </value>
    public LoginOkSessionData? SessionData { get; init; }

    /// <summary>Gets or sets the second trailing block's text, present only when the server sent more bytes.</summary>
    /// <value>
    /// An optional string whose meaning is not identified; <see langword="null"/> both when the block is missing and when the server sent a
    /// <see langword="null"/> text, which <see cref="HasTrailingText"/> tells apart.
    /// </value>
    public string? TrailingText { get; init; }

    /// <summary>
    /// Gets or sets the flag sent after the server build number; its meaning is not identified.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the server set this flag; otherwise, <see langword="false"/>.
    /// </value>
    /// <remarks>The flag is the Boolean written between <see cref="ServerBuild"/> and <see cref="AccountId"/>; nothing in this library reads it.</remarks>
    public required bool Unknown1 { get; init; }

    // The linked third-party account ids, read as one group to keep the field list within a composite.
    private readonly record struct LinkedAccountIds(string? FacebookId, string? GamecenterId, string? GoogleServiceId, string? AmazonId)
    {
        public static ICodec<LinkedAccountIds> Codec { get; } = Codecs.Composite(Codecs.OptionalString,
            static ids => ids.FacebookId,
            Codecs.OptionalString,
            static ids => ids.GamecenterId,
            Codecs.OptionalString,
            static ids => ids.GoogleServiceId,
            Codecs.OptionalString,
            static ids => ids.AmazonId,
            static (facebookId, gamecenterId, googleServiceId, amazonId) => new LinkedAccountIds(facebookId, gamecenterId, googleServiceId, amazonId));
    }

    // Native reads each block only while payload bytes remain, so older servers end the message earlier.
    private readonly record struct TrailingBlocks(LoginOkSessionData? SessionData,
        Presence<string?> Text,
        LoginOkOptionalTexts? OptionalTexts,
        Presence<PlayerPermissionSet?> Permissions)
    {
        public static ICodec<TrailingBlocks> Codec { get; } = Codecs.Composite(Codecs.Trailing(LoginOkSessionData.Codec),
            static blocks => blocks.SessionData,
            Codecs.Trailing(Codecs.Present(Codecs.OptionalString), absent: default, static text => text.IsPresent),
            static blocks => blocks.Text,
            Codecs.Trailing(LoginOkOptionalTexts.Codec),
            static blocks => blocks.OptionalTexts,
            Codecs.Trailing(Codecs.Present(Codecs.Optional(PlayerPermissionSet.Codec)), absent: default, static permissions => permissions.IsPresent),
            static blocks => blocks.Permissions,
            static (sessionData, text, optionalTexts, permissions) => new TrailingBlocks(sessionData, text, optionalTexts, permissions));
    }
}
