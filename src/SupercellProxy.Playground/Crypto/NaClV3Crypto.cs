using System.Buffers.Binary;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using SupercellProxy.Playground.Crypto.Exceptions;

namespace SupercellProxy.Playground.Crypto;

/// <summary>
/// Represents <c>NaClV3Crypto</c>.
/// </summary>
public static class NaClV3Crypto
{
    private static ReadOnlySpan<uint> SigmaConstants =>
        [0x61707865, 0x3320646e, 0x79622d32, 0x6b206574];
    private const int HChaChaIterationsCount = 17;
    private const int ChaChaIterationsCount = 8;

    private static ReadOnlySpan<byte> Curve25519BasePoint =>
        [
            9,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
        ];

    private static readonly BigInteger Poly1305ClampConstant = BigInteger.Parse(
        "0ffffffc0ffffffc0ffffffc0fffffff",
        NumberStyles.AllowHexSpecifier,
        System.Globalization.CultureInfo.InvariantCulture
    );
    private static readonly BigInteger Poly1305PrimeModulus = (BigInteger.One << 130) - 5;
    private static readonly BigInteger Curve25519PrimeModulus = BigInteger.Pow(2, 255) - 19;
    private static readonly BigInteger CurveConstantA24 = new(121665);

    /// <summary>
    /// Executes the <c>CryptoScalarMultBase</c> operation.
    /// </summary>
    public static byte[] CryptoScalarMultBase(ReadOnlySpan<byte> localPrivateKey)
    {
        var result = new byte[32];
        CryptoScalarMult(localPrivateKey, Curve25519BasePoint, result);
        return result;
    }

    /// <summary>
    /// Executes the <c>HChaCha20</c> operation.
    /// </summary>
    public static byte[] HChaCha20(ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce16)
    {
        var result = new byte[32];
        HChaCha20(key, nonce16, result);
        return result;
    }

    /// <summary>
    /// Executes the <c>Box</c> operation.
    /// </summary>
    public static byte[] Box(
        ReadOnlySpan<byte> plaintext,
        ReadOnlySpan<byte> nonce24,
        ReadOnlySpan<byte> remotePublicKey,
        ReadOnlySpan<byte> localPrivateKey
    )
    {
        var sharedSecret = (stackalloc byte[32]);
        CryptoScalarMult(localPrivateKey, remotePublicKey, sharedSecret);

        var beforeNm = (stackalloc byte[32]);
        HChaCha20(sharedSecret, (stackalloc byte[16]), beforeNm);

        var subKey = (stackalloc byte[32]);
        HChaCha20(beforeNm, nonce24[..16], subKey);

        var finalResult = new byte[16 + plaintext.Length];
        var messageAuthenticationCode = finalResult.AsSpan(0, 16);
        var encryptedPayload = finalResult.AsSpan(16);
        var polyKey = (stackalloc byte[32]);

        ChaCha20XorPadded(subKey, nonce24.Slice(16, 8), plaintext, encryptedPayload, polyKey);
        Poly1305(encryptedPayload, polyKey, messageAuthenticationCode);

        return finalResult;
    }

    /// <summary>
    /// Executes the <c>BoxOpen</c> operation.
    /// </summary>
    public static byte[] BoxOpen(
        ReadOnlySpan<byte> ciphertext,
        ReadOnlySpan<byte> nonce24,
        ReadOnlySpan<byte> remotePublicKey,
        ReadOnlySpan<byte> localPrivateKey
    )
    {
        var messageAuthenticationCode = ciphertext[..16];
        var encryptedPayload = ciphertext[16..];

        var sharedSecret = (stackalloc byte[32]);
        CryptoScalarMult(localPrivateKey, remotePublicKey, sharedSecret);

        var beforeNm = (stackalloc byte[32]);
        HChaCha20(sharedSecret, (stackalloc byte[16]), beforeNm);

        var subKey = (stackalloc byte[32]);
        HChaCha20(beforeNm, nonce24[..16], subKey);

        var polyKey = (stackalloc byte[32]);
        var keyStreamBlockSpan = (stackalloc byte[64]);
        ChaCha20Block(subKey, 0, nonce24.Slice(16, 8), keyStreamBlockSpan);
        keyStreamBlockSpan[..32].CopyTo(polyKey);

        var expectedMessageAuthenticationCode = (stackalloc byte[16]);
        Poly1305(encryptedPayload, polyKey, expectedMessageAuthenticationCode);

        if (
            !CryptographicOperations.FixedTimeEquals(
                expectedMessageAuthenticationCode,
                messageAuthenticationCode
            )
        )
            throw new MacVerificationException(isPublicKeyBox: true, "MAC verification failed");

        var fullDecrypted = new byte[encryptedPayload.Length];
        ChaCha20XorPadded(
            subKey,
            nonce24.Slice(16, 8),
            encryptedPayload,
            fullDecrypted,
            (stackalloc byte[32])
        );

        return fullDecrypted;
    }

