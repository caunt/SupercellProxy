using System.Buffers.Binary;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;

namespace SupercellProxy.Playground.Crypto;

public static class NaClV3Crypto
{
    private static ReadOnlySpan<uint> SigmaConstants => [0x61707865, 0x3320646e, 0x79622d32, 0x6b206574];
    private const int HChaChaIterationsCount = 17;
    private const int ChaChaIterationsCount = 8;

    private static ReadOnlySpan<byte> Curve25519BasePointData => [9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

    private static readonly BigInteger Poly1305ClampConstant = BigInteger.Parse("0ffffffc0ffffffc0ffffffc0fffffff", NumberStyles.AllowHexSpecifier);
    private static readonly BigInteger Poly1305PrimeModulus = (BigInteger.One << 130) - 5;
    private static readonly BigInteger Curve25519PrimeModulus = BigInteger.Pow(2, 255) - 19;
    private static readonly BigInteger CurveConstantA24 = new(121665);

    public static byte[] CryptoScalarMultBase(ReadOnlySpan<byte> secretKeyData)
    {
        var resultDataArray = new byte[32];
        CryptoScalarMult(secretKeyData, Curve25519BasePointData, resultDataArray);
        return resultDataArray;
    }

    public static byte[] HChaCha20(ReadOnlySpan<byte> keyData, ReadOnlySpan<byte> nonce16Data)
    {
        var resultDataArray = new byte[32];
        HChaCha20(keyData, nonce16Data, resultDataArray);
        return resultDataArray;
    }

    public static byte[] Box(ReadOnlySpan<byte> plainTextData, ReadOnlySpan<byte> nonce24Data, ReadOnlySpan<byte> serverPublicKeyData, ReadOnlySpan<byte> clientSecretKeyData)
    {
        var sharedSecretData = (stackalloc byte[32]);
        CryptoScalarMult(clientSecretKeyData, serverPublicKeyData, sharedSecretData);

        var beforeNmData = (stackalloc byte[32]);
        HChaCha20(sharedSecretData, (stackalloc byte[16]), beforeNmData);

        var subKeyData = (stackalloc byte[32]);
        HChaCha20(beforeNmData, nonce24Data[..16], subKeyData);

        var finalResultDataArray = new byte[16 + plainTextData.Length];
        var messageAuthenticationCodeData = finalResultDataArray.AsSpan(0, 16);
        var encryptedPayloadData = finalResultDataArray.AsSpan(16);
        var polyKeyData = (stackalloc byte[32]);

        ChaCha20XorPadded(subKeyData, nonce24Data.Slice(16, 8), plainTextData, encryptedPayloadData, polyKeyData);
        Poly1305(encryptedPayloadData, polyKeyData, messageAuthenticationCodeData);

        return finalResultDataArray;
    }

    public static byte[] BoxOpen(ReadOnlySpan<byte> cipherTextData, ReadOnlySpan<byte> nonce24Data, ReadOnlySpan<byte> serverPublicKeyData, ReadOnlySpan<byte> clientSecretKeyData)
    {
        var messageAuthenticationCodeData = cipherTextData[..16];
        var encryptedPayloadData = cipherTextData[16..];

        var sharedSecretData = (stackalloc byte[32]);
        CryptoScalarMult(clientSecretKeyData, serverPublicKeyData, sharedSecretData);

        var beforeNmData = (stackalloc byte[32]);
        HChaCha20(sharedSecretData, (stackalloc byte[16]), beforeNmData);

        var subKeyData = (stackalloc byte[32]);
        HChaCha20(beforeNmData, nonce24Data[..16], subKeyData);

        var polyKeyData = (stackalloc byte[32]);
        var keyStreamBlockSpan = (stackalloc byte[64]);
        ChaCha20Block(subKeyData, 0, nonce24Data.Slice(16, 8), keyStreamBlockSpan);
        keyStreamBlockSpan[..32].CopyTo(polyKeyData);

        var expectedMessageAuthenticationCodeData = (stackalloc byte[16]);
        Poly1305(encryptedPayloadData, polyKeyData, expectedMessageAuthenticationCodeData);

        if (!CryptographicOperations.FixedTimeEquals(expectedMessageAuthenticationCodeData, messageAuthenticationCodeData))
            throw new ArgumentException("MAC verification failed");

        var fullDecryptedDataArray = new byte[encryptedPayloadData.Length];
        ChaCha20XorPadded(subKeyData, nonce24Data.Slice(16, 8), encryptedPayloadData, fullDecryptedDataArray, (stackalloc byte[32]));

        return fullDecryptedDataArray;
    }

    public static byte[] SecretBox(ReadOnlySpan<byte> plainTextData, ReadOnlySpan<byte> nonce24Data, ReadOnlySpan<byte> keyData)
    {
        var subKeyData = (stackalloc byte[32]);
        HChaCha20(keyData, nonce24Data[..16], subKeyData);

        var finalResultDataArray = new byte[16 + plainTextData.Length];
        var messageAuthenticationCodeData = finalResultDataArray.AsSpan(0, 16);
        var encryptedPayloadData = finalResultDataArray.AsSpan(16);
        var polyKeyData = (stackalloc byte[32]);

        ChaCha20XorPadded(subKeyData, nonce24Data.Slice(16, 8), plainTextData, encryptedPayloadData, polyKeyData);
        Poly1305(encryptedPayloadData, polyKeyData, messageAuthenticationCodeData);

        return finalResultDataArray;
    }

    public static byte[] SecretBoxOpen(ReadOnlySpan<byte> cipherTextData, ReadOnlySpan<byte> nonce24Data, ReadOnlySpan<byte> keyData)
    {
        var messageAuthenticationCodeData = cipherTextData[..16];
        var encryptedPayloadData = cipherTextData[16..];

        var subKeyData = (stackalloc byte[32]);
        HChaCha20(keyData, nonce24Data[..16], subKeyData);

        var polyKeyData = (stackalloc byte[32]);
        var keyStreamBlockSpan = (stackalloc byte[64]);
        ChaCha20Block(subKeyData, 0, nonce24Data.Slice(16, 8), keyStreamBlockSpan);
        keyStreamBlockSpan[..32].CopyTo(polyKeyData);

        var expectedMessageAuthenticationCodeData = (stackalloc byte[16]);
        Poly1305(encryptedPayloadData, polyKeyData, expectedMessageAuthenticationCodeData);

        if (!CryptographicOperations.FixedTimeEquals(expectedMessageAuthenticationCodeData, messageAuthenticationCodeData))
            throw new ArgumentException("MAC verification failed");

        var fullDecryptedDataArray = new byte[encryptedPayloadData.Length];
        ChaCha20XorPadded(subKeyData, nonce24Data.Slice(16, 8), encryptedPayloadData, fullDecryptedDataArray, (stackalloc byte[32]));

        return fullDecryptedDataArray;
    }

    private static void QuarterRound(Span<uint> stateSpan, int indexA, int indexB, int indexC, int indexD)
    {
        stateSpan[indexA] = unchecked(stateSpan[indexA] + stateSpan[indexB]);
        stateSpan[indexD] ^= stateSpan[indexA];
        stateSpan[indexD] = BitOperations.RotateLeft(stateSpan[indexD], 16);

        stateSpan[indexC] = unchecked(stateSpan[indexC] + stateSpan[indexD]);
        stateSpan[indexB] ^= stateSpan[indexC];
        stateSpan[indexB] = BitOperations.RotateLeft(stateSpan[indexB], 12);

        stateSpan[indexA] = unchecked(stateSpan[indexA] + stateSpan[indexB]);
        stateSpan[indexD] ^= stateSpan[indexA];
        stateSpan[indexD] = BitOperations.RotateLeft(stateSpan[indexD], 8);

        stateSpan[indexC] = unchecked(stateSpan[indexC] + stateSpan[indexD]);
        stateSpan[indexB] ^= stateSpan[indexC];
        stateSpan[indexB] = BitOperations.RotateLeft(stateSpan[indexB], 7);
    }

    private static void DoubleRounds(Span<uint> stateSpan, int iterationsCount)
    {
        for (var currentIteration = 0; currentIteration < iterationsCount; currentIteration++)
        {
            QuarterRound(stateSpan, 0, 4, 8, 12);
            QuarterRound(stateSpan, 1, 5, 9, 13);
            QuarterRound(stateSpan, 2, 6, 10, 14);
            QuarterRound(stateSpan, 3, 7, 11, 15);

            QuarterRound(stateSpan, 0, 5, 10, 15);
            QuarterRound(stateSpan, 1, 6, 11, 12);
            QuarterRound(stateSpan, 2, 7, 8, 13);
            QuarterRound(stateSpan, 3, 4, 9, 14);
        }
    }

    private static void HChaCha20(ReadOnlySpan<byte> keyData, ReadOnlySpan<byte> nonce16Data, Span<byte> resultData)
    {
        var stateSpan = (stackalloc uint[16]);
        SigmaConstants.CopyTo(stateSpan[..4]);

        for (var keyIndex = 0; keyIndex < 8; keyIndex++)
            stateSpan[4 + keyIndex] = BinaryPrimitives.ReadUInt32LittleEndian(keyData.Slice(keyIndex * 4, 4));

        for (var nonceIndex = 0; nonceIndex < 4; nonceIndex++)
            stateSpan[12 + nonceIndex] = BinaryPrimitives.ReadUInt32LittleEndian(nonce16Data.Slice(nonceIndex * 4, 4));

        DoubleRounds(stateSpan, HChaChaIterationsCount);

        for (var resultIndex = 0; resultIndex < 4; resultIndex++)
            BinaryPrimitives.WriteUInt32LittleEndian(resultData.Slice(resultIndex * 4, 4), stateSpan[resultIndex]);

        for (var resultIndex = 0; resultIndex < 4; resultIndex++)
            BinaryPrimitives.WriteUInt32LittleEndian(resultData.Slice(16 + resultIndex * 4, 4), stateSpan[12 + resultIndex]);
    }

    private static void ChaCha20Block(ReadOnlySpan<byte> keyData, long counterValue, ReadOnlySpan<byte> nonce8Data, Span<byte> outputBlockSpan)
    {
        var initialStateSpan = (stackalloc uint[16]);
        SigmaConstants.CopyTo(initialStateSpan[..4]);

        for (var keyIndex = 0; keyIndex < 8; keyIndex++)
            initialStateSpan[4 + keyIndex] = BinaryPrimitives.ReadUInt32LittleEndian(keyData.Slice(keyIndex * 4, 4));

        initialStateSpan[12] = (uint)(counterValue & 0xFFFFFFFF);
        initialStateSpan[13] = (uint)((counterValue >> 32) & 0xFFFFFFFF);

        for (var nonceIndex = 0; nonceIndex < 2; nonceIndex++)
            initialStateSpan[14 + nonceIndex] = BinaryPrimitives.ReadUInt32LittleEndian(nonce8Data.Slice(nonceIndex * 4, 4));

        var workingStateSpan = (stackalloc uint[16]);
        initialStateSpan.CopyTo(workingStateSpan);
        DoubleRounds(workingStateSpan, ChaChaIterationsCount);

        for (var blockIndex = 0; blockIndex < 16; blockIndex++)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(
                outputBlockSpan.Slice(blockIndex * 4, 4),
                unchecked(workingStateSpan[blockIndex] + initialStateSpan[blockIndex])
            );
        }
    }

