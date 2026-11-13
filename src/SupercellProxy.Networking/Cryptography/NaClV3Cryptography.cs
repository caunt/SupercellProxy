using System.Buffers.Binary;
using System.Numerics;
using System.Security.Cryptography;

namespace SupercellProxy.Networking.Cryptography;

/// <summary>
/// The NaCl-style primitives the game protocol encrypts with: Curve25519 key agreement, a ChaCha stream cipher keyed through
/// HChaCha, and Poly1305 authentication. Supercell changed the round counts (8 ChaCha double rounds and 17 HChaCha double rounds),
/// so standard libsodium boxes do not interoperate with these.
/// </summary>
public static class NaClV3Cryptography
{
    private const int ChaChaIterationsCount = 8;
    private const int HChaChaIterationsCount = 17;

    private const uint LimbMask = 0x3ffffff;

    private static readonly BigInteger Curve25519PrimeModulus = BigInteger.Pow(value: 2, exponent: 255) - 19;
    private static readonly BigInteger CurveConstantA24 = new(value: 121665);

    // A static array: a uint span literal is materialized per access on this runtime and showed up as a per-block allocation.
    private static readonly uint[] SigmaConstants = [0x61707865, 0x3320646e, 0x79622d32, 0x6b206574];

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

    /// <summary>
    /// Encrypts and authenticates a message for the holder of <paramref name="remotePublicKey"/>, as the login handshake does.
    /// </summary>
    /// <param name="plaintext">Bytes to protect.</param>
    /// <param name="nonce24">24-byte nonce; the first 16 bytes derive the per-message key, the last 8 feed the stream.</param>
    /// <param name="remotePublicKey">Recipient's 32-byte Curve25519 public key.</param>
    /// <param name="localPrivateKey">Sender's 32-byte Curve25519 private key.</param>
    /// <returns>A 16-byte Poly1305 tag followed by the encrypted bytes.</returns>
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
    /// Checks and decrypts a message produced by <see cref="Box"/> with the matching key pair.
    /// </summary>
    /// <param name="ciphertext">A 16-byte Poly1305 tag followed by the encrypted bytes.</param>
    /// <param name="nonce24">The 24-byte nonce the sender used.</param>
    /// <param name="remotePublicKey">Sender's 32-byte Curve25519 public key.</param>
    /// <param name="localPrivateKey">Recipient's 32-byte Curve25519 private key.</param>
    /// <returns>The decrypted bytes.</returns>
    /// <exception cref="MacVerificationException">The tag does not match: wrong keys or nonce, or the bytes were altered.</exception>
    public static byte[] BoxOpen(ReadOnlySpan<byte> ciphertext,
        ReadOnlySpan<byte> nonce24,
        ReadOnlySpan<byte> remotePublicKey,
        ReadOnlySpan<byte> localPrivateKey)
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
    /// Derives the Curve25519 public key that belongs to a private key.
    /// </summary>
    /// <param name="localPrivateKey">32-byte private key.</param>
    /// <returns>The 32-byte public key to hand to the other side.</returns>
    public static byte[] CryptoScalarMultBase(ReadOnlySpan<byte> localPrivateKey)
    {
        byte[] publicKey = new byte[32];

        CryptoScalarMult(localPrivateKey, Curve25519BasePoint, publicKey);

        return publicKey;
    }

    /// <summary>
    /// Derives a new 32-byte key from a key and a 16-byte nonce, using the game's 17-double-round variant of HChaCha.
    /// </summary>
    /// <param name="key">32-byte input key.</param>
    /// <param name="nonce16">16 bytes mixed into the derivation.</param>
    /// <returns>The derived 32-byte key.</returns>
    public static byte[] HChaCha20(ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce16)
    {
        byte[] subKey = new byte[32];

        HChaCha20(key, nonce16, subKey);

        return subKey;
    }

