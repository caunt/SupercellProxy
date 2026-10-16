using System.Text.Json;
using System.Text.Json.Serialization;

using Nito.Disposables.Internals;

using SupercellProxy.Networking.Assets;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// Clientbound refusal of a login: outdated assets or app, invalid credentials or token, maintenance, a ban, a redirect to another
/// server, and similar. The error code says which; the fields that matter for it are filled in.
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
    /// Asset server addresses to download a new bundle from, in order of preference, as sent; entries may be null or blank.
    /// </summary>
    public string?[] AssetsUrls { get; init; } = [];

    /// <summary>
    /// The asset server addresses with null and blank entries removed, ready to try one after another.
    /// </summary>
    public IEnumerable<string> AssetsUrlsFiltered =>
        AssetsUrls.Where(static address => !string.IsNullOrWhiteSpace(address)).WhereNotNull();

    /// <summary>
    /// Why the login was refused; it decides which other fields carry information (manifest, maintenance time, redirect host).
    /// </summary>
    public required LoginFailureType ErrorCode { get; init; }

    /// <summary>
    /// Gets the server's estimated remaining maintenance duration in seconds.
    /// Nonpositive values do not provide a positive countdown. Used when <see cref="ErrorCode"/> is <see cref="LoginFailureType.Maintenance"/>.
    /// </summary>
    public int EstimatedMaintenanceSeconds { get; init; }

    /// <summary>
    /// The current asset manifest, parsed from <see cref="GameAssetFingerprintData"/>; present when the error code is out-of-date
    /// content.
    /// </summary>
    /// <exception cref="InvalidOperationException">The message carries no manifest, or the manifest is not valid JSON.</exception>
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
    /// The current asset manifest as raw JSON text; null when the refusal is not about outdated content.
    /// </summary>
    public string? GameAssetFingerprintData { get; init; }

    /// <summary>
    /// Human-readable explanation from the server, such as a ban reason; null or empty when none is given.
    /// </summary>
    public string? Reason { get; init; }

    /// <summary>
    /// Game server host the client should connect to instead, sent with a redirection refusal.
    /// </summary>
    public string? RedirectHost { get; init; }

    /// <summary>
    /// Flag sent after the maintenance time; its meaning is not identified.
    /// </summary>
    public bool Unknown2 { get; init; }

    /// <summary>
    /// First number after the update address; its meaning is not identified.
    /// </summary>
    public int Unknown3 { get; init; }

    /// <summary>
    /// Second number after the update address; its meaning is not identified.
    /// </summary>
    public int Unknown4 { get; init; }

    /// <summary>
    /// Optional id sent after those numbers (written only when it is not empty); its meaning is not identified.
    /// </summary>
    public LongId Unknown5 { get; init; }

    /// <summary>
    /// First of four optional texts sent after that id; their meanings are not identified.
    /// </summary>
    public string? Unknown6 { get; init; }

    /// <summary>
    /// Second of the four optional texts; not identified.
    /// </summary>
    public string? Unknown7 { get; init; }

    /// <summary>
    /// Third of the four optional texts; not identified.
    /// </summary>
    public string? Unknown8 { get; init; }

    /// <summary>
    /// Fourth of the four optional texts; not identified.
    /// </summary>
    public string? Unknown9 { get; init; }

    /// <summary>
    /// Store page the player is sent to when the refusal says the app itself is outdated.
    /// </summary>
    [JsonPropertyName("UpdateUrl")]
    public string? UpdateAddress { get; init; }

    private static ICodec<string?> FlaggedString()
    {
        return Codecs.Flagged(Codecs.OptionalString, string.Empty, static value => !string.IsNullOrWhiteSpace(value));
    }
}