    private static void ChaCha20XorPadded(ReadOnlySpan<byte> keyData, ReadOnlySpan<byte> nonce8Data, ReadOnlySpan<byte> inputData, Span<byte> outputData, Span<byte> polyKeyData)
    {
        var keyStreamBlockSpan = (stackalloc byte[64]);
        ChaCha20Block(keyData, 0, nonce8Data, keyStreamBlockSpan);

        keyStreamBlockSpan[..32].CopyTo(polyKeyData);

        var inputOffsetIndex = 0;
        var bytesToProcessCount = Math.Min(32, inputData.Length);

        for (var byteIndex = 0; byteIndex < bytesToProcessCount; byteIndex++)
            outputData[byteIndex] = (byte)(inputData[byteIndex] ^ keyStreamBlockSpan[32 + byteIndex]);

        inputOffsetIndex += bytesToProcessCount;
        var currentBlockIndex = 1L;

        while (inputOffsetIndex < inputData.Length)
        {
            ChaCha20Block(keyData, currentBlockIndex, nonce8Data, keyStreamBlockSpan);
            bytesToProcessCount = Math.Min(64, inputData.Length - inputOffsetIndex);

            for (var byteIndex = 0; byteIndex < bytesToProcessCount; byteIndex++)
                outputData[inputOffsetIndex + byteIndex] = (byte)(inputData[inputOffsetIndex + byteIndex] ^ keyStreamBlockSpan[byteIndex]);

            inputOffsetIndex += bytesToProcessCount;
            currentBlockIndex++;
        }
    }

