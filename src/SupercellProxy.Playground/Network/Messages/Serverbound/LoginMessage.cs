using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Protocol;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

/// <summary>
/// Represents the <c language="csharp">LoginMessage</c> protocol message.
/// </summary>
internal sealed record LoginMessage : IMessage
{
    /// <summary>
    /// Defines the <c language="csharp">CurrentLoginVersion</c> value.
    /// </summary>
    public const int CurrentLoginVersion = 1122388;

    /// <summary>
    /// Gets or sets the <c language="csharp">AccountId</c> value.
    /// </summary>
    public LongId AccountId { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PassToken</c> value.
    /// </summary>
    public string? PassToken { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ResourceSha</c> value.
    /// </summary>
    public string? ResourceSha { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LoginVersion</c> value.
    /// </summary>
    public required int LoginVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UdId</c> value.
    /// </summary>
    public string? UdId { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">OpenUdId</c> value.
    /// </summary>
    public string? OpenUdId { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">MacAddress</c> value.
    /// </summary>
    public string? MacAddress { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">DeviceModel</c> value.
    /// </summary>
    public string? DeviceModel { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AdvertisingId</c> value.
    /// </summary>
    public string? AdvertisingId { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IsAndroid</c> value.
    /// </summary>
    public bool IsAndroid { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">OsVersion</c> value.
    /// </summary>
    public string? OsVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString0</c> value.
    /// </summary>
    public required string UnknownString0 { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AndroidId</c> value.
    /// </summary>
    public required string AndroidId { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PreferredLanguage</c> value.
    /// </summary>
    public required string PreferredLanguage { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString1</c> value.
    /// </summary>
    public required string UnknownString1 { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AdvertisingTrackingEnabled</c> value.
    /// </summary>
    public required bool AdvertisingTrackingEnabled { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">IdentifierForVendor</c> value.
    /// </summary>
    public required string IdentifierForVendor { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AppStore</c> value.
    /// </summary>
    public required AppStore AppStore { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CompressedData</c> value.
    /// </summary>
    public Memory<byte>? CompressedData { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">StorefrontCountryCode</c> value.
    /// </summary>
    public required string StorefrontCountryCode { get; set; }

    /// <summary>
    /// Gets or sets the <c language="csharp">StorefrontIdentifier</c> value.
    /// </summary>
    public required string StorefrontIdentifier { get; set; }

    /// <summary>
    /// Creates a <c language="csharp">LoginMessage</c> from the supplied data.
    /// </summary>
    public static LoginMessage Create(MessageContainer container)
    {
        return new LoginMessage
        {
            AccountId = container.Payload.ReadLongId(),
            PassToken = container.Payload.ReadOptionalString(),
            ResourceSha = container.Payload.ReadOptionalString(),
            LoginVersion = container.Payload.ReadInt32(),
            UdId = container.Payload.ReadOptionalString(),
            OpenUdId = container.Payload.ReadOptionalString(),
            MacAddress = container.Payload.ReadOptionalString(),
            DeviceModel = container.Payload.ReadOptionalString(),
            AdvertisingId = container.Payload.ReadOptionalString(),
            IsAndroid = container.Payload.ReadBoolean(),
            OsVersion = container.Payload.ReadOptionalString(),
            UnknownString0 = container.Payload.ReadString(),
            AndroidId = container.Payload.ReadString(),
            PreferredLanguage = container.Payload.ReadString(),
            UnknownString1 = container.Payload.ReadString(),
            AdvertisingTrackingEnabled = container.Payload.ReadBoolean(),
            IdentifierForVendor = container.Payload.ReadString(),
            AppStore = System.Runtime.CompilerServices.Unsafe.BitCast<int, AppStore>(
                container.Payload.ReadInt32()
            ),
            CompressedData = container.Payload.ReadOptionalByteArray(),
            StorefrontCountryCode = container.Payload.ReadString(),
            StorefrontIdentifier = container.Payload.ReadString(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = MessageStream.Create();

        supercellStream.WriteLongId(AccountId);
        supercellStream.WriteOptionalString(PassToken);
        supercellStream.WriteOptionalString(ResourceSha);
        supercellStream.WriteInt32(LoginVersion);
        supercellStream.WriteOptionalString(UdId);
        supercellStream.WriteOptionalString(OpenUdId);
        supercellStream.WriteOptionalString(MacAddress);
        supercellStream.WriteOptionalString(DeviceModel);
        supercellStream.WriteOptionalString(AdvertisingId);
        supercellStream.WriteBoolean(IsAndroid);
        supercellStream.WriteOptionalString(OsVersion);
        supercellStream.WriteString(UnknownString0);
        supercellStream.WriteString(AndroidId);
        supercellStream.WriteString(PreferredLanguage);
        supercellStream.WriteString(UnknownString1);
        supercellStream.WriteBoolean(AdvertisingTrackingEnabled);
        supercellStream.WriteString(IdentifierForVendor);
        supercellStream.WriteInt32(
            System.Runtime.CompilerServices.Unsafe.BitCast<AppStore, int>(AppStore)
        );
        supercellStream.WriteOptionalByteArray(CompressedData);
        supercellStream.WriteString(StorefrontCountryCode);
        supercellStream.WriteString(StorefrontIdentifier);

        return new MessageContainer(id, version, supercellStream);
    }
}