    /// <summary>
    /// Encrypts and authenticates a message under a shared symmetric key, as every message after the handshake is.
    /// </summary>
    /// <param name="plaintext">Bytes to protect.</param>
    /// <param name="nonce24">24-byte nonce; must never repeat under the same key.</param>
    /// <param name="key">32-byte session key both sides hold.</param>
    /// <returns>A 16-byte Poly1305 tag followed by the encrypted bytes.</returns>
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
    /// Checks and decrypts a message produced by <see cref="SecretBox"/>.
    /// </summary>
    /// <param name="ciphertext">A 16-byte Poly1305 tag followed by the encrypted bytes.</param>
    /// <param name="nonce24">The 24-byte nonce the sender used.</param>
    /// <param name="key">32-byte session key both sides hold.</param>
    /// <returns>The decrypted bytes.</returns>
    /// <exception cref="MacVerificationException">The tag does not match: wrong key or nonce, or the bytes were altered.</exception>
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
            // Column round: mix the four columns of the 4x4 state.
            QuarterRound(span, indexA: 0, indexB: 4, indexC: 8, indexD: 12);
            QuarterRound(span, indexA: 1, indexB: 5, indexC: 9, indexD: 13);
            QuarterRound(span, indexA: 2, indexB: 6, indexC: 10, indexD: 14);
            QuarterRound(span, indexA: 3, indexB: 7, indexC: 11, indexD: 15);

            // Diagonal round: mix the four diagonals so every column reaches every other.
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

    private static MontgomeryStepResult MontgomeryStep(BigInteger baseXCoordinate, BigInteger xCoordinate2, BigInteger zCoordinate2, BigInteger xCoordinate3, BigInteger zCoordinate3)
    {
        // Square the sum and the difference of the first point's coordinates for the doubling.
        BigInteger sumX2Z2 = Modulo(xCoordinate2 + zCoordinate2, Curve25519PrimeModulus);
        BigInteger squaredSumX2Z2 = Modulo(sumX2Z2 * sumX2Z2, Curve25519PrimeModulus);
        BigInteger diffX2Z2 = Modulo(xCoordinate2 - zCoordinate2, Curve25519PrimeModulus);
        BigInteger squaredDiffX2Z2 = Modulo(diffX2Z2 * diffX2Z2, Curve25519PrimeModulus);

        // Their difference equals 4 * x2 * z2, which the doubling formula reuses.
        BigInteger diffSquaredSumAndDiff = Modulo(squaredSumX2Z2 - squaredDiffX2Z2, Curve25519PrimeModulus);

        // Cross-multiply with the second point's sum and difference for the differential addition.
        BigInteger sumX3Z3 = Modulo(xCoordinate3 + zCoordinate3, Curve25519PrimeModulus);
        BigInteger diffX3Z3 = Modulo(xCoordinate3 - zCoordinate3, Curve25519PrimeModulus);
        BigInteger productDiffX3Z3AndSumX2Z2 = Modulo(diffX3Z3 * sumX2Z2, Curve25519PrimeModulus);
        BigInteger productSumX3Z3AndDiffX2Z2 = Modulo(sumX3Z3 * diffX2Z2, Curve25519PrimeModulus);

        // The squared sum of those products becomes the added point's x.
        BigInteger sumProducts = Modulo(productDiffX3Z3AndSumX2Z2 + productSumX3Z3AndDiffX2Z2, Curve25519PrimeModulus);

        xCoordinate3 = Modulo(sumProducts * sumProducts, Curve25519PrimeModulus);

        BigInteger diffProducts = Modulo(productDiffX3Z3AndSumX2Z2 - productSumX3Z3AndDiffX2Z2, Curve25519PrimeModulus);

        zCoordinate3 = Modulo(baseXCoordinate * Modulo(diffProducts * diffProducts, Curve25519PrimeModulus), Curve25519PrimeModulus);
        xCoordinate2 = Modulo(squaredSumX2Z2 * squaredDiffX2Z2, Curve25519PrimeModulus);

        zCoordinate2 = Modulo(diffSquaredSumAndDiff
                * Modulo(squaredSumX2Z2 + Modulo(CurveConstantA24 * diffSquaredSumAndDiff, Curve25519PrimeModulus), Curve25519PrimeModulus),
            Curve25519PrimeModulus);

        return new MontgomeryStepResult(xCoordinate2, zCoordinate2, xCoordinate3, zCoordinate3);
    }

