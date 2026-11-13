using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Sessions;


namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Serverbound login, sent right after the server's hello: which account to open (by id and pass token, or by Supercell ID session
/// token), the game release and asset hash, and the device's identity.
/// </summary>
public sealed record LoginMessage : CodecMessage<LoginMessage>, ICodecProvider<LoginMessage>
{
    /// <summary>
    /// Gets the wire codec.
    /// </summary>
    /// <value>
    /// A composite codec that writes the account id, pass token, asset hash and packed version, then the device identity block, the remaining
    /// device strings, the store, the compressed session token and the storefront fields.
    /// </value>
    public static ICodec<LoginMessage> Codec { get; } = Codecs.Composite(Codecs.LongId,
        static message => message.AccountId,
        Codecs.OptionalUtf8String,
        static message => message.PassToken,
        Codecs.OptionalUtf8String,
        static message => message.ResourceSha,
        Codecs.BigEndianInt32,
        static message => message.LoginVersion,
        DeviceIdentity.Codec,
        static message => new DeviceIdentity(message.UniqueDeviceId,
            message.OpenUniqueDeviceId,
            message.MacAddress,
            message.DeviceModel,
            message.AdvertisingId,
            message.IsAndroid,
            message.OsVersion),
        Codecs.Utf8String,
        static message => message.UnknownString0,
        Codecs.Utf8String,
        static message => message.AndroidId,
        Codecs.Utf8String,
        static message => message.PreferredLanguage,
        Codecs.Utf8String,
        static message => message.UnknownString1,
        Codecs.Boolean,
        static message => message.AdvertisingTrackingEnabled,
        Codecs.Utf8String,
        static message => message.IdForVendor,
        Codecs.Enum<AppStore>(Codecs.BigEndianInt32),
        static message => message.AppStore,
        Codecs.Map(Codecs.OptionalByteArray,
            static compressedData => compressedData is { } data ? SessionTokenData.Decode(data) : null,
            static sessionToken => sessionToken is null ? null : (ReadOnlyMemory<byte>?)sessionToken.Encode()),
        static message => message.SessionToken,
        Codecs.Utf8String,
        static message => message.StorefrontCountryCode,
        Codecs.Utf8String,
        static message => message.StorefrontId,
        static (accountId,
            passToken,
            resourceSha,
            loginVersion,
            device,
            unknownString0,
            androidId,
            preferredLanguage,
            unknownString1,
            advertisingTrackingEnabled,
            idForVendor,
            appStore,
            sessionToken,
            storefrontCountryCode,
            storefrontId) => new LoginMessage
            {
                AccountId = accountId,
                PassToken = passToken,
                ResourceSha = resourceSha,
                LoginVersion = loginVersion,
                UniqueDeviceId = device.UniqueDeviceId,
                OpenUniqueDeviceId = device.OpenUniqueDeviceId,
                MacAddress = device.MacAddress,
                DeviceModel = device.DeviceModel,
                AdvertisingId = device.AdvertisingId,
                IsAndroid = device.IsAndroid,
                OsVersion = device.OsVersion,
                UnknownString0 = unknownString0,
                AndroidId = androidId,
                PreferredLanguage = preferredLanguage,
                UnknownString1 = unknownString1,
                AdvertisingTrackingEnabled = advertisingTrackingEnabled,
                IdForVendor = idForVendor,
                AppStore = appStore,
                SessionToken = sessionToken,
                StorefrontCountryCode = storefrontCountryCode,
                StorefrontId = storefrontId,
            });

    /// <summary>
    /// Gets or sets the account to log in to; empty for a brand-new anonymous account or a Supercell ID login, where the server finds the account itself.
    /// </summary>
    /// <value>
    /// The high/low account id written first; it defaults to <see cref="LongId.Empty"/>, and an anonymous login with saved credentials sets it
    /// to their account id.
    /// </value>
    [JsonPropertyName("AccountId")]
    public LongId AccountId { get; set; }

