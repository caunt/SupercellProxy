using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

public record LoginOkMessage : IMessage
{
    public static ushort Id => 25220;

    public required int LoginResult { get; init; }
    public required int Field2 { get; init; }
    public required int LoginVersion { get; init; }
    public required int ServerBuild { get; init; }
    public required bool UnknownBool { get; init; }
    public required long AccountId { get; init; }
    public required long HomeId { get; init; }
    public required string CreationTimestamp { get; init; }
    public required string CreationTimestampTrunc { get; init; }
    public required string PassToken { get; init; }
    public required string?[] UnknownStrings { get; init; }
    public required string CountryCode { get; init; }
    public required string EventAssetsUrl { get; init; }

    static IMessage IMessage.Create(MessageContainer container)
    {
        return Create(container);
    }

    public static LoginOkMessage Create(MessageContainer container)
    {
        return new LoginOkMessage
        {
            LoginResult = container.Payload.ReadVarInt(),
            Field2 = container.Payload.ReadVarInt(),
            LoginVersion = container.Payload.ReadVarInt(),
            ServerBuild = container.Payload.ReadVarInt(),
            UnknownBool = container.Payload.ReadBoolean(),
            AccountId = container.Payload.ReadInt64(),
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
            EventAssetsUrl = container.Payload.ReadString()
        };
    }

    public MessageContainer ToContainer(ushort id, ushort version = 2)
    {
        using var supercellStream = SupercellStream.Create();

        supercellStream.WriteVarInt(LoginResult);
        supercellStream.WriteVarInt(Field2);
        supercellStream.WriteVarInt(LoginVersion);
        supercellStream.WriteVarInt(ServerBuild);
        supercellStream.WriteBoolean(UnknownBool);
        supercellStream.WriteInt64(AccountId);
        supercellStream.WriteInt64(HomeId);
        supercellStream.WriteString(CreationTimestamp);
        supercellStream.WriteString(CreationTimestampTrunc);
        supercellStream.WriteString(PassToken);

        foreach (var unknownString in UnknownStrings)
            supercellStream.WriteOptionalString(unknownString);

        supercellStream.WriteString(CountryCode);
        supercellStream.WriteString(EventAssetsUrl);

        return new MessageContainer(id, version, supercellStream);
    }
}