    private static void Poly1305(ReadOnlySpan<byte> message, ReadOnlySpan<byte> key, Span<byte> macResult)
    {
        // Radix 2^26 limbs (poly1305-donna); every product fits in 64 bits, so no per-block allocation is needed.
        // Load r from the first key half, with the masks applying the Poly1305 clamp.
        uint keyLimb0 = BinaryPrimitives.ReadUInt32LittleEndian(key) & 0x3ffffff;
        uint keyLimb1 = (BinaryPrimitives.ReadUInt32LittleEndian(key[3..]) >> 2) & 0x3ffff03;
        uint keyLimb2 = (BinaryPrimitives.ReadUInt32LittleEndian(key[6..]) >> 4) & 0x3ffc0ff;
        uint keyLimb3 = (BinaryPrimitives.ReadUInt32LittleEndian(key[9..]) >> 6) & 0x3f03fff;

        // The clamp limits r to 124 bits, so the top limb keeps only 20.
        uint keyLimb4 = (BinaryPrimitives.ReadUInt32LittleEndian(key[12..]) >> 8) & 0x00fffff;

        // Precompute five times r for the cross terms that wrap past 2^130.
        ulong scaledLimb1 = keyLimb1 * 5UL;
        ulong scaledLimb2 = keyLimb2 * 5UL;
        ulong scaledLimb3 = keyLimb3 * 5UL;
        ulong scaledLimb4 = keyLimb4 * 5UL;

        // The accumulator h starts at zero.
        uint accumulator0 = 0;
        uint accumulator1 = 0;
        uint accumulator2 = 0;
        uint accumulator3 = 0;

        // Its top limb also collects the 2^128 pad bit of every full block.
        uint accumulator4 = 0;

        // Scratch space for padding a short final block.
        Span<byte> finalBlock = stackalloc byte[16];

        for (int offset = 0; offset < message.Length; offset += 16)
        {
            int length = Math.Min(val1: 16, message.Length - offset);
            scoped ReadOnlySpan<byte> block = message.Slice(offset, length);
            uint highBit = 1U << 24;

            if (length < 16)
            {
                // A short final block carries its 1 byte inside the block instead of the 2^128 bit.
                finalBlock.Clear();
                block.CopyTo(finalBlock);

                finalBlock[length] = 1;
                block = finalBlock;
                highBit = 0;
            }

            // Add the block's low 104 bits to the accumulator limbs.
            accumulator0 += BinaryPrimitives.ReadUInt32LittleEndian(block) & LimbMask;
            accumulator1 += (BinaryPrimitives.ReadUInt32LittleEndian(block[3..]) >> 2) & LimbMask;
            accumulator2 += (BinaryPrimitives.ReadUInt32LittleEndian(block[6..]) >> 4) & LimbMask;
            accumulator3 += (BinaryPrimitives.ReadUInt32LittleEndian(block[9..]) >> 6) & LimbMask;

            // The top limb also gets the 2^128 pad bit, unless a short block already carries its padding byte.
            accumulator4 += (BinaryPrimitives.ReadUInt32LittleEndian(block[12..]) >> 8) | highBit;

            // Multiply the accumulator by r; terms past 2^130 come back in through the times-five key limbs.
            ulong product0 = (ulong.CreateChecked(accumulator0) * keyLimb0) + (accumulator1 * scaledLimb4) + (accumulator2 * scaledLimb3) + (accumulator3 * scaledLimb2) + (accumulator4 * scaledLimb1);
            ulong product1 = (ulong.CreateChecked(accumulator0) * keyLimb1) + (ulong.CreateChecked(accumulator1) * keyLimb0) + (accumulator2 * scaledLimb4) + (accumulator3 * scaledLimb3) + (accumulator4 * scaledLimb2);
            ulong product2 = (ulong.CreateChecked(accumulator0) * keyLimb2) + (ulong.CreateChecked(accumulator1) * keyLimb1) + (ulong.CreateChecked(accumulator2) * keyLimb0) + (accumulator3 * scaledLimb4) + (accumulator4 * scaledLimb3);
            ulong product3 = (ulong.CreateChecked(accumulator0) * keyLimb3) + (ulong.CreateChecked(accumulator1) * keyLimb2) + (ulong.CreateChecked(accumulator2) * keyLimb1) + (ulong.CreateChecked(accumulator3) * keyLimb0) + (accumulator4 * scaledLimb4);

            // The top limb has no wrapped terms, and the carry out of limb 0 starts the reduction.
            ulong product4 = (ulong.CreateChecked(accumulator0) * keyLimb4) + (ulong.CreateChecked(accumulator1) * keyLimb3) + (ulong.CreateChecked(accumulator2) * keyLimb2) + (ulong.CreateChecked(accumulator3) * keyLimb1) + (ulong.CreateChecked(accumulator4) * keyLimb0);
            ulong carry = product0 >> 26;

            // Reduce the product back to 26-bit limbs, carrying each limb's overflow into the next.
            accumulator0 = uint.CreateTruncating(product0) & LimbMask;
            product1 += carry;
            carry = product1 >> 26;
            accumulator1 = uint.CreateTruncating(product1) & LimbMask;

            // Limb 2 takes the carry from limb 1 and hands its own overflow on.
            product2 += carry;
            carry = product2 >> 26;
            accumulator2 = uint.CreateTruncating(product2) & LimbMask;

            // Limb 3 does the same, passing its overflow to the top limb.
            product3 += carry;
            carry = product3 >> 26;
            accumulator3 = uint.CreateTruncating(product3) & LimbMask;

            // What the top limb overflows is the part of the product at or above 2^130.
            product4 += carry;
            carry = product4 >> 26;
            accumulator4 = uint.CreateTruncating(product4) & LimbMask;

            // Wrap that part into limb 0 times five and leave limb 1 slightly above 26 bits for the next block.
            accumulator0 += uint.CreateTruncating(carry) * 5;
            accumulator1 += accumulator0 >> 26;
            accumulator0 &= LimbMask;
        }

        uint finalCarry = accumulator1 >> 26;

        // Carry limb 1, the only limb the block loop leaves above 26 bits, into limb 2.
        accumulator1 &= LimbMask;
        accumulator2 += finalCarry;

        // Pass limb 2's overflow on to limb 3.
        finalCarry = accumulator2 >> 26;
        accumulator2 &= LimbMask;
        accumulator3 += finalCarry;

        // Pass limb 3's overflow on to the top limb.
        finalCarry = accumulator3 >> 26;
        accumulator3 &= LimbMask;
        accumulator4 += finalCarry;

        // Bits past 2^130 wrap around to limb 0 times five, because 2^130 is 5 modulo the prime.
        finalCarry = accumulator4 >> 26;
        accumulator4 &= LimbMask;
        accumulator0 += finalCarry * 5;

        // One last carry from limb 0 leaves every limb within 26 bits.
        finalCarry = accumulator0 >> 26;
        accumulator0 &= LimbMask;
        accumulator1 += finalCarry;

        // Subtract the prime 2^130 - 5 once when the accumulator is not below it.
        uint reduced0 = accumulator0 + 5;

        finalCarry = reduced0 >> 26;
        reduced0 &= LimbMask;

        uint reduced1 = accumulator1 + finalCarry;

        finalCarry = reduced1 >> 26;
        reduced1 &= LimbMask;

        uint reduced2 = accumulator2 + finalCarry;

        finalCarry = reduced2 >> 26;
        reduced2 &= LimbMask;

        uint reduced3 = accumulator3 + finalCarry;

        finalCarry = reduced3 >> 26;
        reduced3 &= LimbMask;

        uint reduced4 = unchecked(accumulator4 + finalCarry - (1U << 26));
        uint selectReduced = unchecked((reduced4 >> 31) - 1);
        uint selectOriginal = ~selectReduced;

        // Keep the reduced value only when subtracting the prime did not go negative, using masks instead of a branch.
        accumulator0 = (accumulator0 & selectOriginal) | (reduced0 & selectReduced);
        accumulator1 = (accumulator1 & selectOriginal) | (reduced1 & selectReduced);
        accumulator2 = (accumulator2 & selectOriginal) | (reduced2 & selectReduced);
        accumulator3 = (accumulator3 & selectOriginal) | (reduced3 & selectReduced);

        // The top limb, where the 2^130 was subtracted, follows the same choice.
        accumulator4 = (accumulator4 & selectOriginal) | (reduced4 & selectReduced);

        // Repack the five 26-bit limbs into the four 32-bit words of h modulo 2^128.
        uint word0 = accumulator0 | (accumulator1 << 26);
        uint word1 = (accumulator1 >> 6) | (accumulator2 << 20);
        uint word2 = (accumulator2 >> 12) | (accumulator3 << 14);
        uint word3 = (accumulator3 >> 18) | (accumulator4 << 8);

        // Add the second key half word by word, carrying into the next word, to form the tag.
        ulong sum = ulong.CreateChecked(word0) + BinaryPrimitives.ReadUInt32LittleEndian(key[16..]);

        BinaryPrimitives.WriteUInt32LittleEndian(macResult, uint.CreateTruncating(sum));

        sum = ulong.CreateChecked(word1) + BinaryPrimitives.ReadUInt32LittleEndian(key[20..]) + (sum >> 32);

        BinaryPrimitives.WriteUInt32LittleEndian(macResult[4..], uint.CreateTruncating(sum));

        sum = ulong.CreateChecked(word2) + BinaryPrimitives.ReadUInt32LittleEndian(key[24..]) + (sum >> 32);

        BinaryPrimitives.WriteUInt32LittleEndian(macResult[8..], uint.CreateTruncating(sum));

        sum = ulong.CreateChecked(word3) + BinaryPrimitives.ReadUInt32LittleEndian(key[28..]) + (sum >> 32);

        BinaryPrimitives.WriteUInt32LittleEndian(macResult[12..], uint.CreateTruncating(sum));
    }