    /// <summary>
    /// Gets or sets the device's advertising identifier (Google advertising id or IDFA), if the client shares one.
    /// </summary>
    /// <value>
    /// The fifth string of the device identity block; the managed client sends an empty string.
    /// </value>
    [JsonPropertyName("AdvertisingId")]
    public string? AdvertisingId { get; set; }

    /// <summary>
    /// Gets or sets whether the player allows ad tracking on the device.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the device allows ad tracking, which is what the managed client always sends; otherwise, <see langword="false"/>.
    /// </value>
    public required bool AdvertisingTrackingEnabled { get; set; }

    /// <summary>
    /// Gets or sets the Android device identifier; empty on other platforms.
    /// </summary>
    /// <value>
    /// A string written after <see cref="UnknownString0"/>; the managed client sends an empty string, or the anonymous login's fresh GUID.
    /// </value>
    [JsonPropertyName("AndroidId")]
    public required string AndroidId { get; set; }

    /// <summary>
    /// Gets or sets the store the game was installed from.
    /// </summary>
    /// <value>
    /// The store written as a 32-bit number, matching the hello; the managed client sends <see cref="AppStore.GooglePlay"/>.
    /// </value>
    public required AppStore AppStore { get; set; }

    /// <summary>
    /// Gets or sets the hardware model name the device reports.
    /// </summary>
    /// <value>
    /// The fourth string of the device identity block; the managed client sends an empty string.
    /// </value>
    public string? DeviceModel { get; set; }

    /// <summary>
    /// Gets or sets Apple's identifier-for-vendor of the device; empty on other platforms.
    /// </summary>
    /// <value>
    /// A UUID string written after <see cref="AdvertisingTrackingEnabled"/>; the managed client sends an empty string, or the anonymous login's
    /// fresh GUID.
    /// </value>
    public required string IdForVendor { get; set; }

    /// <summary>
    /// Gets or sets whether the client runs on Android.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the device reports itself as Android, which the managed client always does; otherwise,
    /// <see langword="false"/>.
    /// </value>
    public bool IsAndroid { get; set; }

    /// <summary>
    /// Gets or sets the game release packed into one number as major &lt;&lt; 20 | minor &lt;&lt; 10 | patch, so 1.73.81 is 1123409.
    /// </summary>
    /// <value>
    /// The packed release from <see cref="ProtocolConfiguration.LoginVersion"/>; the transport unpacks it into the stream's game version when the
    /// message is written.
    /// </value>
    public required int LoginVersion { get; set; }

    /// <summary>
    /// Gets or sets the network hardware address the device reports, if any.
    /// </summary>
    /// <value>
    /// The third string of the device identity block; the managed client sends an empty string.
    /// </value>
    public string? MacAddress { get; set; }

    /// <summary>
    /// Gets or sets the legacy OpenUDID device identifier, if the client still sends one.
    /// </summary>
    /// <value>
    /// The second string of the device identity block, serialized to JSON as <c>OpenUdId</c>; the managed client sends an empty string, or the
    /// same fresh GUID as <see cref="UniqueDeviceId"/> for an anonymous login.
    /// </value>
    [JsonPropertyName("OpenUdId")]
    public string? OpenUniqueDeviceId { get; set; }

    /// <summary>
    /// Gets or sets the operating system version the device reports.
    /// </summary>
    /// <value>
    /// The last string of the device identity block; the managed client sends an empty string.
    /// </value>
    public string? OsVersion { get; set; }

    /// <summary>
    /// Gets or sets the secret the server issued to this device for <see cref="AccountId"/>; <see langword="null"/> for a new anonymous account or a Supercell ID login.
    /// </summary>
    /// <value>
    /// The pass token the server returned in an earlier login reply, written as an optional string; <see langword="null"/> is written as a -1 length.
    /// </value>
    public string? PassToken { get; set; }

