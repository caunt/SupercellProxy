using System.Buffers.Binary;
using System.Text.Json;

using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Captures;

/// <summary>Identifies the game release and original asset fingerprint of one recording.</summary>
/// <param name="GameVersion">The three-part client release, such as <c>1.73.81</c>; it also names the recording's directory in a <see cref="CaptureArchive"/>.</param>
/// <param name="AssetFingerprint">The 40-character hexadecimal SHA-1 fingerprint of the asset set the client reported in its hello.</param>
public sealed record CaptureIdentity(string GameVersion, string AssetFingerprint)
{
    /// <summary>The identity document stored inside each versioned recording.</summary>
    public const string FileName = "recording.json";

    /// <summary>Reads the actual client release rather than the current client configuration.</summary>
    /// <param name="hello">The client hello sent at the start of the recorded session; its major, minor and patch versions and asset fingerprint are used.</param>
    /// <returns>A validated identity with the version written as <c>major.minor.patch</c> and the hello's SHA-1 asset fingerprint.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="hello"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidDataException">The hello's asset fingerprint is not 40 hexadecimal characters.</exception>
    public static CaptureIdentity From(ClientHelloMessage hello)
    {
        ArgumentNullException.ThrowIfNull(hello);

        CaptureIdentity identity = new(new Version(hello.MajorVersion, hello.MinorVersion, hello.PatchVersion).ToString(fieldCount: 3), hello.FingerprintSha1);

        identity.Validate();

        return identity;
    }

    /// <summary>Reads retained metadata or recovers it from a legacy recording's plaintext handshake.</summary>
    /// <param name="capture">The recording directory; it is searched for <c>recording.json</c> first and otherwise for the recorded serverbound client hello frames.</param>
    /// <param name="cancellationToken">Cancels reading the identity document or the handshake frames.</param>
    /// <returns>The validated identity from <c>recording.json</c>, or the one shared by every recorded client hello when that document is absent.</returns>
    /// <exception cref="InvalidDataException">The identity document is empty or invalid, a hello frame is truncated, has another message id or a wrong length, the hello frames disagree, or the recording has neither the document nor any hello frame.</exception>
    public static async Task<CaptureIdentity> ReadAsync(string capture, CancellationToken cancellationToken = default)
    {
        string metadata = Path.Combine(capture, FileName);

        if (File.Exists(metadata))
        {
            CaptureIdentity retained = JsonSerializer.Deserialize<CaptureIdentity>(await File.ReadAllBytesAsync(metadata, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
                ?? throw new InvalidDataException(message: "The capture identity is empty.");

            retained.Validate();

            return retained;
        }

        ushort helloId = MessageRegistry.GetId<ClientHelloMessage>(gameVersion: null);
        string pattern = $"*-incoming-serverbound-{helloId}-*.bin";
        CaptureIdentity? identity = null;

        foreach (string file in Directory.EnumerateFiles(capture, pattern).Order(StringComparer.Ordinal))
        {
            byte[] frame = await File.ReadAllBytesAsync(file, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (frame.Length < 7 || BinaryPrimitives.ReadUInt16BigEndian(frame) != helloId)
                throw new InvalidDataException(message: "The retained client handshake is truncated or misidentified.");

            int length = (frame[2] << 16) | (frame[3] << 8) | frame[4];

            if (length != frame.Length - 7)
                throw new InvalidDataException(message: "The retained client handshake has an invalid length.");

            using MessageChannel stream = MessageChannel.Create(frame.AsMemory(start: 7));

            CaptureIdentity candidate = From(ClientHelloMessage.Codec.Read(stream));

            if (identity is not null && identity != candidate)
                throw new InvalidDataException(message: "The recording contains different client versions or asset fingerprints.");

            identity = candidate;
        }

        return identity ?? throw new InvalidDataException(message: "The recording has no game version and asset fingerprint metadata.");
    }

    /// <summary>Writes an identity document without replacing conflicting provenance.</summary>
    /// <param name="capture">The recording directory that receives <c>recording.json</c>; it must already exist.</param>
    /// <param name="cancellationToken">Cancels reading an existing document or writing the new one.</param>
    /// <exception cref="InvalidDataException">This identity fails <see cref="Validate"/>, or the recording already has an identity document with a different version or fingerprint.</exception>
    /// <remarks>An existing document with the same identity is left untouched, so saving twice is harmless.</remarks>
    public async Task SaveAsync(string capture, CancellationToken cancellationToken = default)
    {
        Validate();

        string path = Path.Combine(capture, FileName);

        if (File.Exists(path))
        {
            if (await ReadAsync(capture, cancellationToken).ConfigureAwait(continueOnCapturedContext: false) != this)
                throw new InvalidDataException(message: "The retained capture identity differs from the recording.");

            return;
        }

        await File.WriteAllBytesAsync(path, JsonSerializer.SerializeToUtf8Bytes(this), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>Requires unambiguous metadata suitable for directory names.</summary>
    /// <exception cref="InvalidDataException"><see cref="GameVersion"/> is not exactly three numeric parts, such as <c>1.73.81</c>, or <see cref="AssetFingerprint"/> is not 40 hexadecimal characters.</exception>
    public void Validate()
    {
        bool versionValid = Version.TryParse(GameVersion, out Version? version) && version.Build >= 0 && version.Revision == -1;
        bool fingerprintValid = AssetFingerprint is { Length: 40 } && AssetFingerprint.All(char.IsAsciiHexDigit);

        if (!versionValid || !fingerprintValid)
            throw new InvalidDataException(message: "A recording requires a three-part game version and a complete asset fingerprint.");
    }
}
