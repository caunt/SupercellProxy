using System.Buffers.Binary;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace SupercellProxy.Networking.Sessions;

/// <summary>
/// Represents a decoded ES256 session token or the login protocol's empty-string marker.
/// </summary>
public sealed class LoginSessionToken
{
    private const uint AdlerModulus = 65_521;
    private static readonly UTF8Encoding StrictUnicodeTransformationFormat8 = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    private readonly byte[] _wireData;

    private LoginSessionToken(string value, byte[] wireData, LoginSessionHeader? header, LoginSessionClaims? claims)
    {
        Value = value;
        _wireData = wireData;
        Header = header;
        Claims = claims;
    }

    /// <summary>
    /// Gets the decoded session claims, or null for the empty-string marker.
    /// </summary>
    public LoginSessionClaims? Claims { get; }

    /// <summary>
    /// Gets the token's expiration time in Unix seconds, or null for an empty-string marker.
    /// </summary>
    public long? ExpiresAt => Claims?.ExpiresAt;

    /// <summary>Gets the decoded JWT header, or null for the empty-string marker.</summary>
    public LoginSessionHeader? Header { get; }

    /// <summary>
    /// Gets the Initial Refresh Token Issued At value.
    /// </summary>
    public long? InitialRefreshTokenIssuedAt => Claims?.InitialRefreshTokenIssuedAt;

    /// <summary>
    /// Gets whether the login field contains an empty-string marker with no session token.
    /// </summary>
    public bool IsEmpty => Value.Length is 0;

    /// <summary>
    /// Gets the token's issue time in Unix seconds, or null for an empty-string marker.
    /// </summary>
    public long? IssuedAt => Claims?.IssuedAt;

    /// <summary>
    /// Gets the decoded JWT text, or an empty string when no session token is supplied.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Decodes a JWT or an empty-string marker from the login protocol's wire representation.
    /// </summary>
    public static LoginSessionToken Decode(ReadOnlyMemory<byte> wireData)
    {
        if (wireData.Length < sizeof(int) + 6)
            throw new InvalidDataException(message: "Compressed login session token has no declared decoded length.");

        int decodedLength = BinaryPrimitives.ReadInt32LittleEndian(wireData.Span);

        if (decodedLength < 0)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Compressed login session token has invalid decoded length {decodedLength}."));

        using MemoryStream input = new(wireData[sizeof(int)..].ToArray(), writable: false);

        using SynchronousZLibReader zlib = new(input);

        byte[] decoded = new byte[decodedLength];

        try
        {
            zlib.ReadExactly(decoded);
        }
        catch (EndOfStreamException exception)
        {
            throw new InvalidDataException(message: "Compressed login session token ended before its declared decoded length.", exception);
        }

        if (zlib.ReadByte() is not -1)
            throw new InvalidDataException(message: "Compressed login session token exceeds its declared decoded length.");

        uint expectedChecksum = BinaryPrimitives.ReadUInt32BigEndian(wireData.Span[^4..]);

        if (CalculateAdler32(decoded) != expectedChecksum)
            throw new InvalidDataException(message: "Compressed login session token has an invalid zlib checksum.");

        string value;

        try
        {
            value = StrictUnicodeTransformationFormat8.GetString(decoded);
        }
        catch (DecoderFallbackException exception)
        {
            throw new InvalidDataException(message: "Login session token is not valid UTF-8.", exception);
        }

        return Parse(value, wireData.ToArray());
    }

    /// <summary>
    /// Decodes JWT text or the login protocol's empty-string marker.
    /// </summary>
    public static LoginSessionToken Decode(string value)
    {
        return string.IsNullOrWhiteSpace(value) && value is not ""
            ? throw new InvalidDataException(message: "Login session token is empty.")
            : Parse(value, Compress(StrictUnicodeTransformationFormat8.GetBytes(value)));
    }

    /// <summary>Encodes the decoded token as the login protocol's length-prefixed zlib payload.</summary>
    public byte[] Encode()
    {
        return [.. _wireData];
    }

