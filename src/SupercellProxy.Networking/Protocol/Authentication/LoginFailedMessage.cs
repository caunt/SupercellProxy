using System.Text.Json;

using Nito.Disposables.Internals;

using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Represents the <c language="csharp">LoginFailedMessage</c> protocol message.
/// </summary>
public sealed record LoginFailedMessage : IMessage
{
    private static readonly JsonSerializerOptions DocumentSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// Gets or sets the <c language="csharp">AssetsUrls</c> value.
    /// </summary>
    public string?[]? AssetsUrls { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">AssetsUrlsFiltered</c> value.
    /// </summary>
    public IEnumerable<string> AssetsUrlsFiltered =>
        AssetsUrls?.Where(static address => !string.IsNullOrWhiteSpace(address)).WhereNotNull() ?? [];

    /// <summary>
    /// Gets or sets the <c language="csharp">ErrorCode</c> value.
    /// </summary>
    public required LoginFailureType ErrorCode { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">GameAssetFingerprint</c> value.
    /// </summary>
    public GameAssetFingerprint GameAssetFingerprint
    {
        get
        {
            string resourceFingerprintData =
                GameAssetFingerprintData
                ?? throw new InvalidOperationException($"{nameof(GameAssetFingerprintData)} is null.");

            GameAssetFingerprint resourceFingerprint =
                JsonSerializer.Deserialize<GameAssetFingerprint>(resourceFingerprintData, DocumentSerializerOptions)
                ?? throw new InvalidOperationException($"Failed to deserialize {nameof(GameAssetFingerprint)} from {nameof(GameAssetFingerprintData)}:\n{GameAssetFingerprintData}");

            return resourceFingerprint;
        }
    }

    /// <summary>
    /// Gets or sets the <c language="csharp">GameAssetFingerprintData</c> value.
    /// </summary>
    public string? GameAssetFingerprintData { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Reason</c> value.
    /// </summary>
    public string? Reason { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">RedirectHost</c> value.
    /// </summary>
    public string? RedirectHost { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown2</c> value.
    /// </summary>
    public bool Unknown2 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown3</c> value.
    /// </summary>
    public int Unknown3 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown4</c> value.
    /// </summary>
    public int Unknown4 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown5</c> value.
    /// </summary>
    public LongIdentifier Unknown5 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown6</c> value.
    /// </summary>
    public string? Unknown6 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown7</c> value.
    /// </summary>
    public string? Unknown7 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown8</c> value.
    /// </summary>
    public string? Unknown8 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Unknown9</c> value.
    /// </summary>
    public string? Unknown9 { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">UpdateUrl</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UpdateUrl")]
    public string? UpdateAddress { get; init; }

    /// <summary>
    /// Creates a <c language="csharp">LoginFailedMessage</c> from the supplied data.
    /// </summary>
    public static LoginFailedMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        LoginFailureType errorCode = System.Runtime.CompilerServices.Unsafe.BitCast<int, LoginFailureType>(container.Payload.ReadInt32());

        string? resourceFingerprintData = container.Payload.ReadOptionalString();
        string? reason = container.Payload.ReadOptionalString();
        int unknown1 = container.Payload.ReadInt32();
        bool unknown2 = container.Payload.ReadBoolean();
        string? updateAddress = container.Payload.ReadOptionalString();
        int unknown3 = container.Payload.ReadVariableInt();
        int unknown4 = container.Payload.ReadVariableInt();
        LongIdentifier unknown5 = LongIdentifier.Empty;
        string? unknown6 = string.Empty;
        string? unknown7 = string.Empty;
        string? unknown8 = string.Empty;
        string? unknown9 = string.Empty;

        if (container.Payload.ReadBoolean())
            unknown5 = container.Payload.ReadLongIdentifier();

        if (container.Payload.ReadBoolean())
            unknown6 = container.Payload.ReadOptionalString();

        if (container.Payload.ReadBoolean())
            unknown7 = container.Payload.ReadOptionalString();

        if (container.Payload.ReadBoolean())
            unknown8 = container.Payload.ReadOptionalString();

        if (container.Payload.ReadBoolean())
            unknown9 = container.Payload.ReadOptionalString();

        string?[] assetsUrls = new string?[Math.Max(val1: 0, container.Payload.ReadInt32())];

        for (int index = 0; index < assetsUrls.Length; index++)
            assetsUrls[index] = container.Payload.ReadOptionalString();

        string? redirectHost = container.Payload.ReadOptionalString();

        return new LoginFailedMessage
        {
            ErrorCode = errorCode,
            GameAssetFingerprintData = resourceFingerprintData,
            Reason = reason,
            Unknown1 = unknown1,
            Unknown2 = unknown2,
            UpdateAddress = updateAddress,
            Unknown3 = unknown3,
            Unknown4 = unknown4,
            Unknown5 = unknown5,
            Unknown6 = unknown6,
            Unknown7 = unknown7,
            Unknown8 = unknown8,
            Unknown9 = unknown9,
            AssetsUrls = assetsUrls,
            RedirectHost = redirectHost,
        };
    }

    /// <summary>
    /// Executes the <c language="csharp">ToContainer</c> operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream supercellStream = MessageStream.Create();

        supercellStream.WriteInt32(System.Runtime.CompilerServices.Unsafe.BitCast<LoginFailureType, int>(ErrorCode));
        supercellStream.WriteOptionalString(GameAssetFingerprintData);
        supercellStream.WriteOptionalString(Reason);
        supercellStream.WriteInt32(Unknown1);
        supercellStream.WriteBoolean(Unknown2);
        supercellStream.WriteOptionalString(UpdateAddress);
        supercellStream.WriteVariableInt(Unknown3);
        supercellStream.WriteVariableInt(Unknown4);

        bool hasUnknown5 = Unknown5 != LongIdentifier.Empty;
        supercellStream.WriteBoolean(hasUnknown5);

        if (hasUnknown5)
            supercellStream.WriteLongIdentifier(Unknown5);

        bool hasUnknown6 = !string.IsNullOrWhiteSpace(Unknown6);
        supercellStream.WriteBoolean(hasUnknown6);

        if (hasUnknown6)
            supercellStream.WriteOptionalString(Unknown6);

        bool hasUnknown7 = !string.IsNullOrWhiteSpace(Unknown7);
        supercellStream.WriteBoolean(hasUnknown7);

        if (hasUnknown7)
            supercellStream.WriteOptionalString(Unknown7);

        bool hasUnknown8 = !string.IsNullOrWhiteSpace(Unknown8);
        supercellStream.WriteBoolean(hasUnknown8);

        if (hasUnknown8)
            supercellStream.WriteOptionalString(Unknown8);

        bool hasUnknown9 = !string.IsNullOrWhiteSpace(Unknown9);
        supercellStream.WriteBoolean(hasUnknown9);

        if (hasUnknown9)
            supercellStream.WriteOptionalString(Unknown9);

        supercellStream.WriteInt32(AssetsUrls?.Length ?? 0);

        if (AssetsUrls is not null)
        {
            foreach (string? address in AssetsUrls)
                supercellStream.WriteOptionalString(address);
        }

        supercellStream.WriteOptionalString(RedirectHost);

        return new MessageContainer(identifier, version, supercellStream);
    }
}