    /// <summary>
    /// Gets or sets the language code the game should use for the player, such as <c>en</c>.
    /// </summary>
    /// <value>
    /// A language code string; the managed client sends <c>en</c> for anonymous logins and an empty string otherwise.
    /// </value>
    public required string PreferredLanguage { get; set; }

    /// <summary>
    /// Gets or sets the hash of the asset bundle the client has; when it is not current, the server refuses the login with the current manifest.
    /// </summary>
    /// <value>
    /// The hexadecimal SHA of the asset fingerprint, the same text as the hello's fingerprint; an empty or <see langword="null"/> value asks for the
    /// manifest.
    /// </value>
    public string? ResourceSha { get; set; }

    /// <summary>
    /// Gets or sets the decoded Supercell ID session token, or <see langword="null"/> when the wire field is absent.
    /// </summary>
    /// <value>
    /// The token decoded from an optional byte array that holds a little-endian length and the zlib-compressed JWT; it is written back with the
    /// exact bytes it was read from, and <see langword="null"/> is written as a -1 length.
    /// </value>
    public SessionTokenData? SessionToken { get; set; }

    /// <summary>
    /// Gets or sets the country of the app store account the game is bought through, which sets the shop's prices.
    /// </summary>
    /// <value>
    /// A country code string written after <see cref="SessionToken"/>; the managed client sends an empty string.
    /// </value>
    public required string StorefrontCountryCode { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the app store storefront the game is bought through.
    /// </summary>
    /// <value>
    /// The last string of the message; the managed client sends an empty string.
    /// </value>
    public required string StorefrontId { get; set; }

    /// <summary>
    /// Gets or sets the legacy unique device identifier, if the client still sends one.
    /// </summary>
    /// <value>
    /// The first string of the device identity block, serialized to JSON as <c>UdId</c>; the managed client sends an empty string, or a fresh
    /// GUID for an anonymous login.
    /// </value>
    [JsonPropertyName("UdId")]
    public string? UniqueDeviceId { get; set; }

    /// <summary>
    /// Gets or sets the text sent right after the device identity block, before the Android id; its meaning is not identified.
    /// </summary>
    /// <value>
    /// A length-prefixed string written after <see cref="OsVersion"/>; the managed client sends an empty string.
    /// </value>
    public required string UnknownString0 { get; set; }

    /// <summary>
    /// Gets or sets the text sent after the preferred language; its meaning is not identified.
    /// </summary>
    /// <value>
    /// A length-prefixed string written between <see cref="PreferredLanguage"/> and <see cref="AdvertisingTrackingEnabled"/>; the managed client
    /// sends an empty string.
    /// </value>
    public required string UnknownString1 { get; set; }

    // The device identity block, read as one group to keep the field list within a composite.
    private readonly record struct DeviceIdentity(string? UniqueDeviceId,
        string? OpenUniqueDeviceId,
        string? MacAddress,
        string? DeviceModel,
        string? AdvertisingId,
        bool IsAndroid,
        string? OsVersion)
    {
        public static ICodec<DeviceIdentity> Codec { get; } = Codecs.Composite(Codecs.OptionalUtf8String,
            static device => device.UniqueDeviceId,
            Codecs.OptionalUtf8String,
            static device => device.OpenUniqueDeviceId,
            Codecs.OptionalUtf8String,
            static device => device.MacAddress,
            Codecs.OptionalUtf8String,
            static device => device.DeviceModel,
            Codecs.OptionalUtf8String,
            static device => device.AdvertisingId,
            Codecs.Boolean,
            static device => device.IsAndroid,
            Codecs.OptionalUtf8String,
            static device => device.OsVersion,
            static (uniqueDeviceId, openUniqueDeviceId, macAddress, deviceModel, advertisingId, isAndroid, osVersion) => new DeviceIdentity(uniqueDeviceId, openUniqueDeviceId, macAddress, deviceModel, advertisingId, isAndroid, osVersion));
    }
}
