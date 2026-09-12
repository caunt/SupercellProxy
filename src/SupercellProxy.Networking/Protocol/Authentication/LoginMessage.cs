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
    /// Defines the <c language="csharp">CurrentLoginVersion</c> value.
    /// </summary>
    public const int CurrentLoginVersion = 1122388;

    /// <summary>
    /// Gets or sets the <c language="csharp">AccountId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AccountId")]
    public LongIdentifier AccountIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AdvertisingId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AdvertisingId")]
    public string? AdvertisingIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AdvertisingTrackingEnabled</c> value.
    /// </summary>
    public required bool AdvertisingTrackingEnabled { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AndroidId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AndroidId")]
    public required string AndroidIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AppStore</c> value.
    /// </summary>
    public required AppStore AppStore { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DeviceModel</c> value.
    /// </summary>
    public string? DeviceModel { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IdentifierForVendor</c> value.
    /// </summary>
    public required string IdentifierForVendor { get; set; }

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
    public string? OpenUniqueDeviceIdentifier { get; set; }

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
    public LoginSessionToken? SessionToken { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">StorefrontCountryCode</c> value.
    /// </summary>
    public required string StorefrontCountryCode { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">StorefrontIdentifier</c> value.
    /// </summary>
    public required string StorefrontIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UdId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UdId")]
    public string? UniqueDeviceIdentifier { get; set; }

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
    public static LoginMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new LoginMessage
        {
            AccountIdentifier = container.Payload.ReadLongIdentifier(),
            PassToken = container.Payload.ReadOptionalString(),
            ResourceSha = container.Payload.ReadOptionalString(),
            LoginVersion = container.Payload.ReadInt32(),
            UniqueDeviceIdentifier = container.Payload.ReadOptionalString(),
            OpenUniqueDeviceIdentifier = container.Payload.ReadOptionalString(),
            MacAddress = container.Payload.ReadOptionalString(),
            DeviceModel = container.Payload.ReadOptionalString(),
            AdvertisingIdentifier = container.Payload.ReadOptionalString(),
            IsAndroid = container.Payload.ReadBoolean(),
            OsVersion = container.Payload.ReadOptionalString(),
            UnknownString0 = container.Payload.ReadString(),
            AndroidIdentifier = container.Payload.ReadString(),
            PreferredLanguage = container.Payload.ReadString(),
            UnknownString1 = container.Payload.ReadString(),
            AdvertisingTrackingEnabled = container.Payload.ReadBoolean(),
            IdentifierForVendor = container.Payload.ReadString(),
            AppStore = System.Runtime.CompilerServices.Unsafe.BitCast<int, AppStore>(container.Payload.ReadInt32()),
            SessionToken = container.Payload.ReadOptionalByteArray() is { } compressedData
                ? LoginSessionToken.Decode(compressedData)
                : null,
            StorefrontCountryCode = container.Payload.ReadString(),
            StorefrontIdentifier = container.Payload.ReadString(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream supercellStream = MessageStream.Create();

        supercellStream.WriteLongIdentifier(AccountIdentifier);
        supercellStream.WriteOptionalString(PassToken);
        supercellStream.WriteOptionalString(ResourceSha);
        supercellStream.WriteInt32(LoginVersion);
        supercellStream.WriteOptionalString(UniqueDeviceIdentifier);
        supercellStream.WriteOptionalString(OpenUniqueDeviceIdentifier);
        supercellStream.WriteOptionalString(MacAddress);
        supercellStream.WriteOptionalString(DeviceModel);
        supercellStream.WriteOptionalString(AdvertisingIdentifier);
        supercellStream.WriteBoolean(IsAndroid);
        supercellStream.WriteOptionalString(OsVersion);
        supercellStream.WriteString(UnknownString0);
        supercellStream.WriteString(AndroidIdentifier);
        supercellStream.WriteString(PreferredLanguage);
        supercellStream.WriteString(UnknownString1);
        supercellStream.WriteBoolean(AdvertisingTrackingEnabled);
        supercellStream.WriteString(IdentifierForVendor);
        supercellStream.WriteInt32(System.Runtime.CompilerServices.Unsafe.BitCast<AppStore, int>(AppStore));
        supercellStream.WriteOptionalByteArray(SessionToken?.Encode().AsMemory());
        supercellStream.WriteString(StorefrontCountryCode);
        supercellStream.WriteString(StorefrontIdentifier);

        return new MessageContainer(identifier, version, supercellStream);
    }
}
