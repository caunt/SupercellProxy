using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record LoginMessage : IMessage
{
    public long AccountId { get; init; }
    public string? PassToken { get; init; }
    public required string ResourceSha { get; init; }
    public required int LoginVersion { get; init; }
    public string? UdId { get; init; }
    public string? OpenUdId { get; init; }
    public string? MacAddress { get; init; }
    public required string DeviceModel { get; init; }
    public required string AdId { get; init; }
    public required bool IsAdTracking { get; init; }
    public required string OsVersion { get; init; }
    public required string Locale { get; init; }
    public required string Idfv { get; init; }
    public required string PreferredLanguage { get; init; }
    public required string ScidString { get; init; }
    public required bool UnknownBool { get; init; }
    public required string ScIdToken { get; init; }
    public required int UnknownInt { get; init; }
    public required int DataRef { get; init; }
    public Memory<byte> UnknownData { get; init; }

    public static LoginMessage Create(MessageContainer container)
    {
        var accountId = container.Payload.ReadInt64();
        var passToken = container.Payload.ReadOptionalString();
        var resourceSha = container.Payload.ReadString();
        var loginVersion = container.Payload.ReadInt32();
        var udId = container.Payload.ReadOptionalString();
        var openUdId = container.Payload.ReadOptionalString();
        var macAddress = container.Payload.ReadOptionalString();
        var deviceModel = container.Payload.ReadString();
        var adId = container.Payload.ReadString();
        var isAdTracking = container.Payload.ReadBoolean();
        var osVersion = container.Payload.ReadString();
        var locale = container.Payload.ReadString();
        var idfv = container.Payload.ReadString();
        var preferredLanguage = container.Payload.ReadString();
        var scidString = container.Payload.ReadString();
        var unknownBool = container.Payload.ReadBoolean();
        var scIdToken = container.Payload.ReadString();
        var unknownInt = container.Payload.ReadInt32();
        var dataRef = container.Payload.ReadInt32();
        var unknownData = container.Payload.ReadToEnd();

        return new LoginMessage
        {
            AccountId = accountId,
            PassToken = passToken,
            ResourceSha = resourceSha,
            LoginVersion = loginVersion,
            UdId = udId,
            OpenUdId = openUdId,
            MacAddress = macAddress,
            DeviceModel = deviceModel,
            AdId = adId,
            IsAdTracking = isAdTracking,
            OsVersion = osVersion,
            Locale = locale,
            Idfv = idfv,
            PreferredLanguage = preferredLanguage,
            ScidString = scidString,
            UnknownBool = unknownBool,
            ScIdToken = scIdToken,
            UnknownInt = unknownInt,
            DataRef = dataRef,
            UnknownData = unknownData
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 5209)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteInt64(AccountId);
        supercellStream.WriteOptionalString(PassToken);
        supercellStream.WriteString(ResourceSha);
        supercellStream.WriteInt32(LoginVersion);
        supercellStream.WriteOptionalString(UdId);
        supercellStream.WriteOptionalString(OpenUdId);
        supercellStream.WriteOptionalString(MacAddress);
        supercellStream.WriteString(DeviceModel);
        supercellStream.WriteString(AdId);
        supercellStream.WriteBoolean(IsAdTracking);
        supercellStream.WriteString(OsVersion);
        supercellStream.WriteString(Locale);
        supercellStream.WriteString(Idfv);
        supercellStream.WriteString(PreferredLanguage);
        supercellStream.WriteString(ScidString);
        supercellStream.WriteBoolean(UnknownBool);
        supercellStream.WriteString(ScIdToken);
        supercellStream.WriteInt32(UnknownInt);
        supercellStream.WriteInt32(DataRef);

        supercellStream.Write(UnknownData.Span);

        return new MessageContainer(id, version, supercellStream);
    }
}