    /// <summary>
    /// Returns whether a nonempty token has reached its refresh time.
    /// </summary>
    public bool ShouldRefresh(TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        if (IssuedAt is not { } issuedAt || ExpiresAt is not { } expiresAt)
            return false;

        long lifetime = expiresAt - issuedAt;

        if (lifetime <= 0)
            return true;

        long refreshAt = expiresAt - (lifetime / 8);

        return timeProvider.GetUtcNow().ToUnixTimeSeconds() >= refreshAt;
    }

    /// <summary>
    /// Provides the To String value or operation.
    /// </summary>
    public override string ToString()
    {
        return nameof(LoginSessionToken);
    }

    private static uint CalculateAdler32(ReadOnlySpan<byte> data)
    {
        uint first = 1;
        uint second = 0;

        foreach (byte value in data)
        {
            first = (first + value) % AdlerModulus;
            second = (second + first) % AdlerModulus;
        }

        return (second << 16) | first;
    }

    private static byte[] Compress(byte[] decoded)
    {
        // ZLibStream emits nothing for empty input; include the length prefix and a complete empty zlib stream.
        if (decoded.Length is 0)
            return [0, 0, 0, 0, 0x78, 0x9C, 0x03, 0, 0, 0, 0, 1];

        using MemoryStream output = new();

        Span<byte> length = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32LittleEndian(length, decoded.Length);
        output.Write(length);

        using (SynchronousZLibWriter zlib = new(output))
            zlib.Write(decoded);

        return output.ToArray();
    }

    private static byte[] DecodeBase64Address(string value, string name)
    {
        if (value.Any(static character => !(char.IsAsciiLetterOrDigit(character) || character is '-' or '_')))
            throw new InvalidDataException($"Login session token {name} is not valid unpadded Base64URL.");

        int padding = (4 - (value.Length % 4)) % 4;
        string encoded = value.Replace(oldChar: '-', newChar: '+').Replace(oldChar: '_', newChar: '/') + new string(c: '=', padding);

        try
        {
            return Convert.FromBase64String(encoded);
        }
        catch (FormatException exception)
        {
            throw new InvalidDataException($"Login session token {name} is not valid Base64URL.", exception);
        }
    }

    private static LoginSessionToken Parse(string value, byte[] wireData)
    {
        if (value.Length is 0)
            return new LoginSessionToken(value, wireData, header: null, claims: null);

        string[] segments = value.Split(separator: '.');

        if (segments.Length is not 3 || segments.Any(string.IsNullOrEmpty))
            throw new InvalidDataException(message: "Login session token is not a compact three-part JWT.");

        LoginSessionHeader header = ParseSegment<LoginSessionHeader>(segments[0], name: "header");
        LoginSessionClaims claims = ParseSegment<LoginSessionClaims>(segments[1], name: "claims");
        byte[] signature = DecodeBase64Address(segments[2], name: "signature");

        return signature.Length is not 64
            ? throw new InvalidDataException(message: "Login session token has an invalid ES256 signature.")
            : !string.Equals(header.Algorithm, b: "ES256", StringComparison.Ordinal)
            ? throw new InvalidDataException(message: "Login session token does not use ES256.")
            : claims.ExpiresAt <= claims.IssuedAt
            ? throw new InvalidDataException(message: "Login session token expiration does not follow its issue time.")
            : new LoginSessionToken(value, wireData, header, claims);
    }

    private static TValue ParseSegment<TValue>(string segment, string name) where TValue : class
    {
        try
        {
            return JsonSerializer.Deserialize<TValue>(DecodeBase64Address(segment, name))
                ?? throw new InvalidDataException($"Login session token {name} is null.");
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException($"Login session token {name} is not valid JSON.", exception);
        }
    }

    private sealed class SynchronousZLibReader(Stream input) : IDisposable
    {
        private readonly ZLibStream _stream = new(input, CompressionMode.Decompress);

        public void Dispose()
        {
            _stream.Dispose();
        }

        public int ReadByte()
        {
            return _stream.ReadByte();
        }

        public void ReadExactly(byte[] buffer)
        {
            _stream.ReadExactly(buffer);
        }
    }

    private sealed class SynchronousZLibWriter(Stream output) : IDisposable
    {
        private readonly ZLibStream _stream = new(output, CompressionLevel.Optimal, leaveOpen: true);

        public void Dispose()
        {
            _stream.Dispose();
        }

        public void Write(byte[] buffer)
        {
            _stream.Write(buffer);
        }
    }
}
