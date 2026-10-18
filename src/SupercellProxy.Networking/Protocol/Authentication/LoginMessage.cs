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
    public static ICodec<LoginMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.AccountId,
        Codecs.OptionalString,
        static message => message.PassToken,
        Codecs.OptionalString,
        static message => message.ResourceSha,
        Codecs.Int32,
        static message => message.LoginVersion,
        DeviceIdentity.Codec,
        static message => new DeviceIdentity(
            message.UniqueDeviceId,
            message.OpenUniqueDeviceId,
            message.MacAddress,
            message.DeviceModel,
            message.AdvertisingId,
            message.IsAndroid,
            message.OsVersion
        ),
        Codecs.String,
        static message => message.UnknownString0,
        Codecs.String,
        static message => message.AndroidId,
        Codecs.String,
        static message => message.PreferredLanguage,
        Codecs.String,
        static message => message.UnknownString1,
        Codecs.Boolean,
        static message => message.AdvertisingTrackingEnabled,
        Codecs.String,
        static message => message.IdForVendor,
        Codecs.Enum<AppStore>(Codecs.Int32),
        static message => message.AppStore,
        Codecs.Map(
            Codecs.OptionalByteArray,
            static compressedData => compressedData is { } data ? SessionTokenData.Decode(data) : null,
            static sessionToken => sessionToken is null ? null : (ReadOnlyMemory<byte>?)sessionToken.Encode()
        ),
        static message => message.SessionToken,
        Codecs.String,
        static message => message.StorefrontCountryCode,
        Codecs.String,
        static message => message.StorefrontId,
        static (
            accountId,
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
            storefrontId
        ) => new LoginMessage
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
        }
    );

    /// <summary>
    /// Account to log in to; empty for a brand-new anonymous account or a Supercell ID login, where the server finds the account itself.
    /// </summary>
    [JsonPropertyName("AccountId")]
    public LongId AccountId { get; set; }

    /// <summary>
    /// The device's advertising identifier (Google advertising id or IDFA), if the client shares one.
    /// </summary>
    [JsonPropertyName("AdvertisingId")]
    public string? AdvertisingId { get; set; }

    /// <summary>
    /// Whether the player allows ad tracking on the device.
    /// </summary>
    public required bool AdvertisingTrackingEnabled { get; set; }

    /// <summary>
    /// Android device identifier; empty on other platforms.
    /// </summary>
    [JsonPropertyName("AndroidId")]
    public required string AndroidId { get; set; }

    /// <summary>
    /// Store the game was installed from.
    /// </summary>
    public required AppStore AppStore { get; set; }

    /// <summary>
    /// Hardware model name the device reports.
    /// </summary>
    public string? DeviceModel { get; set; }

    /// <summary>
    /// Apple's identifier-for-vendor of the device; empty on other platforms.
    /// </summary>
    public required string IdForVendor { get; set; }

    /// <summary>
    /// Whether the client runs on Android.
    /// </summary>
    public bool IsAndroid { get; set; }

    /// <summary>
    /// Game release packed into one number as major &lt;&lt; 20 | minor &lt;&lt; 10 | patch, so 1.73.81 is 1123409.
    /// </summary>
    public required int LoginVersion { get; set; }

    /// <summary>
    /// Network hardware address the device reports, if any.
    /// </summary>
    public string? MacAddress { get; set; }

    /// <summary>
    /// Legacy OpenUDID device identifier, if the client still sends one.
    /// </summary>
    [JsonPropertyName("OpenUdId")]
    public string? OpenUniqueDeviceId { get; set; }

    /// <summary>
    /// Operating system version the device reports.
    /// </summary>
    public string? OsVersion { get; set; }

    /// <summary>
    /// Secret the server issued to this device for <see cref="AccountId"/>; null for a new anonymous account or a Supercell ID login.
    /// </summary>
    public string? PassToken { get; set; }

    /// <summary>
    /// Language code the game should use for the player, such as <c>en</c>.
    /// </summary>
    public required string PreferredLanguage { get; set; }

    /// <summary>
    /// Hash of the asset bundle the client has; when it is not current, the server refuses the login with the current manifest.
    /// </summary>
    public string? ResourceSha { get; set; }

    /// <summary>
    /// Gets or sets the decoded Supercell ID session token, or null when the wire field is absent.
    /// </summary>
    public SessionTokenData? SessionToken { get; set; }

    /// <summary>
    /// Country of the app store account the game is bought through, which sets the shop's prices.
    /// </summary>
    public required string StorefrontCountryCode { get; set; }

    /// <summary>
    /// Identifier of the app store storefront the game is bought through.
    /// </summary>
    public required string StorefrontId { get; set; }

    /// <summary>
    /// Legacy unique device identifier, if the client still sends one.
    /// </summary>
    [JsonPropertyName("UdId")]
    public string? UniqueDeviceId { get; set; }

    /// <summary>
    /// Text sent right after the device identity block, before the Android id; its meaning is not identified.
    /// </summary>
    public required string UnknownString0 { get; set; }

    /// <summary>
    /// Text sent after the preferred language; its meaning is not identified.
    /// </summary>
    public required string UnknownString1 { get; set; }

    // The device identity block, read as one group to keep the field list within a composite.
    private readonly record struct DeviceIdentity(
        string? UniqueDeviceId,
        string? OpenUniqueDeviceId,
        string? MacAddress,
        string? DeviceModel,
        string? AdvertisingId,
        bool IsAndroid,
        string? OsVersion
    )
    {
        public static ICodec<DeviceIdentity> Codec { get; } = Codecs.Composite(
            Codecs.OptionalString,
            static device => device.UniqueDeviceId,
            Codecs.OptionalString,
            static device => device.OpenUniqueDeviceId,
            Codecs.OptionalString,
            static device => device.MacAddress,
            Codecs.OptionalString,
            static device => device.DeviceModel,
            Codecs.OptionalString,
            static device => device.AdvertisingId,
            Codecs.Boolean,
            static device => device.IsAndroid,
            Codecs.OptionalString,
            static device => device.OsVersion,
            static (uniqueDeviceId, openUniqueDeviceId, macAddress, deviceModel, advertisingId, isAndroid, osVersion) => new DeviceIdentity(uniqueDeviceId, openUniqueDeviceId, macAddress, deviceModel, advertisingId, isAndroid, osVersion)
        );
    }
}
