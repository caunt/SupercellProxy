using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

public record LoginOkMessage : IMessage
{
    public required int LoginResult { get; init; }
    public required int Unknown0 { get; init; }
    public required int LoginVersion { get; init; }
    public required int ServerBuild { get; init; }
    public required bool Unknown1 { get; init; }
    public required LogicLong AccountId { get; init; }
    public required LogicLong HomeId { get; init; }
    public required string CreationTimestamp { get; init; }
    public required string CreationTimestampTrunc { get; init; }
    public required string PassToken { get; init; }
    public required string?[] Unknown2 { get; init; }
    public required string CountryCode { get; init; }
    public required string EventAssetsUrl { get; init; }
    public Memory<byte> UnknownData { get; init; }

    public static LoginOkMessage Create(MessageContainer container)
    {
        return new LoginOkMessage
        {
            LoginResult = container.Payload.ReadVarInt(),
            Unknown0 = container.Payload.ReadVarInt(),
            LoginVersion = container.Payload.ReadVarInt(),
            ServerBuild = container.Payload.ReadVarInt(),
            Unknown1 = container.Payload.ReadBoolean(),
            AccountId = container.Payload.ReadLogicLong(),
            HomeId = container.Payload.ReadLogicLong(),
            CreationTimestamp = container.Payload.ReadString(),
            CreationTimestampTrunc = container.Payload.ReadString(),
            PassToken = container.Payload.ReadString(),
            Unknown2 =
            [
                container.Payload.ReadOptionalString(),
                container.Payload.ReadOptionalString(),
                container.Payload.ReadOptionalString(),
                container.Payload.ReadOptionalString()
            ],
            CountryCode = container.Payload.ReadString(),
            EventAssetsUrl = container.Payload.ReadString(),
            UnknownData = container.Payload.ReadToEnd()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 2)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteVarInt(LoginResult);
        supercellStream.WriteVarInt(Unknown0);
        supercellStream.WriteVarInt(LoginVersion);
        supercellStream.WriteVarInt(ServerBuild);
        supercellStream.WriteBoolean(Unknown1);
        supercellStream.WriteLogicLong(AccountId);
        supercellStream.WriteLogicLong(HomeId);
        supercellStream.WriteString(CreationTimestamp);
        supercellStream.WriteString(CreationTimestampTrunc);
        supercellStream.WriteString(PassToken);

        foreach (var unknownString in Unknown2)
            supercellStream.WriteOptionalString(unknownString);

        supercellStream.WriteString(CountryCode);
        supercellStream.WriteString(EventAssetsUrl);

        supercellStream.Write(UnknownData.Span);

        return new MessageContainer(id, version, supercellStream);
    }
}