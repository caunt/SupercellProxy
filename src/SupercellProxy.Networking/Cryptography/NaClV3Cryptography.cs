using System.Buffers.Binary;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;

namespace SupercellProxy.Networking.Cryptography;

/// <summary>
/// Represents <c language="csharp">NaClV3Cryptography</c>.
/// </summary>
public static class NaClV3Cryptography
{
    private const int ChaChaIterationsCount = 8;
    private const int HChaChaIterationsCount = 17;

    private static readonly BigInteger Poly1305ClampConstant = BigInteger.Parse(value: "0ffffffc0ffffffc0ffffffc0fffffff", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
    private static readonly BigInteger Poly1305PrimeModulus = (BigInteger.One << 130) - 5;
    private static readonly BigInteger Curve25519PrimeModulus = BigInteger.Pow(value: 2, exponent: 255) - 19;
    private static readonly BigInteger CurveConstantA24 = new(value: 121665);

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
    private static ReadOnlySpan<uint> SigmaConstants =>
        [0x61707865, 0x3320646e, 0x79622d32, 0x6b206574];

    /// <summary>
    /// Executes the <c language="csharp">Box</c> operation.
    /// </summary>
    public static byte[] Box(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> nonce24, ReadOnlySpan<byte> remotePublicKey, ReadOnlySpan<byte> localPrivateKey)
    {
        Span<byte> sharedSecret = stackalloc byte[32];
        CryptoScalarMult(localPrivateKey, remotePublicKey, sharedSecret);

        Span<byte> beforeNm = stackalloc byte[32];
        HChaCha20(sharedSecret, (stackalloc byte[16]), beforeNm);

        Span<byte> subKey = stackalloc byte[32];
        HChaCha20(beforeNm, nonce24[..16], subKey);

        byte[] finalResult = new byte[16 + plaintext.Length];
        Span<byte> messageAuthenticationCode = finalResult.AsSpan(start: 0, length: 16);
        Span<byte> encryptedPayload = finalResult.AsSpan(start: 16);
        Span<byte> polyKey = stackalloc byte[32];

        ChaCha20XorPadded(subKey, nonce24.Slice(start: 16, length: 8), plaintext, encryptedPayload, polyKey);
        Poly1305(encryptedPayload, polyKey, messageAuthenticationCode);

        return finalResult;
    }

    /// <summary>
    /// Executes the <c language="csharp">BoxOpen</c> operation.
    /// </summary>
    public static byte[] BoxOpen(
        ReadOnlySpan<byte> ciphertext,
        ReadOnlySpan<byte> nonce24,
        ReadOnlySpan<byte> remotePublicKey,
        ReadOnlySpan<byte> localPrivateKey
    )
    {
        ReadOnlySpan<byte> messageAuthenticationCode = ciphertext[..16];
        ReadOnlySpan<byte> encryptedPayload = ciphertext[16..];

        Span<byte> sharedSecret = stackalloc byte[32];
        CryptoScalarMult(localPrivateKey, remotePublicKey, sharedSecret);

        Span<byte> beforeNm = stackalloc byte[32];
        HChaCha20(sharedSecret, (stackalloc byte[16]), beforeNm);

        Span<byte> subKey = stackalloc byte[32];
        HChaCha20(beforeNm, nonce24[..16], subKey);

        Span<byte> polyKey = stackalloc byte[32];
        Span<byte> keyStreamBlockSpan = stackalloc byte[64];
        ChaCha20Block(subKey, counterValue: 0, nonce24.Slice(start: 16, length: 8), keyStreamBlockSpan);
        keyStreamBlockSpan[..32].CopyTo(polyKey);

        Span<byte> expectedMessageAuthenticationCode = stackalloc byte[16];
        Poly1305(encryptedPayload, polyKey, expectedMessageAuthenticationCode);

        if (!CryptographicOperations.FixedTimeEquals(expectedMessageAuthenticationCode, messageAuthenticationCode))
            throw new MacVerificationException(isPublicKeyBox: true, MacVerificationException.DefaultMessage);

        byte[] fullDecrypted = new byte[encryptedPayload.Length];
        ChaCha20XorPadded(subKey, nonce24.Slice(start: 16, length: 8), encryptedPayload, fullDecrypted, (stackalloc byte[32]));

        return fullDecrypted;
    }

    /// <summary>
    /// Executes the <c language="csharp">CryptoScalarMultBase</c> operation.
    /// </summary>
    public static byte[] CryptoScalarMultBase(ReadOnlySpan<byte> localPrivateKey)
    {
        byte[] result = new byte[32];
        CryptoScalarMult(localPrivateKey, Curve25519BasePoint, result);

        return result;
    }

    /// <summary>
    /// Executes the <c language="csharp">HChaCha20</c> operation.
    /// </summary>
    public static byte[] HChaCha20(ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce16)
    {
        byte[] result = new byte[32];
        HChaCha20(key, nonce16, result);

        return result;
    }

    /// <summary>
    /// Executes the <c language="csharp">SecretBox</c> operation.
    /// </summary>
    public static byte[] SecretBox(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> nonce24, ReadOnlySpan<byte> key)
    {
        Span<byte> subKey = stackalloc byte[32];
        HChaCha20(key, nonce24[..16], subKey);

        byte[] finalResult = new byte[16 + plaintext.Length];
        Span<byte> messageAuthenticationCode = finalResult.AsSpan(start: 0, length: 16);
        Span<byte> encryptedPayload = finalResult.AsSpan(start: 16);
        Span<byte> polyKey = stackalloc byte[32];

        ChaCha20XorPadded(subKey, nonce24.Slice(start: 16, length: 8), plaintext, encryptedPayload, polyKey);
        Poly1305(encryptedPayload, polyKey, messageAuthenticationCode);

        return finalResult;
    }

    /// <summary>
    /// Executes the <c language="csharp">SecretBoxOpen</c> operation.
    /// </summary>
    public static byte[] SecretBoxOpen(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> nonce24, ReadOnlySpan<byte> key)
    {
        ReadOnlySpan<byte> messageAuthenticationCode = ciphertext[..16];
        ReadOnlySpan<byte> encryptedPayload = ciphertext[16..];

        Span<byte> subKey = stackalloc byte[32];
        HChaCha20(key, nonce24[..16], subKey);

        Span<byte> polyKey = stackalloc byte[32];
        Span<byte> keyStreamBlockSpan = stackalloc byte[64];
        ChaCha20Block(subKey, counterValue: 0, nonce24.Slice(start: 16, length: 8), keyStreamBlockSpan);
        keyStreamBlockSpan[..32].CopyTo(polyKey);

        Span<byte> expectedMessageAuthenticationCode = stackalloc byte[16];
        Poly1305(encryptedPayload, polyKey, expectedMessageAuthenticationCode);

        if (!CryptographicOperations.FixedTimeEquals(expectedMessageAuthenticationCode, messageAuthenticationCode))
            throw new MacVerificationException(MacVerificationException.DefaultMessage);

        byte[] fullDecrypted = new byte[encryptedPayload.Length];
        ChaCha20XorPadded(subKey, nonce24.Slice(start: 16, length: 8), encryptedPayload, fullDecrypted, (stackalloc byte[32]));

        return fullDecrypted;
    }

    private static void ChaCha20Block(ReadOnlySpan<byte> key, long counterValue, ReadOnlySpan<byte> nonce8, Span<byte> output)
    {
        Span<uint> initialStateSpan = stackalloc uint[16];
        SigmaConstants.CopyTo(initialStateSpan[..4]);

        for (int keyIndex = 0; keyIndex < 8; keyIndex++)
            initialStateSpan[4 + keyIndex] = BinaryPrimitives.ReadUInt32LittleEndian(key.Slice(keyIndex * 4, length: 4));

        initialStateSpan[index: 12] = uint.CreateTruncating(counterValue & 0xFFFFFFFF);
        initialStateSpan[index: 13] = uint.CreateTruncating((counterValue >> 32) & 0xFFFFFFFF);

        for (int nonceIndex = 0; nonceIndex < 2; nonceIndex++)
            initialStateSpan[14 + nonceIndex] = BinaryPrimitives.ReadUInt32LittleEndian(nonce8.Slice(nonceIndex * 4, length: 4));

        Span<uint> workingStateSpan = stackalloc uint[16];
        initialStateSpan.CopyTo(workingStateSpan);
        DoubleRounds(workingStateSpan, ChaChaIterationsCount);

        for (int blockIndex = 0; blockIndex < 16; blockIndex++)
            BinaryPrimitives.WriteUInt32LittleEndian(output.Slice(blockIndex * 4, length: 4), unchecked(workingStateSpan[blockIndex] + initialStateSpan[blockIndex]));
    }

    private static void ChaCha20XorPadded(ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce8, ReadOnlySpan<byte> input, Span<byte> output, Span<byte> polyKey)
    {
        Span<byte> keyStreamBlockSpan = stackalloc byte[64];
        ChaCha20Block(key, counterValue: 0, nonce8, keyStreamBlockSpan);

        keyStreamBlockSpan[..32].CopyTo(polyKey);

        int inputOffsetIndex = 0;
        int bytesToProcessCount = Math.Min(val1: 32, input.Length);

        for (int byteIndex = 0; byteIndex < bytesToProcessCount; byteIndex++)
            output[byteIndex] = byte.CreateTruncating(input[byteIndex] ^ keyStreamBlockSpan[32 + byteIndex]);

        inputOffsetIndex += bytesToProcessCount;
        long currentBlockIndex = 1L;

        while (inputOffsetIndex < input.Length)
        {
            ChaCha20Block(key, currentBlockIndex, nonce8, keyStreamBlockSpan);
            bytesToProcessCount = Math.Min(val1: 64, input.Length - inputOffsetIndex);

            for (int byteIndex = 0; byteIndex < bytesToProcessCount; byteIndex++)
                output[inputOffsetIndex + byteIndex] = byte.CreateTruncating(input[inputOffsetIndex + byteIndex] ^ keyStreamBlockSpan[byteIndex]);

            inputOffsetIndex += bytesToProcessCount;
            currentBlockIndex++;
        }
    }

    private static void CryptoScalarMult(ReadOnlySpan<byte> localPrivateKey, ReadOnlySpan<byte> remotePublicKey, Span<byte> result)
    {
        Span<byte> clampedPrivateKey = stackalloc byte[32];
        localPrivateKey[..32].CopyTo(clampedPrivateKey);
        clampedPrivateKey[index: 0] &= 248;
        clampedPrivateKey[index: 31] &= 127;
        clampedPrivateKey[index: 31] |= 64;

        BigInteger scalarValue = new(clampedPrivateKey, isUnsigned: true, isBigEndian: false);

        Span<byte> clampedPublicKey = stackalloc byte[32];
        remotePublicKey[..32].CopyTo(clampedPublicKey);
        clampedPublicKey[index: 31] &= 127;

        BigInteger baseXCoordinate = new(clampedPublicKey, isUnsigned: true, isBigEndian: false);

        (BigInteger xCoordinate2, BigInteger zCoordinate2) = RunMontgomeryLadder(scalarValue, baseXCoordinate);

        BigInteger zCoordinate2Inverse = BigInteger.ModPow(zCoordinate2, Curve25519PrimeModulus - 2, Curve25519PrimeModulus);

        BigInteger finalXCoordinate = Modulo(xCoordinate2 * zCoordinate2Inverse, Curve25519PrimeModulus);

        if (!finalXCoordinate.TryWriteBytes(result, out _, isUnsigned: true, isBigEndian: false))
            throw new CryptographicException(message: "The shared-secret coordinate does not fit its output buffer.");
    }

    private static void DoubleRounds(Span<uint> span, int iterationsCount)
    {
        for (int currentIteration = 0; currentIteration < iterationsCount; currentIteration++)
        {
            QuarterRound(span, indexA: 0, indexB: 4, indexC: 8, indexD: 12);
            QuarterRound(span, indexA: 1, indexB: 5, indexC: 9, indexD: 13);
            QuarterRound(span, indexA: 2, indexB: 6, indexC: 10, indexD: 14);
            QuarterRound(span, indexA: 3, indexB: 7, indexC: 11, indexD: 15);

            QuarterRound(span, indexA: 0, indexB: 5, indexC: 10, indexD: 15);
            QuarterRound(span, indexA: 1, indexB: 6, indexC: 11, indexD: 12);
            QuarterRound(span, indexA: 2, indexB: 7, indexC: 8, indexD: 13);
            QuarterRound(span, indexA: 3, indexB: 4, indexC: 9, indexD: 14);
        }
    }

    private static void HChaCha20(ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce16, Span<byte> result)
    {
        Span<uint> stateSpan = stackalloc uint[16];
        SigmaConstants.CopyTo(stateSpan[..4]);

        for (int keyIndex = 0; keyIndex < 8; keyIndex++)
            stateSpan[4 + keyIndex] = BinaryPrimitives.ReadUInt32LittleEndian(key.Slice(keyIndex * 4, length: 4));

        for (int nonceIndex = 0; nonceIndex < 4; nonceIndex++)
            stateSpan[12 + nonceIndex] = BinaryPrimitives.ReadUInt32LittleEndian(nonce16.Slice(nonceIndex * 4, length: 4));

        DoubleRounds(stateSpan, HChaChaIterationsCount);

        for (int resultIndex = 0; resultIndex < 4; resultIndex++)
            BinaryPrimitives.WriteUInt32LittleEndian(result.Slice(resultIndex * 4, length: 4), stateSpan[resultIndex]);

        for (int resultIndex = 0; resultIndex < 4; resultIndex++)
            BinaryPrimitives.WriteUInt32LittleEndian(result.Slice(16 + (resultIndex * 4), length: 4), stateSpan[12 + resultIndex]);
    }

    private static BigInteger Modulo(BigInteger value, BigInteger modulus)
    {
        BigInteger remainder = value % modulus;

        return remainder.Sign < 0 ? remainder + modulus : remainder;
    }

    private static (
        BigInteger XCoordinate2,
        BigInteger ZCoordinate2,
        BigInteger XCoordinate3,
        BigInteger ZCoordinate3
    ) MontgomeryStep(BigInteger baseXCoordinate, BigInteger xCoordinate2, BigInteger zCoordinate2, BigInteger xCoordinate3, BigInteger zCoordinate3)
    {
        BigInteger sumX2Z2 = Modulo(xCoordinate2 + zCoordinate2, Curve25519PrimeModulus);
        BigInteger squaredSumX2Z2 = Modulo(sumX2Z2 * sumX2Z2, Curve25519PrimeModulus);
        BigInteger diffX2Z2 = Modulo(xCoordinate2 - zCoordinate2, Curve25519PrimeModulus);
        BigInteger squaredDiffX2Z2 = Modulo(diffX2Z2 * diffX2Z2, Curve25519PrimeModulus);

        BigInteger diffSquaredSumAndDiff = Modulo(squaredSumX2Z2 - squaredDiffX2Z2, Curve25519PrimeModulus);

        BigInteger sumX3Z3 = Modulo(xCoordinate3 + zCoordinate3, Curve25519PrimeModulus);
        BigInteger diffX3Z3 = Modulo(xCoordinate3 - zCoordinate3, Curve25519PrimeModulus);

        BigInteger productDiffX3Z3AndSumX2Z2 = Modulo(diffX3Z3 * sumX2Z2, Curve25519PrimeModulus);
        BigInteger productSumX3Z3AndDiffX2Z2 = Modulo(sumX3Z3 * diffX2Z2, Curve25519PrimeModulus);

        BigInteger sumProducts = Modulo(productDiffX3Z3AndSumX2Z2 + productSumX3Z3AndDiffX2Z2, Curve25519PrimeModulus);

        xCoordinate3 = Modulo(sumProducts * sumProducts, Curve25519PrimeModulus);

        BigInteger diffProducts = Modulo(productDiffX3Z3AndSumX2Z2 - productSumX3Z3AndDiffX2Z2, Curve25519PrimeModulus);

        zCoordinate3 = Modulo(baseXCoordinate * Modulo(diffProducts * diffProducts, Curve25519PrimeModulus), Curve25519PrimeModulus);

        xCoordinate2 = Modulo(squaredSumX2Z2 * squaredDiffX2Z2, Curve25519PrimeModulus);
        zCoordinate2 = Modulo(
            diffSquaredSumAndDiff
                * Modulo(squaredSumX2Z2 + Modulo(CurveConstantA24 * diffSquaredSumAndDiff, Curve25519PrimeModulus), Curve25519PrimeModulus),
            Curve25519PrimeModulus
        );

        return (xCoordinate2, zCoordinate2, xCoordinate3, zCoordinate3);
    }

    private static void Poly1305(ReadOnlySpan<byte> message, ReadOnlySpan<byte> key, Span<byte> macResult)
    {
        BigInteger clampedKeyBigInt =
            new BigInteger(key[..16], isUnsigned: true, isBigEndian: false) & Poly1305ClampConstant;

        BigInteger secretStateBigInt = new(key.Slice(start: 16, length: 16), isUnsigned: true, isBigEndian: false);

        BigInteger accumulatorBigInt = BigInteger.Zero;

        for (int chunkStartIndex = 0; chunkStartIndex < message.Length; chunkStartIndex += 16)
        {
            int currentChunkLength = Math.Min(val1: 16, message.Length - chunkStartIndex);

            BigInteger chunkValueBigInt =
                new BigInteger(message.Slice(chunkStartIndex, currentChunkLength), isUnsigned: true, isBigEndian: false) + (BigInteger.One << (8 * currentChunkLength));

            accumulatorBigInt =
                (accumulatorBigInt + chunkValueBigInt) * clampedKeyBigInt % Poly1305PrimeModulus;
        }

        BigInteger finalCalculatedMac =
            (accumulatorBigInt + secretStateBigInt) & ((BigInteger.One << 128) - 1);

        macResult.Clear();

        if (!finalCalculatedMac.TryWriteBytes(macResult, out _, isUnsigned: true, isBigEndian: false))
            throw new CryptographicException(message: "The message authentication code does not fit its output buffer.");
    }

    private static void QuarterRound(Span<uint> span, int indexA, int indexB, int indexC, int indexD)
    {
        span[indexA] = unchecked(span[indexA] + span[indexB]);
        span[indexD] ^= span[indexA];
        span[indexD] = BitOperations.RotateLeft(span[indexD], offset: 16);

        span[indexC] = unchecked(span[indexC] + span[indexD]);
        span[indexB] ^= span[indexC];
        span[indexB] = BitOperations.RotateLeft(span[indexB], offset: 12);

        span[indexA] = unchecked(span[indexA] + span[indexB]);
        span[indexD] ^= span[indexA];
        span[indexD] = BitOperations.RotateLeft(span[indexD], offset: 8);

        span[indexC] = unchecked(span[indexC] + span[indexD]);
        span[indexB] ^= span[indexC];
        span[indexB] = BitOperations.RotateLeft(span[indexB], offset: 7);
    }

    private static (BigInteger XCoordinate, BigInteger ZCoordinate) RunMontgomeryLadder(BigInteger scalarValue, BigInteger baseXCoordinate)
    {
        BigInteger xCoordinate2 = BigInteger.One;
        BigInteger zCoordinate2 = BigInteger.Zero;
        BigInteger xCoordinate3 = baseXCoordinate;
        BigInteger zCoordinate3 = BigInteger.One;
        int swapFlag = 0;

        for (int bitIndex = 254; bitIndex >= 0; bitIndex--)
        {
            int currentBit = int.CreateTruncating((scalarValue >> bitIndex) & 1);
            swapFlag ^= currentBit;

            if (swapFlag is not 0)
            {
                (xCoordinate2, xCoordinate3) = (xCoordinate3, xCoordinate2);
                (zCoordinate2, zCoordinate3) = (zCoordinate3, zCoordinate2);
            }

            swapFlag = currentBit;
            (xCoordinate2, zCoordinate2, xCoordinate3, zCoordinate3) = MontgomeryStep(baseXCoordinate, xCoordinate2, zCoordinate2, xCoordinate3, zCoordinate3);
        }

        if (swapFlag is not 0)
        {
            xCoordinate2 = xCoordinate3;
            zCoordinate2 = zCoordinate3;
        }

        return (xCoordinate2, zCoordinate2);
    }
}
