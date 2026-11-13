using System.Buffers.Binary;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace SupercellProxy.Networking.Sessions;

/// <summary>
/// Represents a decoded ES256 session token or the login protocol's empty-string marker.
/// </summary>
public sealed class SessionTokenData
{
    private const uint AdlerModulus = 65_521;

    private static readonly UTF8Encoding StrictUnicodeTransformationFormat8 = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    private readonly byte[] _wireData;

    private SessionTokenData(string value, byte[] wireData, SessionTokenHeader? header, SessionTokenPayload? payload)
    {
        Token = value;
        _wireData = wireData;
        Header = header;
        Payload = payload;
    }

    /// <summary>
    /// Gets the token's expiration time in Unix seconds, or <see langword="null"/> for an empty-string marker.
    /// </summary>
    /// <value>The payload's <c>exp</c> claim, always later than <see cref="Iat"/> for a decoded token; once it has passed the token no longer authenticates.</value>
    public long? Exp => Payload?.Exp;

    /// <summary>Gets the decoded JWT header, or <see langword="null"/> for the empty-string marker.</summary>
    /// <value>The first JWT segment, whose algorithm decoding has already checked to be ES256.</value>
    public SessionTokenHeader? Header { get; }

    /// <summary>
    /// Gets the token's issue time in Unix seconds, or <see langword="null"/> for an empty-string marker.
    /// </summary>
    /// <value>The payload's <c>iat</c> claim; together with <see cref="Exp"/> it sets the token lifetime that <see cref="ShouldRefresh"/> works from.</value>
    public long? Iat => Payload?.Iat;

    /// <summary>
    /// Gets the Unix seconds when the Supercell ID sign-in that this token descends from first issued its refresh token; it stays the same
    /// across refreshes. <see langword="null"/> for an empty-string marker or when the token does not carry the claim.
    /// </summary>
    /// <value>The payload's <c>initialRefreshTokenIssuedAt</c> claim, which tells how old the underlying sign-in is regardless of how recently this token was refreshed.</value>
    public long? InitialRefreshTokenIssuedAt => Payload?.InitialRefreshTokenIssuedAt;

    /// <summary>
    /// Gets whether the login field contains an empty-string marker with no session token.
    /// </summary>
    /// <value><see langword="true"/> if <see cref="Token"/> is the empty string, in which case <see cref="Header"/> and <see cref="Payload"/> are <see langword="null"/>; otherwise, <see langword="false"/>.</value>
    public bool IsEmpty => Token.Length is 0;

    /// <summary>
    /// Gets the decoded session payload, or <see langword="null"/> for the empty-string marker.
    /// </summary>
    /// <value>Claims parsed from the second JWT segment, such as the token type, account ids and lifetime; the signature is checked for length only, not verified.</value>
    public SessionTokenPayload? Payload { get; }

    /// <summary>
    /// Gets the decoded JWT text, or an empty string when no session token is supplied.
    /// </summary>
    /// <value>The compact <c>header.payload.signature</c> text exactly as it was decoded or given; it is a secret and should not be logged.</value>
    public string Token { get; }

    /// <summary>
    /// Decodes a JWT or an empty-string marker from the login protocol's wire representation.
    /// </summary>
    /// <param name="wireData">The login field's bytes: a 4-byte little-endian decoded length followed by a zlib stream of the UTF-8 token text.</param>
    /// <returns>The parsed token, which keeps <paramref name="wireData"/> so <see cref="Encode"/> reproduces it byte for byte; an empty-string marker when the decoded text is empty.</returns>
    /// <exception cref="InvalidDataException">The data is shorter than 10 bytes, declares a negative length, inflates to more or fewer bytes than declared, fails its Adler-32 check, is not valid UTF-8, or is not a well-formed ES256 JWT whose expiry follows its issue time.</exception>
    public static SessionTokenData Decode(ReadOnlyMemory<byte> wireData)
    {
        if (wireData.Length < sizeof(int) + 6)
            throw new InvalidDataException(message: "Compressed session token has no declared decoded length.");

        int decodedLength = BinaryPrimitives.ReadInt32LittleEndian(wireData.Span);

        if (decodedLength < 0)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Compressed session token has invalid decoded length {decodedLength}."));

        using MemoryStream input = new(wireData[sizeof(int)..].ToArray(), writable: false);

        using SynchronousZLibReader zlib = new(input);

        byte[] decoded = new byte[decodedLength];

        try
        {
            zlib.ReadExactly(decoded);
        }
        catch (EndOfStreamException exception)
        {
            throw new InvalidDataException(message: "Compressed session token ended before its declared decoded length.", exception);
        }

        if (zlib.ReadByte() is not -1)
            throw new InvalidDataException(message: "Compressed session token exceeds its declared decoded length.");

        uint expectedChecksum = BinaryPrimitives.ReadUInt32BigEndian(wireData.Span[^4..]);

        if (CalculateAdler32(decoded) != expectedChecksum)
            throw new InvalidDataException(message: "Compressed session token has an invalid zlib checksum.");

        string value;

        try
        {
            value = StrictUnicodeTransformationFormat8.GetString(decoded);
        }
        catch (DecoderFallbackException exception)
        {
            throw new InvalidDataException(message: "Session token is not valid UTF-8.", exception);
        }

        return Parse(value, wireData.ToArray());
    }