    private static void Poly1305(ReadOnlySpan<byte> messageData, ReadOnlySpan<byte> keyData, Span<byte> macResultData)
    {
        var clampedKeyBigInt = new BigInteger(keyData[..16], isUnsigned: true, isBigEndian: false) & Poly1305ClampConstant;
        var secretStateBigInt = new BigInteger(keyData.Slice(16, 16), isUnsigned: true, isBigEndian: false);
        var accumulatorBigInt = BigInteger.Zero;

        for (var chunkStartIndex = 0; chunkStartIndex < messageData.Length; chunkStartIndex += 16)
        {
            var currentChunkLength = Math.Min(16, messageData.Length - chunkStartIndex);
            var chunkValueBigInt = new BigInteger(messageData.Slice(chunkStartIndex, currentChunkLength), isUnsigned: true, isBigEndian: false) + (BigInteger.One << (8 * currentChunkLength));

            accumulatorBigInt = ((accumulatorBigInt + chunkValueBigInt) * clampedKeyBigInt) % Poly1305PrimeModulus;
        }

        var finalCalculatedMac = (accumulatorBigInt + secretStateBigInt) & ((BigInteger.One << 128) - 1);

        macResultData.Clear();
        finalCalculatedMac.TryWriteBytes(macResultData, out _, isUnsigned: true, isBigEndian: false);
    }

