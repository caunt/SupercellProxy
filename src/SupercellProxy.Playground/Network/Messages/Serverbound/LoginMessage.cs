using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record LoginMessage : IMessage
{
    public LogicLong AccountId { get; set; }
    public string? PassToken { get; set; }
    public string? ResourceSha { get; set; }
    public required int LoginVersion { get; set; }

    public string? UdId { get; set; }
    public string? OpenUdId { get; set; }
    public string? MacAddress { get; set; }

    public string? DeviceModel { get; set; }

    public string? AdvertisingId { get; set; }

    public bool IsAndroid { get; set; }

    public string? OsVersion { get; set; }
    public required string UnknownString0 { get; set; }
    public required string AndroidId { get; set; }

    public required string PreferredLanguage { get; set; }

    public required string UnknownString1 { get; set; }
    public required bool AdvertisingTrackingEnabled { get; set; }
    public required string IdentifierForVendor { get; set; }

    public required AppStore AppStore { get; set; }

    public Memory<byte>? CompressedData { get; set; }
    public required string StorefrontCountryCode { get; set; }
    public required string StorefrontIdentifier { get; set; }

    public static LoginMessage Create(MessageContainer container)
    {
        return new LoginMessage
        {
            AccountId = container.Payload.ReadLogicLong(),
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
            AppStore = (AppStore)container.Payload.ReadInt32(),
            CompressedData = container.Payload.ReadOptionalByteArray(),
            StorefrontCountryCode = container.Payload.ReadString(),
            StorefrontIdentifier = container.Payload.ReadString()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 5209)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteLogicLong(AccountId);
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
        supercellStream.WriteInt32((int)AppStore);
        supercellStream.WriteOptionalByteArray(CompressedData);
        supercellStream.WriteString(StorefrontCountryCode);
        supercellStream.WriteString(StorefrontIdentifier);

        return new MessageContainer(id, version, supercellStream);
    }
}
