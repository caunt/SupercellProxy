using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Network.Messages.Clientbound;

/// <summary>
/// Represents the <c language="csharp">LoginOkMessage</c> protocol message.
/// </summary>
internal sealed record LoginOkMessage : IMessage
{
    /// <summary>
    /// Gets or sets the <c language="csharp">LoginResult</c> value.
    /// </summary>
    public required int LoginResult { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public required int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LoginVersion</c> value.
    /// </summary>
    public required int LoginVersion { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ServerBuild</c> value.
    /// </summary>
    public required int ServerBuild { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public required bool Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">AccountId</c> value.
    /// </summary>
    public required LongId AccountId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HomeId</c> value.
    /// </summary>
    public required LongId HomeId { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CreationTimestamp</c> value.
    /// </summary>
    public required string CreationTimestamp { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CreationTimestampTrunc</c> value.
    /// </summary>
    public required string CreationTimestampTrunc { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PassToken</c> value.
    /// </summary>
    public required string PassToken { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown2</c> value.
    /// </summary>
    public required string?[] Unknown2 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CountryCode</c> value.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EventAssetsUrl</c> value.
    /// </summary>
    public required string EventAssetsUrl { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownData</c> value.
    /// </summary>
    public Memory<byte> UnknownData { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">LoginOkMessage</c> from the supplied data.
    /// </summary>
    public static LoginOkMessage Create(MessageContainer container)
    {
        return new LoginOkMessage
        {
            LoginResult = container.Payload.ReadVarInt(),
            Unknown0 = container.Payload.ReadVarInt(),
            LoginVersion = container.Payload.ReadVarInt(),
            ServerBuild = container.Payload.ReadVarInt(),
            Unknown1 = container.Payload.ReadBoolean(),
            AccountId = container.Payload.ReadLongId(),
            HomeId = container.Payload.ReadLongId(),
            CreationTimestamp = container.Payload.ReadString(),
            CreationTimestampTrunc = container.Payload.ReadString(),
            PassToken = container.Payload.ReadString(),
            Unknown2 =
            [
                container.Payload.ReadOptionalString(),
                container.Payload.ReadOptionalString(),
                container.Payload.ReadOptionalString(),
                container.Payload.ReadOptionalString(),
            ],
            CountryCode = container.Payload.ReadString(),
            EventAssetsUrl = container.Payload.ReadString(),
            UnknownData = container.Payload.ReadToEnd(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort id, ushort version = 0)
    {
        using var supercellStream = MessageStream.Create();

        supercellStream.WriteVarInt(LoginResult);
        supercellStream.WriteVarInt(Unknown0);
        supercellStream.WriteVarInt(LoginVersion);
        supercellStream.WriteVarInt(ServerBuild);
        supercellStream.WriteBoolean(Unknown1);
        supercellStream.WriteLongId(AccountId);
        supercellStream.WriteLongId(HomeId);
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
