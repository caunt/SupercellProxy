using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Sessions;


namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Represents the <c language="csharp">LoginMessage</c> protocol message.
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
    /// Gets or sets the <c language="csharp">AccountId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AccountId")]
    public LongId AccountId { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AdvertisingId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AdvertisingId")]
    public string? AdvertisingId { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AdvertisingTrackingEnabled</c> value.
    /// </summary>
    public required bool AdvertisingTrackingEnabled { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AndroidId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AndroidId")]
    public required string AndroidId { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AppStore</c> value.
    /// </summary>
    public required AppStore AppStore { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DeviceModel</c> value.
    /// </summary>
    public string? DeviceModel { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IdForVendor</c> value.
    /// </summary>
    public required string IdForVendor { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IsAndroid</c> value.
    /// </summary>
    public bool IsAndroid { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LoginVersion</c> value.
    /// </summary>
    public required int LoginVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MacAddress</c> value.
    /// </summary>
    public string? MacAddress { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">OpenUdId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("OpenUdId")]
    public string? OpenUniqueDeviceId { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">OsVersion</c> value.
    /// </summary>
    public string? OsVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PassToken</c> value.
    /// </summary>
    public string? PassToken { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PreferredLanguage</c> value.
    /// </summary>
    public required string PreferredLanguage { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ResourceSha</c> value.
    /// </summary>
    public string? ResourceSha { get; set; }

    /// <summary>
    /// Gets or sets the decoded Supercell ID session token, or null when the wire field is absent.
    /// </summary>
    public SessionTokenData? SessionToken { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">StorefrontCountryCode</c> value.
    /// </summary>
    public required string StorefrontCountryCode { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">StorefrontId</c> value.
    /// </summary>
    public required string StorefrontId { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UdId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UdId")]
    public string? UniqueDeviceId { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString0</c> value.
    /// </summary>
    public required string UnknownString0 { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString1</c> value.
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
