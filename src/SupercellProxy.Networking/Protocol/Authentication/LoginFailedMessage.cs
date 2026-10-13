using System.Text.Json;

using Nito.Disposables.Internals;

using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Represents the <c language="csharp">LoginFailedMessage</c> protocol message.
/// </summary>
public sealed record LoginFailedMessage : CodecMessage<LoginFailedMessage>, ICodecProvider<LoginFailedMessage>
{
    /// <summary>
    /// Gets the wire codec.
    /// </summary>
    public static ICodec<LoginFailedMessage> Codec { get; } = Codecs.Composite(
        Codecs.Enum<LoginFailureType>(Codecs.Int32),
        static message => message.ErrorCode,
        Codecs.OptionalString,
        static message => message.GameAssetFingerprintData,
        Codecs.OptionalString,
        static message => message.Reason,
        Codecs.Int32,
        static message => message.EstimatedMaintenanceSeconds,
        Codecs.Boolean,
        static message => message.Unknown2,
        Codecs.OptionalString,
        static message => message.UpdateAddress,
        Codecs.VarInt,
        static message => message.Unknown3,
        Codecs.VarInt,
        static message => message.Unknown4,
        Codecs.Flagged(Codecs.LongId, LongId.Empty, static value => value != LongId.Empty),
        static message => message.Unknown5,
        FlaggedString(),
        static message => message.Unknown6,
        FlaggedString(),
        static message => message.Unknown7,
        FlaggedString(),
        static message => message.Unknown8,
        FlaggedString(),
        static message => message.Unknown9,
        Codecs.Array(Codecs.OptionalString, CollectionCount.Int32.WithNegativeAsEmpty()),
        static message => message.AssetsUrls,
        Codecs.OptionalString,
        static message => message.RedirectHost,
        static (
            errorCode,
            gameAssetFingerprintData,
            reason,
            estimatedMaintenanceSeconds,
            unknown2,
            updateAddress,
            unknown3,
            unknown4,
            unknown5,
            unknown6,
            unknown7,
            unknown8,
            unknown9,
            assetsUrls,
            redirectHost
        ) => new LoginFailedMessage
        {
            ErrorCode = errorCode,
            GameAssetFingerprintData = gameAssetFingerprintData,
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
        }
    );

    private static readonly JsonSerializerOptions DocumentSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// Gets or sets the <c language="csharp">AssetsUrls</c> value.
    /// </summary>
    public string?[] AssetsUrls { get; init; } = [];

    /// <summary>
    /// Gets the <c language="csharp">AssetsUrlsFiltered</c> value.
    /// </summary>
    public IEnumerable<string> AssetsUrlsFiltered =>
        AssetsUrls.Where(static address => !string.IsNullOrWhiteSpace(address)).WhereNotNull();

    /// <summary>
    /// Gets or sets the <c language="csharp">ErrorCode</c> value.
    /// </summary>
    public required LoginFailureType ErrorCode { get; init; }

    /// <summary>
    /// Gets the server's estimated remaining maintenance duration in seconds.
    /// Nonpositive values do not provide a positive countdown. Used when <see cref="ErrorCode"/> is <see cref="LoginFailureType.Maintenance"/>.
    /// </summary>
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

    private static ICodec<string?> FlaggedString()
    {
        return Codecs.Flagged(Codecs.OptionalString, string.Empty, static value => !string.IsNullOrWhiteSpace(value));
    }
}
