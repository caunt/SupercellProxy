using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SupercellProxy.Networking.Protocol;

/// <summary>
/// Represents <c language="csharp">LongId</c>.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="LongIdentifier"/> instance.
/// </remarks>
[StructLayout(LayoutKind.Auto)]
public readonly record struct LongIdentifier : IEquatable<LongIdentifier>
{

    /// <summary>
    /// Defines the <c language="csharp">Empty</c> value.
    /// </summary>
    public static readonly LongIdentifier Empty;
    /// <summary>Creates an identifier from its two 32-bit words.</summary>
    public LongIdentifier(int highInt32, int lowInt32)
    {
        HighInt32 = highInt32;
        LowInt32 = lowInt32;
    }

    private static ReadOnlySpan<char> ValidAlphabet => "0289PYLQGRJCUV";

    /// <summary>
    /// Gets the <c language="csharp">HighInt32</c> value.
    /// </summary>
    public int HighInt32 { get; }

    /// <summary>
    /// Gets the <c language="csharp">LowInt32</c> value.
    /// </summary>
    public int LowInt32 { get; }

    /// <summary>
    /// Gets the <c language="csharp">HighUInt32</c> value.
    /// </summary>
    public uint HighUInt32 => uint.CreateTruncating(HighInt32);

    /// <summary>
    /// Gets the <c language="csharp">LowUInt32</c> value.
    /// </summary>
    public uint LowUInt32 => uint.CreateTruncating(LowInt32);

    /// <summary>
    /// Gets the <c language="csharp">AsInt64</c> value.
    /// </summary>
    public long AsInt64 => (long.CreateTruncating(HighInt32) << 32) | LowUInt32;

    /// <summary>
    /// Gets the <c language="csharp">AsUInt64</c> value.
    /// </summary>
    public ulong AsUInt64 => (ulong.CreateTruncating(HighUInt32) << 32) | LowUInt32;

    /// <summary>
    /// Gets the <c language="csharp">CanRepresentAsTag</c> value.
    /// </summary>
    public bool CanRepresentAsTag => HighUInt32 <= 255;

    /// <summary>
    /// Gets the <c language="csharp">TagInternalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("TagInternalId")]
    public ulong TagInternalIdentifier => !CanRepresentAsTag
                ? throw new InvalidOperationException(message: "HighUInt32 must be less than or equal to 255 to be representable as an public tag ID.")
                : (LowUInt32 * 256UL) + HighUInt32;

    /// <summary>
    /// Applies the <c language="csharp">+</c> operator.
    /// </summary>
    public static LongIdentifier operator +(LongIdentifier leftLongIdentifier, ulong rightValue)
    {
        unchecked
        {
            ulong addedValue = leftLongIdentifier.AsUInt64 + rightValue;
            int newHighInt32 = int.CreateTruncating(addedValue >> 32);
            int newLowInt32 = int.CreateTruncating(addedValue);

            return new LongIdentifier(newHighInt32, newLowInt32);
        }
    }

    /// <summary>
    /// Applies the <c language="csharp">++</c> operator.
    /// </summary>
    public static LongIdentifier operator ++(LongIdentifier logicLong)
    {
        return logicLong + 1UL;
    }

    /// <summary>
    /// Applies the <c language="csharp">-</c> operator.
    /// </summary>
    public static LongIdentifier operator -(LongIdentifier leftLongIdentifier, ulong rightValue)
    {
        unchecked
        {
            ulong subtractedValue = leftLongIdentifier.AsUInt64 - rightValue;
            int newHighInt32 = int.CreateTruncating(subtractedValue >> 32);
            int newLowInt32 = int.CreateTruncating(subtractedValue);

            return new LongIdentifier(newHighInt32, newLowInt32);
        }
    }

    /// <summary>
    /// Applies the <c language="csharp">--</c> operator.
    /// </summary>
    public static LongIdentifier operator --(LongIdentifier logicLong)
    {
        return logicLong - 1UL;
    }

    /// <summary>
    /// Executes the <c language="csharp">Parse</c> operation.
    /// </summary>
    public static LongIdentifier Parse(ReadOnlySpan<char> tagStringSpan)
    {
        return !TryParse(tagStringSpan, out LongIdentifier logicLong)
            ? throw new FormatException($"The provided tag string '{tagStringSpan}' is not in a valid format.")
            : logicLong;
    }

    /// <summary>
    /// Executes the <c language="csharp">ParseLazily</c> operation.
    /// </summary>
    public static IEnumerable<LongIdentifier> ParseLazily(IEnumerable<string> inputTagStrings)
    {
        ArgumentNullException.ThrowIfNull(inputTagStrings);

        foreach (string? currentTagString in inputTagStrings)
        {
            if (currentTagString is not null && TryParse(currentTagString.AsSpan(), out LongIdentifier logicLong))
                yield return logicLong;
        }
    }

    /// <summary>
    /// Attempts the <c language="csharp">Parse</c> operation.
    /// </summary>
    public static bool TryParse(ReadOnlySpan<char> tagStringSpan, out LongIdentifier result)
    {
        ulong totalCalculatedValue = 0UL;
        bool foundAnyValidDigits = false;

        foreach (char currentCharacter in tagStringSpan)
        {
            if (currentCharacter == '#')
                continue;

            foundAnyValidDigits = true;
            char normalizedCharacter = NormalizeTagCharacter(currentCharacter);

            if (!TryGetAlphabetIndex(normalizedCharacter, out int alphabetIndex))
            {
                result = default;

                return false;
            }

            ulong nextCalculatedValue =
                (totalCalculatedValue * 14UL) + ulong.CreateTruncating(alphabetIndex);

            if (nextCalculatedValue < totalCalculatedValue)
            {
                result = default;

                return false;
            }

            totalCalculatedValue = nextCalculatedValue;
        }

        if (!foundAnyValidDigits)
        {
            result = default;

            return false;
        }

        uint highInternalIdentifier = uint.CreateTruncating(totalCalculatedValue % 256UL);
        ulong lowInternalIdentifier = totalCalculatedValue / 256UL;

        if (lowInternalIdentifier > uint.MaxValue)
        {
            result = default;

            return false;
        }

        result = new LongIdentifier(int.CreateTruncating(highInternalIdentifier), int.CreateTruncating(lowInternalIdentifier));

        return true;
    }

    /// <summary>Adds an unsigned offset to this identifier.</summary>
    public LongIdentifier Add(ulong value)
    {
        return this + value;
    }

    /// <summary>Returns the identifier decremented by one.</summary>
    public LongIdentifier Decrement()
    {
        return this - 1UL;
    }

    /// <summary>
    /// Executes the <c language="csharp">Equals</c> operation.
    /// </summary>
    public bool Equals(LongIdentifier logicLong)
    {
        return HighInt32 == logicLong.HighInt32 && LowInt32 == logicLong.LowInt32;
    }

    /// <summary>
    /// Gets <c language="csharp">HashCode</c>.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(HighInt32, LowInt32);
    }

    /// <summary>Returns the identifier incremented by one.</summary>
    public LongIdentifier Increment()
    {
        return this + 1UL;
    }

    /// <summary>Subtracts an unsigned offset from this identifier.</summary>
    public LongIdentifier Subtract(ulong value)
    {
        return this - value;
    }

    /// <summary>
    /// Executes the <c language="csharp">ToFormattedString</c> operation.
    /// </summary>
    public string ToFormattedString(bool includeHashPrefix = true)
    {
        if (!CanRepresentAsTag)
            throw new InvalidOperationException(message: "This logic long cannot be represented as a tag.");

        ulong totalCalculatedValue = (LowUInt32 * 256UL) + HighUInt32;

        if (totalCalculatedValue == 0)
            return includeHashPrefix ? "#0" : "0";

        int calculatedDigitCount = 0;
        ulong temporaryValueForCounting = totalCalculatedValue;

        do
        {
            calculatedDigitCount++;
            temporaryValueForCounting /= 14UL;
        } while (temporaryValueForCounting > 0);

        int neededTotalLength = calculatedDigitCount + (includeHashPrefix ? 1 : 0);

        return string.Create(
            neededTotalLength,
            (totalCalculatedValue, includeHashPrefix, calculatedDigitCount),
            static (destinationSpan, formatState) =>
            {
                int currentDestinationIndex = 0;

                if (formatState.includeHashPrefix)
                {
                    destinationSpan[currentDestinationIndex] = '#';
                    currentDestinationIndex++;
                }

                ulong currentValueToFormat = formatState.totalCalculatedValue;

                for (
                    int reverseWriteIndex = formatState.calculatedDigitCount - 1; reverseWriteIndex >= 0;
                    reverseWriteIndex--
                )
                {
                    int remainderValue = int.CreateTruncating(currentValueToFormat % 14UL);
                    currentValueToFormat /= 14UL;
                    destinationSpan[currentDestinationIndex + reverseWriteIndex] = ValidAlphabet[remainderValue];
                }
            }
        );
    }

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
    public override string ToString()
    {
        return CanRepresentAsTag ? ToFormattedString() : $"{HighInt32}-{LowInt32}";
    }

    /// <summary>
    /// Attempts the <c language="csharp">Format</c> operation.
    /// </summary>
    public bool TryFormat(Span<char> destinationSpan, out int totalCharactersWritten, bool includeHashPrefix = true)
    {
        if (!CanRepresentAsTag)
        {
            totalCharactersWritten = 0;

            return false;
        }

        ulong totalCalculatedValue = (ulong.CreateTruncating(LowUInt32) * 256UL) + HighUInt32;

        if (totalCalculatedValue == 0)
            return TryFormatZero(destinationSpan, out totalCharactersWritten, includeHashPrefix);

        Span<char> temporaryDigitsSpan = stackalloc char[16];
        int currentDigitCount = 0;

        while (totalCalculatedValue > 0)
        {
            int remainderValue = int.CreateTruncating(totalCalculatedValue % 14UL);
            totalCalculatedValue /= 14UL;

            temporaryDigitsSpan[currentDigitCount] = ValidAlphabet[remainderValue];
            currentDigitCount++;
        }

        int neededTotalLength = currentDigitCount + (includeHashPrefix ? 1 : 0);

        if (destinationSpan.Length < neededTotalLength)
        {
            totalCharactersWritten = 0;

            return false;
        }

        int currentDestinationIndex = 0;

        if (includeHashPrefix)
        {
            destinationSpan[currentDestinationIndex] = '#';
            currentDestinationIndex++;
        }

        for (
            int reverseReadIndex = currentDigitCount - 1; reverseReadIndex >= 0;
            reverseReadIndex--
        )
        {
            destinationSpan[currentDestinationIndex] = temporaryDigitsSpan[reverseReadIndex];
            currentDestinationIndex++;
        }

        totalCharactersWritten = neededTotalLength;

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char NormalizeTagCharacter(char inputCharacter)
    {
        if (uint.CreateTruncating(inputCharacter - 'a') <= ('z' - 'a'))
            inputCharacter = Convert.ToChar(inputCharacter - 32);

        if (inputCharacter == 'O')
            inputCharacter = '0';

        return inputCharacter;
    }

    private static bool TryFormatZero(Span<char> destinationSpan, out int totalCharactersWritten, bool includeHashPrefix)
    {
        int neededLength = includeHashPrefix ? 2 : 1;

        if (destinationSpan.Length < neededLength)
        {
            totalCharactersWritten = 0;

            return false;
        }

        int writeIndex = 0;

        if (includeHashPrefix)
            destinationSpan[writeIndex++] = '#';

        destinationSpan[writeIndex] = '0';
        totalCharactersWritten = neededLength;

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryGetAlphabetIndex(char inputCharacter, out int alphabetIndex)
    {
        alphabetIndex = inputCharacter switch
        {
            '0' => 0,
            '2' => 1,
            '8' => 2,
            '9' => 3,
            'P' => 4,
            'Y' => 5,
            'L' => 6,
            'Q' => 7,
            'G' => 8,
            'R' => 9,
            'J' => 10,
            'C' => 11,
            'U' => 12,
            'V' => 13,
            _ => -1,
        };

        return alphabetIndex >= 0;
    }
}