    /// <summary>
    /// Executes the <c>SecretBox</c> operation.
    /// </summary>
    public static byte[] SecretBox(
        ReadOnlySpan<byte> plaintext,
        ReadOnlySpan<byte> nonce24,
        ReadOnlySpan<byte> key
    )
    {
        var subKey = (stackalloc byte[32]);
        HChaCha20(key, nonce24[..16], subKey);

        var finalResult = new byte[16 + plaintext.Length];
        var messageAuthenticationCode = finalResult.AsSpan(0, 16);
        var encryptedPayload = finalResult.AsSpan(16);
        var polyKey = (stackalloc byte[32]);

        ChaCha20XorPadded(subKey, nonce24.Slice(16, 8), plaintext, encryptedPayload, polyKey);
        Poly1305(encryptedPayload, polyKey, messageAuthenticationCode);

        return finalResult;
    }

    /// <summary>
    /// Executes the <c>SecretBoxOpen</c> operation.
    /// </summary>
    public static byte[] SecretBoxOpen(
        ReadOnlySpan<byte> ciphertext,
        ReadOnlySpan<byte> nonce24,
        ReadOnlySpan<byte> key
    )
    {
        var messageAuthenticationCode = ciphertext[..16];
        var encryptedPayload = ciphertext[16..];

        var subKey = (stackalloc byte[32]);
        HChaCha20(key, nonce24[..16], subKey);

        var polyKey = (stackalloc byte[32]);
        var keyStreamBlockSpan = (stackalloc byte[64]);
        ChaCha20Block(subKey, 0, nonce24.Slice(16, 8), keyStreamBlockSpan);
        keyStreamBlockSpan[..32].CopyTo(polyKey);

        var expectedMessageAuthenticationCode = (stackalloc byte[16]);
        Poly1305(encryptedPayload, polyKey, expectedMessageAuthenticationCode);

        if (
            !CryptographicOperations.FixedTimeEquals(
                expectedMessageAuthenticationCode,
                messageAuthenticationCode
            )
        )
            throw new MacVerificationException("MAC verification failed");

        var fullDecrypted = new byte[encryptedPayload.Length];
        ChaCha20XorPadded(
            subKey,
            nonce24.Slice(16, 8),
            encryptedPayload,
            fullDecrypted,
            (stackalloc byte[32])
        );

        return fullDecrypted;
    }

    private static void QuarterRound(
        Span<uint> span,
        int indexA,
        int indexB,
        int indexC,
        int indexD
    )
    {
        span[indexA] = unchecked(span[indexA] + span[indexB]);
        span[indexD] ^= span[indexA];
        span[indexD] = BitOperations.RotateLeft(span[indexD], 16);

        span[indexC] = unchecked(span[indexC] + span[indexD]);
        span[indexB] ^= span[indexC];
        span[indexB] = BitOperations.RotateLeft(span[indexB], 12);

        span[indexA] = unchecked(span[indexA] + span[indexB]);
        span[indexD] ^= span[indexA];
        span[indexD] = BitOperations.RotateLeft(span[indexD], 8);

        span[indexC] = unchecked(span[indexC] + span[indexD]);
        span[indexB] ^= span[indexC];
        span[indexB] = BitOperations.RotateLeft(span[indexB], 7);
    }

