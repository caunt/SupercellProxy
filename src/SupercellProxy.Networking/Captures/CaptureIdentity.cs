using System.Buffers.Binary;
using System.Text.Json;

using SupercellProxy.Networking.Protocol.Authentication;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Captures;

/// <summary>Identifies the game release and original asset fingerprint of one recording.</summary>
public sealed record CaptureIdentity(string GameVersion, string AssetFingerprint)
{
    /// <summary>The identity document stored inside each versioned recording.</summary>
    public const string FileName = "recording.json";

    /// <summary>Reads the actual client release rather than the current client configuration.</summary>
    public static CaptureIdentity FromHello(ClientHelloMessage hello)
    {
        ArgumentNullException.ThrowIfNull(hello);
        CaptureIdentity identity = new(new Version(hello.MajorVersion, hello.MinorVersion, hello.PatchVersion).ToString(fieldCount: 3), hello.FingerprintSha1);
        identity.Validate();

        return identity;
    }

    /// <summary>Reads retained metadata or recovers it from a legacy recording's plaintext handshake.</summary>
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

        ushort helloId = MessageRegistry.GetId<ClientHelloMessage>();
        string pattern = $"*-incoming-serverbound-{helloId}-*.bin";
        CaptureIdentity? identity = null;

        foreach (string file in Directory.EnumerateFiles(capture, pattern).Order(StringComparer.Ordinal))
        {
            byte[] frame = await File.ReadAllBytesAsync(file, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            if (frame.Length < 7 || BinaryPrimitives.ReadUInt16BigEndian(frame) != helloId)
                throw new InvalidDataException(message: "The retained client handshake is truncated or misidentified.");

            int length = (frame[2] << 16) | (frame[3] << 8) | frame[4];

            if (length != frame.Length - 7)
                throw new InvalidDataException(message: "The retained client handshake has an invalid length.");

            using MessageStream stream = MessageStream.Create(frame.AsMemory(start: 7));

            CaptureIdentity candidate = FromHello(ClientHelloMessage.Decode(stream));

            if (identity is not null && identity != candidate)
                throw new InvalidDataException(message: "The recording contains different client versions or asset fingerprints.");

            identity = candidate;
        }

        return identity ?? throw new InvalidDataException(message: "The recording has no game version and asset fingerprint metadata.");
    }

    /// <summary>Writes an identity document without replacing conflicting provenance.</summary>
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

        await File.WriteAllBytesAsync(path, JsonSerializer.SerializeToUtf8Bytes(this), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    /// <summary>Requires unambiguous metadata suitable for directory names.</summary>
    public void Validate()
    {
        bool versionValid = Version.TryParse(GameVersion, out Version? version) && version.Build >= 0 && version.Revision == -1;
        bool fingerprintValid = AssetFingerprint is { Length: 40 } && AssetFingerprint.All(char.IsAsciiHexDigit);

        if (!versionValid || !fingerprintValid)
            throw new InvalidDataException(message: "A recording requires a three-part game version and a complete asset fingerprint.");
    }
}
