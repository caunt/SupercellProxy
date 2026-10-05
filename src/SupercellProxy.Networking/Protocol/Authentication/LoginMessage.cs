using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Sessions;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Represents the <c language="csharp">LoginMessage</c> protocol message.
/// </summary>
public sealed record LoginMessage : IMessage
{
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

    /// <summary>
    /// Creates a <c language="csharp">LoginMessage</c> from the supplied data.
    /// </summary>
    public static LoginMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new LoginMessage
        {
            AccountId = stream.ReadLongId(),
            PassToken = stream.ReadOptionalString(),
            ResourceSha = stream.ReadOptionalString(),
            LoginVersion = stream.ReadInt32(),
            UniqueDeviceId = stream.ReadOptionalString(),
            OpenUniqueDeviceId = stream.ReadOptionalString(),
            MacAddress = stream.ReadOptionalString(),
            DeviceModel = stream.ReadOptionalString(),
            AdvertisingId = stream.ReadOptionalString(),
            IsAndroid = stream.ReadBoolean(),
            OsVersion = stream.ReadOptionalString(),
            UnknownString0 = stream.ReadString(),
            AndroidId = stream.ReadString(),
            PreferredLanguage = stream.ReadString(),
            UnknownString1 = stream.ReadString(),
            AdvertisingTrackingEnabled = stream.ReadBoolean(),
            IdForVendor = stream.ReadString(),
            AppStore = System.Runtime.CompilerServices.Unsafe.BitCast<int, AppStore>(stream.ReadInt32()),
            SessionToken = stream.ReadOptionalByteArray() is { } compressedData
                ? SessionTokenData.Decode(compressedData)
                : null,
            StorefrontCountryCode = stream.ReadString(),
            StorefrontId = stream.ReadString(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">Encode</c> operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteLongId(AccountId);
        stream.WriteOptionalString(PassToken);
        stream.WriteOptionalString(ResourceSha);
        stream.WriteInt32(LoginVersion);
        stream.WriteOptionalString(UniqueDeviceId);
        stream.WriteOptionalString(OpenUniqueDeviceId);
        stream.WriteOptionalString(MacAddress);
        stream.WriteOptionalString(DeviceModel);
        stream.WriteOptionalString(AdvertisingId);
        stream.WriteBoolean(IsAndroid);
        stream.WriteOptionalString(OsVersion);
        stream.WriteString(UnknownString0);
        stream.WriteString(AndroidId);
        stream.WriteString(PreferredLanguage);
        stream.WriteString(UnknownString1);
        stream.WriteBoolean(AdvertisingTrackingEnabled);
        stream.WriteString(IdForVendor);
        stream.WriteInt32(System.Runtime.CompilerServices.Unsafe.BitCast<AppStore, int>(AppStore));
        stream.WriteOptionalByteArray(SessionToken?.Encode().AsMemory());
        stream.WriteString(StorefrontCountryCode);
        stream.WriteString(StorefrontId);
    }
}
