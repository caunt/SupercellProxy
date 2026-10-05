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
    /// Gets the server's estimated remaining maintenance duration in seconds.
    /// Nonpositive values do not provide a positive countdown. Used when <see cref="ErrorCode"/> is <see cref="LoginFailureType.Maintenance"/>.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown1")]
    public int EstimatedMaintenanceSeconds { get; init; }

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
    public LongId Unknown5 { get; init; }

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
    public static LoginFailedMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        LoginFailureType errorCode = System.Runtime.CompilerServices.Unsafe.BitCast<int, LoginFailureType>(stream.ReadInt32());

        string? resourceFingerprintData = stream.ReadOptionalString();
        string? reason = stream.ReadOptionalString();
        int estimatedMaintenanceSeconds = stream.ReadInt32();
        bool unknown2 = stream.ReadBoolean();
        string? updateAddress = stream.ReadOptionalString();
        int unknown3 = stream.ReadVarInt();
        int unknown4 = stream.ReadVarInt();
        LongId unknown5 = LongId.Empty;
        string? unknown6 = string.Empty;
        string? unknown7 = string.Empty;
        string? unknown8 = string.Empty;
        string? unknown9 = string.Empty;

        if (stream.ReadBoolean())
            unknown5 = stream.ReadLongId();

        if (stream.ReadBoolean())
            unknown6 = stream.ReadOptionalString();

        if (stream.ReadBoolean())
            unknown7 = stream.ReadOptionalString();

        if (stream.ReadBoolean())
            unknown8 = stream.ReadOptionalString();

        if (stream.ReadBoolean())
            unknown9 = stream.ReadOptionalString();

        string?[] assetsUrls = new string?[Math.Max(val1: 0, stream.ReadInt32())];

        for (int index = 0; index < assetsUrls.Length; index++)
            assetsUrls[index] = stream.ReadOptionalString();

        string? redirectHost = stream.ReadOptionalString();

        return new LoginFailedMessage
        {
            ErrorCode = errorCode,
            GameAssetFingerprintData = resourceFingerprintData,
            Reason = reason,
            EstimatedMaintenanceSeconds = estimatedMaintenanceSeconds,
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
    /// Executes the <c language="csharp">Encode</c> operation.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteInt32(System.Runtime.CompilerServices.Unsafe.BitCast<LoginFailureType, int>(ErrorCode));
        stream.WriteOptionalString(GameAssetFingerprintData);
        stream.WriteOptionalString(Reason);
        stream.WriteInt32(EstimatedMaintenanceSeconds);
        stream.WriteBoolean(Unknown2);
        stream.WriteOptionalString(UpdateAddress);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);

        bool hasUnknown5 = Unknown5 != LongId.Empty;
        stream.WriteBoolean(hasUnknown5);

        if (hasUnknown5)
            stream.WriteLongId(Unknown5);

        bool hasUnknown6 = !string.IsNullOrWhiteSpace(Unknown6);
        stream.WriteBoolean(hasUnknown6);

        if (hasUnknown6)
            stream.WriteOptionalString(Unknown6);

        bool hasUnknown7 = !string.IsNullOrWhiteSpace(Unknown7);
        stream.WriteBoolean(hasUnknown7);

        if (hasUnknown7)
            stream.WriteOptionalString(Unknown7);

        bool hasUnknown8 = !string.IsNullOrWhiteSpace(Unknown8);
        stream.WriteBoolean(hasUnknown8);

        if (hasUnknown8)
            stream.WriteOptionalString(Unknown8);

        bool hasUnknown9 = !string.IsNullOrWhiteSpace(Unknown9);
        stream.WriteBoolean(hasUnknown9);

        if (hasUnknown9)
            stream.WriteOptionalString(Unknown9);

        stream.WriteInt32(AssetsUrls?.Length ?? 0);

        if (AssetsUrls is not null)
        {
            foreach (string? address in AssetsUrls)
                stream.WriteOptionalString(address);
        }

        stream.WriteOptionalString(RedirectHost);
    }
}
