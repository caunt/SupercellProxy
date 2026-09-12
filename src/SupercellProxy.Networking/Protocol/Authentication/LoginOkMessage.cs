using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Represents the <c language="csharp">LoginOkMessage</c> protocol message.
/// </summary>
public sealed record LoginOkMessage : IMessage
{

    /// <summary>
    /// Gets or sets the <c language="csharp">AccountId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("AccountId")]
    public required LongIdentifier AccountIdentifier { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CountryCode</c> value.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CreationTimestamp</c> value.
    /// </summary>
    public required string CreationTimestamp { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">CreationTimestampTrunc</c> value.
    /// </summary>
    public required string CreationTimestampTrunc { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">EventAssetsUrl</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("EventAssetsUrl")]
    public required string EventAssetsAddress { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">HomeId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("HomeId")]
    public required LongIdentifier HomeIdentifier { get; init; }
    /// <summary>
    /// Gets or sets the <c language="csharp">LoginResult</c> value.
    /// </summary>
    public required int LoginResult { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">LoginVersion</c> value.
    /// </summary>
    public required int LoginVersion { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">PassToken</c> value.
    /// </summary>
    public required string PassToken { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">ServerBuild</c> value.
    /// </summary>
    public required int ServerBuild { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public required int Unknown0 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public required bool Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown2</c> value.
    /// </summary>
    public required string?[] Unknown2 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownData</c> value.
    /// </summary>
    public Memory<byte> UnknownData { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">LoginOkMessage</c> from the supplied data.
    /// </summary>
    public static LoginOkMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new LoginOkMessage
        {
            LoginResult = container.Payload.ReadVariableInt(),
            Unknown0 = container.Payload.ReadVariableInt(),
            LoginVersion = container.Payload.ReadVariableInt(),
            ServerBuild = container.Payload.ReadVariableInt(),
            Unknown1 = container.Payload.ReadBoolean(),
            AccountIdentifier = container.Payload.ReadLongIdentifier(),
            HomeIdentifier = container.Payload.ReadLongIdentifier(),
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
            EventAssetsAddress = container.Payload.ReadString(),
            UnknownData = container.Payload.ReadToEnd(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream supercellStream = MessageStream.Create();

        supercellStream.WriteVariableInt(LoginResult);
        supercellStream.WriteVariableInt(Unknown0);
        supercellStream.WriteVariableInt(LoginVersion);
        supercellStream.WriteVariableInt(ServerBuild);
        supercellStream.WriteBoolean(Unknown1);
        supercellStream.WriteLongIdentifier(AccountIdentifier);
        supercellStream.WriteLongIdentifier(HomeIdentifier);
        supercellStream.WriteString(CreationTimestamp);
        supercellStream.WriteString(CreationTimestampTrunc);
        supercellStream.WriteString(PassToken);

        foreach (string? unknownString in Unknown2)
            supercellStream.WriteOptionalString(unknownString);

        supercellStream.WriteString(CountryCode);
        supercellStream.WriteString(EventAssetsAddress);

        supercellStream.Write(UnknownData.Span);

        return new MessageContainer(identifier, version, supercellStream);
    }
}
