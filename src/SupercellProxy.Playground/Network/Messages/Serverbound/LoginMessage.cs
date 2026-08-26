using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Protocol;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

/// <summary>
/// Represents the <c>LoginMessage</c> protocol message.
/// </summary>
public record LoginMessage : IMessage
{
    /// <summary>
    /// Defines the <c>CurrentLoginVersion</c> value.
    /// </summary>
    public const int CurrentLoginVersion = 1122388;

    /// <summary>
    /// Gets or sets the <c>AccountId</c> value.
    /// </summary>
    public LongId AccountId { get; set; }

    /// <summary>
    /// Gets or sets the <c>PassToken</c> value.
    /// </summary>
    public string? PassToken { get; set; }

    /// <summary>
    /// Gets or sets the <c>ResourceSha</c> value.
    /// </summary>
    public string? ResourceSha { get; set; }

    /// <summary>
    /// Gets or sets the <c>LoginVersion</c> value.
    /// </summary>
    public required int LoginVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c>UdId</c> value.
    /// </summary>
    public string? UdId { get; set; }

    /// <summary>
    /// Gets or sets the <c>OpenUdId</c> value.
    /// </summary>
    public string? OpenUdId { get; set; }

    /// <summary>
    /// Gets or sets the <c>MacAddress</c> value.
    /// </summary>
    public string? MacAddress { get; set; }

    /// <summary>
    /// Gets or sets the <c>DeviceModel</c> value.
    /// </summary>
    public string? DeviceModel { get; set; }

    /// <summary>
    /// Gets or sets the <c>AdvertisingId</c> value.
    /// </summary>
    public string? AdvertisingId { get; set; }

    /// <summary>
    /// Gets or sets the <c>IsAndroid</c> value.
    /// </summary>
    public bool IsAndroid { get; set; }

    /// <summary>
    /// Gets or sets the <c>OsVersion</c> value.
    /// </summary>
    public string? OsVersion { get; set; }

    /// <summary>
    /// Gets or sets the <c>UnknownString0</c> value.
    /// </summary>
    public required string UnknownString0 { get; set; }

    /// <summary>
    /// Gets or sets the <c>AndroidId</c> value.
    /// </summary>
    public required string AndroidId { get; set; }

    /// <summary>
    /// Gets or sets the <c>PreferredLanguage</c> value.
    /// </summary>
    public required string PreferredLanguage { get; set; }

    /// <summary>
    /// Gets or sets the <c>UnknownString1</c> value.
    /// </summary>
    public required string UnknownString1 { get; set; }

    /// <summary>
    /// Gets or sets the <c>AdvertisingTrackingEnabled</c> value.
    /// </summary>
    public required bool AdvertisingTrackingEnabled { get; set; }

    /// <summary>
    /// Gets or sets the <c>IdentifierForVendor</c> value.
    /// </summary>
    public required string IdentifierForVendor { get; set; }

    /// <summary>
    /// Gets or sets the <c>AppStore</c> value.
    /// </summary>
    public required AppStore AppStore { get; set; }

    /// <summary>
    /// Gets or sets the <c>CompressedData</c> value.
    /// </summary>
    public Memory<byte>? CompressedData { get; set; }

    /// <summary>
    /// Gets or sets the <c>StorefrontCountryCode</c> value.
    /// </summary>
    public required string StorefrontCountryCode { get; set; }

    /// <summary>
    /// Gets or sets the <c>StorefrontIdentifier</c> value.
    /// </summary>
    public required string StorefrontIdentifier { get; set; }

    /// <summary>
    /// Creates a <c>LoginMessage</c> from the supplied data.
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
    /// Executes the <c>ToContainer</c> operation.
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