    /// <summary>
    /// Decodes JWT text or the login protocol's empty-string marker.
    /// </summary>
    /// <param name="value">Compact JWT text, or the empty string for the marker; text made only of white space is not accepted.</param>
    /// <returns>The parsed token together with a freshly compressed wire form for <see cref="Encode"/>; an empty-string marker for <c>""</c>.</returns>
    /// <exception cref="InvalidDataException"><paramref name="value"/> is non-empty white space, or is not a three-part JWT with Base64URL segments, JSON header and payload, a 64-byte signature, the ES256 algorithm and an expiry after its issue time.</exception>
    public static SessionTokenData Decode(string value)
    {
        return string.IsNullOrWhiteSpace(value) && value is not ""
            ? throw new InvalidDataException(message: "Session token is empty.")
            : Parse(value, Compress(StrictUnicodeTransformationFormat8.GetBytes(value)));
    }

    /// <summary>Rejects callback data that cannot authenticate a session.</summary>
    /// <param name="token">Token returned by a session-token provider, or <see langword="null"/>, which is rejected.</param>
    /// <param name="timeProvider">Clock the expiry is compared against.</param>
    /// <exception cref="ArgumentNullException"><paramref name="timeProvider"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidDataException"><paramref name="token"/> is <see langword="null"/>, is the empty-string marker, has a token type other than <c>session</c>, or has already expired.</exception>
    public static void ValidateAuthentication(SessionTokenData? token, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        if (token?.Payload is not { Type: "session" } payload || payload.Exp <= timeProvider.GetUtcNow().ToUnixTimeSeconds())
            throw new InvalidDataException(message: "Authentication requires a current session token.");
    }

    /// <summary>Encodes the decoded token as the login protocol's length-prefixed zlib payload.</summary>
    /// <returns>A new copy of the wire bytes: the received bytes for a token read from the wire, or the compressed form built when it was decoded from text.</returns>
    public byte[] Encode()
    {
        return [.. _wireData];
    }

    /// <summary>
    /// Returns whether a nonempty token has reached its refresh time.
    /// </summary>
    /// <param name="timeProvider">Clock that supplies the current time.</param>
    /// <returns><see langword="true"/> if the token is not the empty-string marker and either the last eighth of its lifetime (from <see cref="Iat"/> to <see cref="Exp"/>) has begun or that lifetime is not positive; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="timeProvider"/> is <see langword="null"/>.</exception>
    public bool ShouldRefresh(TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        if (Iat is not { } issuedAt || Exp is not { } expiresAt)
            return false;

        long lifetime = expiresAt - issuedAt;

        if (lifetime <= 0)
            return true;

        long refreshAt = expiresAt - (lifetime / 8);

        return timeProvider.GetUtcNow().ToUnixTimeSeconds() >= refreshAt;
    }

    /// <summary>
    /// Returns only the type name, so a token that ends up in a log or exception message never leaks its secret text.
    /// </summary>
    /// <returns>The constant text <c>SessionTokenData</c>.</returns>
    public override string ToString()
    {
        return nameof(SessionTokenData);
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
            throw new InvalidDataException($"Session token {name} is not valid unpadded Base64URL.");

        int padding = (4 - (value.Length % 4)) % 4;
        string encoded = value.Replace(oldChar: '-', newChar: '+').Replace(oldChar: '_', newChar: '/') + new string(c: '=', padding);

        try
        {
            return Convert.FromBase64String(encoded);
        }
        catch (FormatException exception)
        {
            throw new InvalidDataException($"Session token {name} is not valid Base64URL.", exception);
        }
    }

    private static SessionTokenData Parse(string value, byte[] wireData)
    {
        if (value.Length is 0)
            return new SessionTokenData(value, wireData, header: null, payload: null);

        string[] segments = value.Split(separator: '.');

        if (segments.Length is not 3 || segments.Any(string.IsNullOrEmpty))
            throw new InvalidDataException(message: "Session token is not a compact three-part JWT.");

        SessionTokenHeader header = Parse<SessionTokenHeader>(segments[0], name: "header");
        SessionTokenPayload payload = Parse<SessionTokenPayload>(segments[1], name: "payload");
        byte[] signature = DecodeBase64Address(segments[2], name: "signature");

        return signature.Length is not 64
            ? throw new InvalidDataException(message: "Session token has an invalid ES256 signature.")
            : !string.Equals(header.Alg, b: "ES256", StringComparison.Ordinal)
            ? throw new InvalidDataException(message: "Session token does not use ES256.")
            : payload.Exp <= payload.Iat
            ? throw new InvalidDataException(message: "Session token expiration does not follow its issue time.")
            : new SessionTokenData(value, wireData, header, payload);
    }

    private static TValue Parse<TValue>(string segment, string name) where TValue : class
    {
        try
        {
            return JsonSerializer.Deserialize<TValue>(DecodeBase64Address(segment, name))
                ?? throw new InvalidDataException($"Session token {name} is null.");
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException($"Session token {name} is not valid JSON.", exception);
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
