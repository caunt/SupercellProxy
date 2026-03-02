using SupercellProxy.Playground.Network.Streams;
using SupercellProxy.Playground.Supercell;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

public record LoginOkMessage : IMessage
{
    public required long LoginResult { get; init; }
    public required long Field2 { get; init; }
    public required long LoginVersion { get; init; }
    public required long ServerBuild { get; init; }
    public required bool UnknownBool { get; init; }
    public required AccountId AccountId { get; init; }
    public required long HomeId { get; init; }
    public required string CreationTimestamp { get; init; }
    public required string CreationTimestampTrunc { get; init; }
    public required string PassToken { get; init; }
    public required string?[] UnknownStrings { get; init; }
    public required string CountryCode { get; init; }
    public required string EventAssetsUrl { get; init; }
    public Memory<byte> UnknownData { get; init; }

    public static LoginOkMessage Create(MessageContainer container)
    {
        return new LoginOkMessage
        {
            LoginResult = container.Payload.ReadVarInt64(),
            Field2 = container.Payload.ReadVarInt64(),
            LoginVersion = container.Payload.ReadVarInt64(),
            ServerBuild = container.Payload.ReadVarInt64(),
            UnknownBool = container.Payload.ReadBoolean(),
            AccountId = container.Payload.ReadAccountId(),
            HomeId = container.Payload.ReadInt64(),
            CreationTimestamp = container.Payload.ReadString(),
            CreationTimestampTrunc = container.Payload.ReadString(),
            PassToken = container.Payload.ReadString(),
            UnknownStrings =
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

        supercellStream.WriteVarInt64(LoginResult);
        supercellStream.WriteVarInt64(Field2);
        supercellStream.WriteVarInt64(LoginVersion);
        supercellStream.WriteVarInt64(ServerBuild);
        supercellStream.WriteBoolean(UnknownBool);
        supercellStream.WriteAccountId(AccountId);
        supercellStream.WriteInt64(HomeId);
        supercellStream.WriteString(CreationTimestamp);
        supercellStream.WriteString(CreationTimestampTrunc);
        supercellStream.WriteString(PassToken);

        foreach (var unknownString in UnknownStrings)
            supercellStream.WriteOptionalString(unknownString);

        supercellStream.WriteString(CountryCode);
        supercellStream.WriteString(EventAssetsUrl);

        supercellStream.Write(UnknownData.Span);

        return new MessageContainer(id, version, supercellStream);
    }
}