    private static void CryptoScalarMult(ReadOnlySpan<byte> clientSecretKeyData, ReadOnlySpan<byte> serverPublicKeyData, Span<byte> resultData)
    {
        var clampedSecretKeyData = (stackalloc byte[32]);
        clientSecretKeyData[..32].CopyTo(clampedSecretKeyData);
        clampedSecretKeyData[0] &= 248;
        clampedSecretKeyData[31] &= 127;
        clampedSecretKeyData[31] |= 64;

        var scalarValue = new BigInteger(clampedSecretKeyData, isUnsigned: true, isBigEndian: false);

        var clampedPublicKeyData = (stackalloc byte[32]);
        serverPublicKeyData[..32].CopyTo(clampedPublicKeyData);
        clampedPublicKeyData[31] &= 127;
        var baseXCoordinate = new BigInteger(clampedPublicKeyData, isUnsigned: true, isBigEndian: false);

        var xCoordinate2 = BigInteger.One;
        var zCoordinate2 = BigInteger.Zero;
        var xCoordinate3 = baseXCoordinate;
        var zCoordinate3 = BigInteger.One;

        var swapFlag = 0;

        for (var bitIndex = 254; bitIndex >= 0; bitIndex--)
        {
            var currentBit = (int)((scalarValue >> bitIndex) & 1);
            swapFlag ^= currentBit;

            if (swapFlag != 0)
            {
                (xCoordinate2, xCoordinate3) = (xCoordinate3, xCoordinate2);
                (zCoordinate2, zCoordinate3) = (zCoordinate3, zCoordinate2);
            }

            swapFlag = currentBit;

            var sumX2Z2 = Modulo(xCoordinate2 + zCoordinate2, Curve25519PrimeModulus);
            var squaredSumX2Z2 = Modulo(sumX2Z2 * sumX2Z2, Curve25519PrimeModulus);
            var diffX2Z2 = Modulo(xCoordinate2 - zCoordinate2, Curve25519PrimeModulus);
            var squaredDiffX2Z2 = Modulo(diffX2Z2 * diffX2Z2, Curve25519PrimeModulus);
            var diffSquaredSumAndDiff = Modulo(squaredSumX2Z2 - squaredDiffX2Z2, Curve25519PrimeModulus);

            var sumX3Z3 = Modulo(xCoordinate3 + zCoordinate3, Curve25519PrimeModulus);
            var diffX3Z3 = Modulo(xCoordinate3 - zCoordinate3, Curve25519PrimeModulus);

            var productDiffX3Z3AndSumX2Z2 = Modulo(diffX3Z3 * sumX2Z2, Curve25519PrimeModulus);
            var productSumX3Z3AndDiffX2Z2 = Modulo(sumX3Z3 * diffX2Z2, Curve25519PrimeModulus);

            var sumProducts = Modulo(productDiffX3Z3AndSumX2Z2 + productSumX3Z3AndDiffX2Z2, Curve25519PrimeModulus);
            xCoordinate3 = Modulo(sumProducts * sumProducts, Curve25519PrimeModulus);

            var diffProducts = Modulo(productDiffX3Z3AndSumX2Z2 - productSumX3Z3AndDiffX2Z2, Curve25519PrimeModulus);
            zCoordinate3 = Modulo(baseXCoordinate * Modulo(diffProducts * diffProducts, Curve25519PrimeModulus), Curve25519PrimeModulus);

            xCoordinate2 = Modulo(squaredSumX2Z2 * squaredDiffX2Z2, Curve25519PrimeModulus);
            zCoordinate2 = Modulo(diffSquaredSumAndDiff * Modulo(squaredSumX2Z2 + Modulo(CurveConstantA24 * diffSquaredSumAndDiff, Curve25519PrimeModulus), Curve25519PrimeModulus), Curve25519PrimeModulus);
        }

        if (swapFlag != 0)
        {
            (xCoordinate2, xCoordinate3) = (xCoordinate3, xCoordinate2);
            (zCoordinate2, zCoordinate3) = (zCoordinate3, zCoordinate2);
        }

        var zCoordinate2Inverse = BigInteger.ModPow(zCoordinate2, Curve25519PrimeModulus - 2, Curve25519PrimeModulus);
        var finalXCoordinate = Modulo(xCoordinate2 * zCoordinate2Inverse, Curve25519PrimeModulus);

        finalXCoordinate.TryWriteBytes(resultData, out _, isUnsigned: true, isBigEndian: false);
    }

    private static BigInteger Modulo(BigInteger valueData, BigInteger modulusData)
    {
        var remainderData = valueData % modulusData;
        return remainderData.Sign < 0 ? remainderData + modulusData : remainderData;
    }
}