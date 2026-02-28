using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record LoginMessage : IMessage
{
    public static ushort Id => 10101;

    public required long AccountId { get; init; }
    public required string PassToken { get; init; }
    public required string ResourceSha { get; init; }
    public required int LoginVersion { get; init; }
    public required string UdId { get; init; }
    public required string OpenUdId { get; init; }
    public required string MacAddress { get; init; }
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
    public required string SystemString1 { get; init; }
    public required string SystemString2 { get; init; }

    static IMessage IMessage.Create(MessageContainer container)
    {
        return Create(container);
    }

    public static LoginMessage Create(MessageContainer container)
    {
        return new LoginMessage
        {
            AccountId = container.Payload.ReadInt64(),
            PassToken = container.Payload.ReadString(),
            ResourceSha = container.Payload.ReadString(),
            LoginVersion = container.Payload.ReadInt32(),
            UdId = container.Payload.ReadString(),
            OpenUdId = container.Payload.ReadString(),
            MacAddress = container.Payload.ReadString(),
            DeviceModel = container.Payload.ReadString(),
            AdId = container.Payload.ReadString(),
            IsAdTracking = container.Payload.ReadBoolean(),
            OsVersion = container.Payload.ReadString(),
            Locale = container.Payload.ReadString(),
            Idfv = container.Payload.ReadString(),
            PreferredLanguage = container.Payload.ReadString(),
            ScidString = container.Payload.ReadString(),
            UnknownBool = container.Payload.ReadBoolean(),
            ScIdToken = container.Payload.ReadString(),
            UnknownInt = container.Payload.ReadInt32(),
            DataRef = container.Payload.ReadInt32(),
            SystemString1 = container.Payload.ReadString(),
            SystemString2 = container.Payload.ReadString()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 5209)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteInt64(AccountId);
        supercellStream.WriteString(PassToken);
        supercellStream.WriteString(ResourceSha);
        supercellStream.WriteInt32(LoginVersion);
        supercellStream.WriteString(UdId);
        supercellStream.WriteString(OpenUdId);
        supercellStream.WriteString(MacAddress);
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
        supercellStream.WriteString(SystemString1);
        supercellStream.WriteString(SystemString2);

        return new MessageContainer(id, version, supercellStream);
    }
}
