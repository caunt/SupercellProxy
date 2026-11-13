using System.Collections.Immutable;
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
    /// <value>
    /// A composite codec that reads and writes the fields in wire order: the error code, manifest, reason, maintenance time, the unidentified
    /// fields, the asset addresses and finally the redirect host.
    /// </value>
    public static ICodec<LoginFailedMessage> Codec { get; } = Codecs.Composite(Codecs.Enum<LoginFailureType>(Codecs.BigEndianInt32),
        static message => message.ErrorCode,
        Codecs.OptionalUtf8String,
        static message => message.GameAssetFingerprintData,
        Codecs.OptionalUtf8String,
        static message => message.Reason,
        Codecs.BigEndianInt32,
        static message => message.EstimatedMaintenanceSeconds,
        Codecs.Boolean,
        static message => message.Unknown2,
        Codecs.OptionalUtf8String,
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
        Codecs.Array(Codecs.OptionalUtf8String, CollectionCount.BigEndianInt32.WithNegativeAsEmpty()),
        static message => message.AssetsUrls,
        Codecs.OptionalUtf8String,
        static message => message.RedirectHost,
        static (errorCode,
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
            redirectHost) => new LoginFailedMessage
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
            });

    private static readonly JsonSerializerOptions DocumentSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// Gets or sets the asset server addresses to download a new bundle from, in order of preference, as sent; entries may be <see langword="null"/> or blank.
    /// </summary>
    /// <value>
    /// The base addresses from an Int32-counted array of optional strings; empty by default and when the server writes a negative count.
    /// </value>
    public ImmutableArray<string?> AssetsUrls { get; init; } = [];

    /// <summary>
    /// Gets the asset server addresses with <see langword="null"/> and blank entries removed, ready to try one after another.
    /// </summary>
    /// <value>
    /// A lazily filtered view of <see cref="AssetsUrls"/> in the server's order; it is empty when the server sent no usable address.
    /// </value>
    public IEnumerable<string> AssetsUrlsFiltered =>
        AssetsUrls.Where(static address => !string.IsNullOrWhiteSpace(address)).WhereNotNull();

    /// <summary>
    /// Gets or sets why the login was refused; it decides which other fields carry information (manifest, maintenance time, redirect host).
    /// </summary>
    /// <value>
    /// The refusal code, written first as a 32-bit integer; codes not named in <see cref="LoginFailureType"/> are kept as their raw number.
    /// </value>
    public required LoginFailureType ErrorCode { get; init; }

    /// <summary>
    /// Gets or sets the server's estimated remaining maintenance duration in seconds.
    /// Nonpositive values do not provide a positive countdown. Used when <see cref="ErrorCode"/> is <see cref="LoginFailureType.Maintenance"/>.
    /// </summary>
    /// <value>
    /// A 32-bit count of seconds until the servers are expected back; 0 by default and for refusals other than maintenance.
    /// </value>
    public int EstimatedMaintenanceSeconds { get; init; }

    /// <summary>
    /// Gets the current asset manifest, parsed from <see cref="GameAssetFingerprintData"/>; present when the error code is out-of-date
    /// content.
    /// </summary>
    /// <value>
    /// A new <see cref="Assets.GameAssetFingerprint"/> deserialized from the camel-case JSON on every read, holding the bundle hash, its version
    /// and the hash of each file.
    /// </value>
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
    /// Gets or sets the current asset manifest as raw JSON text; <see langword="null"/> when the refusal is not about outdated content.
    /// </summary>
    /// <value>
    /// The fingerprint JSON (bundle hash, version and per-file hashes) exactly as sent; <see cref="GameAssetFingerprint"/> parses it on each read.
    /// </value>
    public string? GameAssetFingerprintData { get; init; }

    /// <summary>
    /// Gets or sets the human-readable explanation from the server, such as a ban reason; <see langword="null"/> or empty when none is given.
    /// </summary>
    /// <value>
    /// Free text shown to the player; <see cref="Client.LoginException"/> appends it to its message when it is not blank.
    /// </value>
    public string? Reason { get; init; }

    /// <summary>
    /// Gets or sets the game server host the client should connect to instead, sent with a redirection refusal.
    /// </summary>
    /// <value>
    /// The host written as the last field of the message; <see langword="null"/> unless <see cref="ErrorCode"/> is
    /// <see cref="LoginFailureType.Redirection"/>.
    /// </value>
    public string? RedirectHost { get; init; }

    /// <summary>
    /// Gets or sets the flag sent after the maintenance time; its meaning is not identified.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the server set this flag; otherwise, <see langword="false"/>.
    /// </value>
    /// <remarks>The flag is the Boolean written between <see cref="EstimatedMaintenanceSeconds"/> and <see cref="UpdateAddress"/>, and it defaults to <see langword="false"/>.</remarks>
    public bool Unknown2 { get; init; }

    /// <summary>
    /// Gets or sets the first number after the update address; its meaning is not identified.
    /// </summary>
    /// <value>
    /// A variable-length integer written right after <see cref="UpdateAddress"/>; it defaults to 0.
    /// </value>
    public int Unknown3 { get; init; }

    /// <summary>
    /// Gets or sets the second number after the update address; its meaning is not identified.
    /// </summary>
    /// <value>
    /// A variable-length integer written right after <see cref="Unknown3"/>; it defaults to 0.
    /// </value>
    public int Unknown4 { get; init; }

    /// <summary>
    /// Gets or sets the optional id sent after those numbers (written only when it is not empty); its meaning is not identified.
    /// </summary>
    /// <value>
    /// A presence flag followed, when set, by a high/low id pair; <see cref="LongId.Empty"/> when the flag is clear, which is also the default.
    /// </value>
    public LongId Unknown5 { get; init; }

    /// <summary>
    /// Gets or sets the first of four optional texts sent after that id; their meanings are not identified.
    /// </summary>
    /// <value>
    /// A presence flag followed, when set, by an optional string; <see langword="null"/> when the server sends none, and a blank value is written
    /// back with the flag cleared.
    /// </value>
    public string? Unknown6 { get; init; }

    /// <summary>
    /// Gets or sets the second of the four optional texts; not identified.
    /// </summary>
    /// <value>
    /// A flag-prefixed optional string written after <see cref="Unknown6"/>; <see langword="null"/> when the server sends none, and a blank value
    /// is written back as absent.
    /// </value>
    public string? Unknown7 { get; init; }

    /// <summary>
    /// Gets or sets the third of the four optional texts; not identified.
    /// </summary>
    /// <value>
    /// A flag-prefixed optional string written after <see cref="Unknown7"/>; <see langword="null"/> when the server sends none, and a blank value
    /// is written back as absent.
    /// </value>
    public string? Unknown8 { get; init; }

    /// <summary>
    /// Gets or sets the fourth of the four optional texts; not identified.
    /// </summary>
    /// <value>
    /// A flag-prefixed optional string written just before <see cref="AssetsUrls"/>; <see langword="null"/> when the server sends none, and a blank
    /// value is written back as absent.
    /// </value>
    public string? Unknown9 { get; init; }

    /// <summary>
    /// Gets or sets the store page the player is sent to when the refusal says the app itself is outdated.
    /// </summary>
    /// <value>
    /// An address written as an optional string after <see cref="Unknown2"/>; <see langword="null"/> when the server sends none, and it is serialized
    /// to JSON as <c>UpdateUrl</c>.
    /// </value>
    [JsonPropertyName("UpdateUrl")]
    public string? UpdateAddress { get; init; }

    private static ICodec<string?> FlaggedString()
    {
        return Codecs.Flagged(Codecs.OptionalUtf8String, string.Empty, static value => !string.IsNullOrWhiteSpace(value));
    }
}