    private static void DoubleRounds(Span<uint> span, int iterationsCount)
    {
        for (var currentIteration = 0; currentIteration < iterationsCount; currentIteration++)
        {
            QuarterRound(span, 0, 4, 8, 12);
            QuarterRound(span, 1, 5, 9, 13);
            QuarterRound(span, 2, 6, 10, 14);
            QuarterRound(span, 3, 7, 11, 15);

            QuarterRound(span, 0, 5, 10, 15);
            QuarterRound(span, 1, 6, 11, 12);
            QuarterRound(span, 2, 7, 8, 13);
            QuarterRound(span, 3, 4, 9, 14);
        }
    }

    private static void HChaCha20(
        ReadOnlySpan<byte> key,
        ReadOnlySpan<byte> nonce16,
        Span<byte> result
    )
    {
        var stateSpan = (stackalloc uint[16]);
        SigmaConstants.CopyTo(stateSpan[..4]);

        for (var keyIndex = 0; keyIndex < 8; keyIndex++)
            stateSpan[4 + keyIndex] = BinaryPrimitives.ReadUInt32LittleEndian(
                key.Slice(keyIndex * 4, 4)
            );

        for (var nonceIndex = 0; nonceIndex < 4; nonceIndex++)
            stateSpan[12 + nonceIndex] = BinaryPrimitives.ReadUInt32LittleEndian(
                nonce16.Slice(nonceIndex * 4, 4)
            );

        DoubleRounds(stateSpan, HChaChaIterationsCount);

        for (var resultIndex = 0; resultIndex < 4; resultIndex++)
            BinaryPrimitives.WriteUInt32LittleEndian(
                result.Slice(resultIndex * 4, 4),
                stateSpan[resultIndex]
            );

        for (var resultIndex = 0; resultIndex < 4; resultIndex++)
            BinaryPrimitives.WriteUInt32LittleEndian(
                result.Slice(16 + resultIndex * 4, 4),
                stateSpan[12 + resultIndex]
            );
    }

