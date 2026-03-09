using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;

namespace SupercellProxy.Playground.Network.Messages.Serverbound;

public record LoginMessage : IMessage
{
    public LogicLong AccountId { get; set; }
    public string? PassToken { get; set; }
    public string? ResourceSha { get; set; }
    public required uint LoginVersion { get; set; }

    public string? UdId { get; set; }
    public string? OpenUdId { get; set; }
    public string? MacAddress { get; set; }

    public string? DeviceModel { get; set; }

    public string? AdId { get; set; }

    public bool IsAdTracking { get; set; }

    public string? OsVersion { get; set; }
    public required string Locale { get; set; }
    public required string Idfv { get; set; }

    public required string PreferredLanguage { get; set; }

    public required string ScidString { get; set; }
    public required bool Unknown0 { get; set; }
    public required string ScIdToken { get; set; }

    public uint Unknown1 { get; set; }

    public int DataReference { get; set; }

    public Memory<byte> Unknown2 { get; set; }

    public static LoginMessage Create(MessageContainer container)
    {
        return new LoginMessage
        {
            AccountId = container.Payload.ReadLogicLong(),
            PassToken = container.Payload.ReadOptionalString(),
            ResourceSha = container.Payload.ReadOptionalString(),
            LoginVersion = container.Payload.ReadUInt32(),
            UdId = container.Payload.ReadOptionalString(),
            OpenUdId = container.Payload.ReadOptionalString(),
            MacAddress = container.Payload.ReadOptionalString(),
            DeviceModel = container.Payload.ReadOptionalString(),
            AdId = container.Payload.ReadOptionalString(),
            IsAdTracking = container.Payload.ReadBoolean(),
            OsVersion = container.Payload.ReadOptionalString(),
            Locale = container.Payload.ReadString(),
            Idfv = container.Payload.ReadString(),
            PreferredLanguage = container.Payload.ReadString(),
            ScidString = container.Payload.ReadString(),
            Unknown0 = container.Payload.ReadBoolean(),
            ScIdToken = container.Payload.ReadString(),
            Unknown1 = container.Payload.ReadUInt32(),
            DataReference = container.Payload.ReadInt32(),
            Unknown2 = container.Payload.ReadToEnd()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 5209)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteLogicLong(AccountId);
        supercellStream.WriteOptionalString(PassToken);
        supercellStream.WriteOptionalString(ResourceSha);
        supercellStream.WriteUInt32(LoginVersion);
        supercellStream.WriteOptionalString(UdId);
        supercellStream.WriteOptionalString(OpenUdId);
        supercellStream.WriteOptionalString(MacAddress);
        supercellStream.WriteOptionalString(DeviceModel);
        supercellStream.WriteOptionalString(AdId);
        supercellStream.WriteBoolean(IsAdTracking);
        supercellStream.WriteOptionalString(OsVersion);
        supercellStream.WriteString(Locale);
        supercellStream.WriteString(Idfv);
        supercellStream.WriteString(PreferredLanguage);
        supercellStream.WriteString(ScidString);
        supercellStream.WriteBoolean(Unknown0);
        supercellStream.WriteString(ScIdToken);
        supercellStream.WriteUInt32(Unknown1);
        supercellStream.WriteInt32(DataReference);
        supercellStream.Write(Unknown2.Span);

        return new MessageContainer(id, version, supercellStream);
    }
}
