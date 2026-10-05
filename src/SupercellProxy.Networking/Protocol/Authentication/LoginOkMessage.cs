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
    public required LongId AccountId { get; init; }

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
    public required LongId HomeId { get; init; }
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
    public static LoginOkMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new LoginOkMessage
        {
            LoginResult = stream.ReadVarInt(),
            Unknown0 = stream.ReadVarInt(),
            LoginVersion = stream.ReadVarInt(),
            ServerBuild = stream.ReadVarInt(),
            Unknown1 = stream.ReadBoolean(),
            AccountId = stream.ReadLongId(),
            HomeId = stream.ReadLongId(),
            CreationTimestamp = stream.ReadString(),
            CreationTimestampTrunc = stream.ReadString(),
            PassToken = stream.ReadString(),
            Unknown2 =
            [
                stream.ReadOptionalString(),
                stream.ReadOptionalString(),
                stream.ReadOptionalString(),
                stream.ReadOptionalString(),
            ],
            CountryCode = stream.ReadString(),
            EventAssetsAddress = stream.ReadString(),
            UnknownData = stream.ReadToEnd(),
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">Encode</c> operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(LoginResult);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(LoginVersion);
        stream.WriteVarInt(ServerBuild);
        stream.WriteBoolean(Unknown1);
        stream.WriteLongId(AccountId);
        stream.WriteLongId(HomeId);
        stream.WriteString(CreationTimestamp);
        stream.WriteString(CreationTimestampTrunc);
        stream.WriteString(PassToken);

        foreach (string? unknownString in Unknown2)
            stream.WriteOptionalString(unknownString);

        stream.WriteString(CountryCode);
        stream.WriteString(EventAssetsAddress);

        stream.Write(UnknownData.Span);
    }
}