    private static void ChaCha20Block(
        ReadOnlySpan<byte> key,
        long counterValue,
        ReadOnlySpan<byte> nonce8,
        Span<byte> output
    )
    {
        var initialStateSpan = (stackalloc uint[16]);
        SigmaConstants.CopyTo(initialStateSpan[..4]);

        for (var keyIndex = 0; keyIndex < 8; keyIndex++)
            initialStateSpan[4 + keyIndex] = BinaryPrimitives.ReadUInt32LittleEndian(
                key.Slice(keyIndex * 4, 4)
            );

        initialStateSpan[12] = uint.CreateTruncating((counterValue & 0xFFFFFFFF));
        initialStateSpan[13] = uint.CreateTruncating(((counterValue >> 32) & 0xFFFFFFFF));

        for (var nonceIndex = 0; nonceIndex < 2; nonceIndex++)
            initialStateSpan[14 + nonceIndex] = BinaryPrimitives.ReadUInt32LittleEndian(
                nonce8.Slice(nonceIndex * 4, 4)
            );

        var workingStateSpan = (stackalloc uint[16]);
        initialStateSpan.CopyTo(workingStateSpan);
        DoubleRounds(workingStateSpan, ChaChaIterationsCount);

        for (var blockIndex = 0; blockIndex < 16; blockIndex++)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(
                output.Slice(blockIndex * 4, 4),
                unchecked(workingStateSpan[blockIndex] + initialStateSpan[blockIndex])
            );
        }
    }

    private static void ChaCha20XorPadded(
        ReadOnlySpan<byte> key,
        ReadOnlySpan<byte> nonce8,
        ReadOnlySpan<byte> input,
        Span<byte> output,
        Span<byte> polyKey
    )
    {
        var keyStreamBlockSpan = (stackalloc byte[64]);
        ChaCha20Block(key, 0, nonce8, keyStreamBlockSpan);

        keyStreamBlockSpan[..32].CopyTo(polyKey);

        var inputOffsetIndex = 0;
        var bytesToProcessCount = Math.Min(32, input.Length);

        for (var byteIndex = 0; byteIndex < bytesToProcessCount; byteIndex++)
            output[byteIndex] = byte.CreateTruncating(
                (input[byteIndex] ^ keyStreamBlockSpan[32 + byteIndex])
            );

        inputOffsetIndex += bytesToProcessCount;
        var currentBlockIndex = 1L;

        while (inputOffsetIndex < input.Length)
        {
            ChaCha20Block(key, currentBlockIndex, nonce8, keyStreamBlockSpan);
            bytesToProcessCount = Math.Min(64, input.Length - inputOffsetIndex);

            for (var byteIndex = 0; byteIndex < bytesToProcessCount; byteIndex++)
                output[inputOffsetIndex + byteIndex] = byte.CreateTruncating(
                    (input[inputOffsetIndex + byteIndex] ^ keyStreamBlockSpan[byteIndex])
                );

            inputOffsetIndex += bytesToProcessCount;
            currentBlockIndex++;
        }
    }

    private static void Poly1305(
        ReadOnlySpan<byte> message,
        ReadOnlySpan<byte> key,
        Span<byte> macResult
    )
    {
        var clampedKeyBigInt =
            new BigInteger(key[..16], isUnsigned: true, isBigEndian: false) & Poly1305ClampConstant;
        var secretStateBigInt = new BigInteger(
            key.Slice(16, 16),
            isUnsigned: true,
            isBigEndian: false
        );
        var accumulatorBigInt = BigInteger.Zero;

        for (var chunkStartIndex = 0; chunkStartIndex < message.Length; chunkStartIndex += 16)
        {
            var currentChunkLength = Math.Min(16, message.Length - chunkStartIndex);
            var chunkValueBigInt =
                new BigInteger(
                    message.Slice(chunkStartIndex, currentChunkLength),
                    isUnsigned: true,
                    isBigEndian: false
                ) + (BigInteger.One << (8 * currentChunkLength));

            accumulatorBigInt =
                ((accumulatorBigInt + chunkValueBigInt) * clampedKeyBigInt) % Poly1305PrimeModulus;
        }

        var finalCalculatedMac =
            (accumulatorBigInt + secretStateBigInt) & ((BigInteger.One << 128) - 1);

        macResult.Clear();
        finalCalculatedMac.TryWriteBytes(macResult, out _, isUnsigned: true, isBigEndian: false);
    }

    private static void CryptoScalarMult(
        ReadOnlySpan<byte> localPrivateKey,
        ReadOnlySpan<byte> remotePublicKey,
        Span<byte> result
    )
    {
        var clampedPrivateKey = (stackalloc byte[32]);
        localPrivateKey[..32].CopyTo(clampedPrivateKey);
        clampedPrivateKey[0] &= 248;
        clampedPrivateKey[31] &= 127;
        clampedPrivateKey[31] |= 64;

        var scalarValue = new BigInteger(clampedPrivateKey, isUnsigned: true, isBigEndian: false);

        var clampedPublicKey = (stackalloc byte[32]);
        remotePublicKey[..32].CopyTo(clampedPublicKey);
        clampedPublicKey[31] &= 127;
        var baseXCoordinate = new BigInteger(
            clampedPublicKey,
            isUnsigned: true,
            isBigEndian: false
        );

        var (xCoordinate2, zCoordinate2) = RunMontgomeryLadder(scalarValue, baseXCoordinate);

        var zCoordinate2Inverse = BigInteger.ModPow(
            zCoordinate2,
            Curve25519PrimeModulus - 2,
            Curve25519PrimeModulus
        );
        var finalXCoordinate = Modulo(xCoordinate2 * zCoordinate2Inverse, Curve25519PrimeModulus);

        finalXCoordinate.TryWriteBytes(result, out _, isUnsigned: true, isBigEndian: false);
    }

    private static (BigInteger XCoordinate, BigInteger ZCoordinate) RunMontgomeryLadder(
        BigInteger scalarValue,
        BigInteger baseXCoordinate
    )
    {
        var xCoordinate2 = BigInteger.One;
        var zCoordinate2 = BigInteger.Zero;
        var xCoordinate3 = baseXCoordinate;
        var zCoordinate3 = BigInteger.One;
        var swapFlag = 0;

        for (var bitIndex = 254; bitIndex >= 0; bitIndex--)
        {
            var currentBit = int.CreateTruncating((scalarValue >> bitIndex) & 1);
            swapFlag ^= currentBit;

            if (swapFlag is not 0)
            {
                (xCoordinate2, xCoordinate3) = (xCoordinate3, xCoordinate2);
                (zCoordinate2, zCoordinate3) = (zCoordinate3, zCoordinate2);
            }

            swapFlag = currentBit;
            (xCoordinate2, zCoordinate2, xCoordinate3, zCoordinate3) = MontgomeryStep(
                baseXCoordinate,
                xCoordinate2,
                zCoordinate2,
                xCoordinate3,
                zCoordinate3
            );
        }

        if (swapFlag is not 0)
        {
            (xCoordinate2, xCoordinate3) = (xCoordinate3, xCoordinate2);
            (zCoordinate2, zCoordinate3) = (zCoordinate3, zCoordinate2);
        }

        return (xCoordinate2, zCoordinate2);
    }

    private static (
        BigInteger XCoordinate2,
        BigInteger ZCoordinate2,
        BigInteger XCoordinate3,
        BigInteger ZCoordinate3
    ) MontgomeryStep(
        BigInteger baseXCoordinate,
        BigInteger xCoordinate2,
        BigInteger zCoordinate2,
        BigInteger xCoordinate3,
        BigInteger zCoordinate3
    )
    {
        var sumX2Z2 = Modulo(xCoordinate2 + zCoordinate2, Curve25519PrimeModulus);
        var squaredSumX2Z2 = Modulo(sumX2Z2 * sumX2Z2, Curve25519PrimeModulus);
        var diffX2Z2 = Modulo(xCoordinate2 - zCoordinate2, Curve25519PrimeModulus);
        var squaredDiffX2Z2 = Modulo(diffX2Z2 * diffX2Z2, Curve25519PrimeModulus);
        var diffSquaredSumAndDiff = Modulo(
            squaredSumX2Z2 - squaredDiffX2Z2,
            Curve25519PrimeModulus
        );

        var sumX3Z3 = Modulo(xCoordinate3 + zCoordinate3, Curve25519PrimeModulus);
        var diffX3Z3 = Modulo(xCoordinate3 - zCoordinate3, Curve25519PrimeModulus);

        var productDiffX3Z3AndSumX2Z2 = Modulo(diffX3Z3 * sumX2Z2, Curve25519PrimeModulus);
        var productSumX3Z3AndDiffX2Z2 = Modulo(sumX3Z3 * diffX2Z2, Curve25519PrimeModulus);

        var sumProducts = Modulo(
            productDiffX3Z3AndSumX2Z2 + productSumX3Z3AndDiffX2Z2,
            Curve25519PrimeModulus
        );
        xCoordinate3 = Modulo(sumProducts * sumProducts, Curve25519PrimeModulus);

        var diffProducts = Modulo(
            productDiffX3Z3AndSumX2Z2 - productSumX3Z3AndDiffX2Z2,
            Curve25519PrimeModulus
        );
        zCoordinate3 = Modulo(
            baseXCoordinate * Modulo(diffProducts * diffProducts, Curve25519PrimeModulus),
            Curve25519PrimeModulus
        );

        xCoordinate2 = Modulo(squaredSumX2Z2 * squaredDiffX2Z2, Curve25519PrimeModulus);
        zCoordinate2 = Modulo(
            diffSquaredSumAndDiff
                * Modulo(
                    squaredSumX2Z2
                        + Modulo(CurveConstantA24 * diffSquaredSumAndDiff, Curve25519PrimeModulus),
                    Curve25519PrimeModulus
                ),
            Curve25519PrimeModulus
        );
        return (xCoordinate2, zCoordinate2, xCoordinate3, zCoordinate3);
    }

    private static BigInteger Modulo(BigInteger value, BigInteger modulus)
    {
        var remainder = value % modulus;
        return remainder.Sign < 0 ? remainder + modulus : remainder;
    }
}