    private static void QuarterRound(Span<uint> span, int indexA, int indexB, int indexC, int indexD)
    {
        // Let b's value flow into a, then scramble d with the new a.
        span[indexA] = unchecked(span[indexA] + span[indexB]);
        span[indexD] ^= span[indexA];
        span[indexD] = BitOperations.RotateLeft(span[indexD], offset: 16);

        // Let d's value flow into c, then scramble b with the new c.
        span[indexC] = unchecked(span[indexC] + span[indexD]);
        span[indexB] ^= span[indexC];
        span[indexB] = BitOperations.RotateLeft(span[indexB], offset: 12);

        // Repeat the a and d mix with the shorter rotation of the second half.
        span[indexA] = unchecked(span[indexA] + span[indexB]);
        span[indexD] ^= span[indexA];
        span[indexD] = BitOperations.RotateLeft(span[indexD], offset: 8);

        // Repeat the c and b mix, after which every word depends on all four inputs.
        span[indexC] = unchecked(span[indexC] + span[indexD]);
        span[indexB] ^= span[indexC];
        span[indexB] = BitOperations.RotateLeft(span[indexB], offset: 7);
    }

    private static MontgomeryPoint RunMontgomeryLadder(BigInteger scalarValue, BigInteger baseXCoordinate)
    {
        // Start the ladder from the point at infinity and the base point.
        BigInteger xCoordinate2 = BigInteger.One;
        BigInteger zCoordinate2 = BigInteger.Zero;
        BigInteger xCoordinate3 = baseXCoordinate;
        BigInteger zCoordinate3 = BigInteger.One;

        // The two points start unswapped.
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

        return new MontgomeryPoint(xCoordinate2, zCoordinate2);
    }
}
