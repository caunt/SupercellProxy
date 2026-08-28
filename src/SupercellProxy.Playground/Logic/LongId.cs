using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SupercellProxy.Playground.Logic;

/// <summary>
/// Represents <c language="csharp">LongId</c>.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="LongId"/> instance.
/// </remarks>
[StructLayout(LayoutKind.Auto)]
internal readonly struct LongId(int highInt32, int lowInt32) : IEquatable<LongId>
{
    /// <summary>
    /// Defines the <c language="csharp">Empty</c> value.
    /// </summary>
    public static readonly LongId Empty;

    private static ReadOnlySpan<char> ValidAlphabet => "0289PYLQGRJCUV";

    /// <summary>
    /// Gets the <c language="csharp">HighInt32</c> value.
    /// </summary>
    public int HighInt32 { get; } = highInt32;

    /// <summary>
    /// Gets the <c language="csharp">LowInt32</c> value.
    /// </summary>
    public int LowInt32 { get; } = lowInt32;

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
    public ulong TagInternalId
    {
        get
        {
            if (!CanRepresentAsTag)
                throw new InvalidOperationException(
                    "HighUInt32 must be less than or equal to 255 to be representable as an internal tag ID."
                );

            return LowUInt32 * 256UL + HighUInt32;
        }
    }

    /// <summary>
    /// Executes the <c language="csharp">Parse</c> operation.
    /// </summary>
    public static LongId Parse(ReadOnlySpan<char> tagStringSpan)
    {
        if (!TryParse(tagStringSpan, out var logicLong))
            throw new FormatException(
                $"The provided tag string '{tagStringSpan}' is not in a valid format."
            );

        return logicLong;
    }

    /// <summary>
    /// Attempts the <c language="csharp">Parse</c> operation.
    /// </summary>
    public static bool TryParse(ReadOnlySpan<char> tagStringSpan, out LongId result)
    {
        var totalCalculatedValue = 0UL;
        var foundAnyValidDigits = false;

        foreach (var currentCharacter in tagStringSpan)
        {
            if (currentCharacter == '#')
                continue;

            foundAnyValidDigits = true;
            var normalizedCharacter = NormalizeTagCharacter(currentCharacter);

            if (!TryGetAlphabetIndex(normalizedCharacter, out var alphabetIndex))
            {
                result = default;
                return false;
            }

            var nextCalculatedValue =
                totalCalculatedValue * 14UL + ulong.CreateTruncating(alphabetIndex);

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

        var highInternalId = uint.CreateTruncating((totalCalculatedValue % 256UL));
        var lowInternalId = totalCalculatedValue / 256UL;

        if (lowInternalId > uint.MaxValue)
        {
            result = default;
            return false;
        }

        result = new LongId(
            int.CreateTruncating(highInternalId),
            int.CreateTruncating(lowInternalId)
        );
        return true;
    }

    /// <summary>
    /// Executes the <c language="csharp">ParseLazily</c> operation.
    /// </summary>
    public static IEnumerable<LongId> ParseLazily(IEnumerable<string> inputTagStrings)
    {
        foreach (var currentTagString in inputTagStrings)
        {
            if (
                currentTagString is not null
                && TryParse(currentTagString.AsSpan(), out var logicLong)
            )
                yield return logicLong;
        }
    }

    /// <summary>
    /// Attempts the <c language="csharp">Format</c> operation.
    /// </summary>
    public bool TryFormat(
        Span<char> destinationSpan,
        out int totalCharactersWritten,
        bool includeHashPrefix = true
    )
    {
        if (!CanRepresentAsTag)
        {
            totalCharactersWritten = 0;
            return false;
        }

        var totalCalculatedValue = ulong.CreateTruncating(LowUInt32) * 256UL + HighUInt32;

        if (totalCalculatedValue == 0)
            return TryFormatZero(destinationSpan, out totalCharactersWritten, includeHashPrefix);

        var temporaryDigitsSpan = (stackalloc char[16]);
        var currentDigitCount = 0;

        while (totalCalculatedValue > 0)
        {
            var remainderValue = int.CreateTruncating((totalCalculatedValue % 14UL));
            totalCalculatedValue /= 14UL;

            temporaryDigitsSpan[currentDigitCount] = ValidAlphabet[remainderValue];
            currentDigitCount++;
        }

        var neededTotalLength = currentDigitCount + (includeHashPrefix ? 1 : 0);

        if (destinationSpan.Length < neededTotalLength)
        {
            totalCharactersWritten = 0;
            return false;
        }

        var currentDestinationIndex = 0;

        if (includeHashPrefix)
        {
            destinationSpan[currentDestinationIndex] = '#';
            currentDestinationIndex++;
        }

        for (
            var reverseReadIndex = currentDigitCount - 1;
            reverseReadIndex >= 0;
            reverseReadIndex--
        )
        {
            destinationSpan[currentDestinationIndex] = temporaryDigitsSpan[reverseReadIndex];
            currentDestinationIndex++;
        }

        totalCharactersWritten = neededTotalLength;
        return true;
    }

    private static bool TryFormatZero(
        Span<char> destinationSpan,
        out int totalCharactersWritten,
        bool includeHashPrefix
    )
    {
        var neededLength = includeHashPrefix ? 2 : 1;
        if (destinationSpan.Length < neededLength)
        {
            totalCharactersWritten = 0;
            return false;
        }

        var writeIndex = 0;
        if (includeHashPrefix)
            destinationSpan[writeIndex++] = '#';

        destinationSpan[writeIndex] = '0';
        totalCharactersWritten = neededLength;
        return true;
    }

    /// <summary>
    /// Executes the <c language="csharp">ToFormattedString</c> operation.
    /// </summary>
    public string ToFormattedString(bool includeHashPrefix = true)
    {
        if (!CanRepresentAsTag)
            throw new InvalidOperationException("This logic long cannot be represented as a tag.");

        var totalCalculatedValue = LowUInt32 * 256UL + HighUInt32;

        if (totalCalculatedValue == 0)
            return includeHashPrefix ? "#0" : "0";

        var calculatedDigitCount = 0;
        var temporaryValueForCounting = totalCalculatedValue;

        do
        {
            calculatedDigitCount++;
            temporaryValueForCounting /= 14UL;
        } while (temporaryValueForCounting > 0);

        var neededTotalLength = calculatedDigitCount + (includeHashPrefix ? 1 : 0);

        return string.Create(
            neededTotalLength,
            (totalCalculatedValue, includeHashPrefix, calculatedDigitCount),
            static (destinationSpan, formatState) =>
            {
                var currentDestinationIndex = 0;

                if (formatState.includeHashPrefix)
                {
                    destinationSpan[currentDestinationIndex] = '#';
                    currentDestinationIndex++;
                }

                var currentValueToFormat = formatState.totalCalculatedValue;

                for (
                    var reverseWriteIndex = formatState.calculatedDigitCount - 1;
                    reverseWriteIndex >= 0;
                    reverseWriteIndex--
                )
                {
                    var remainderValue = int.CreateTruncating((currentValueToFormat % 14UL));
                    currentValueToFormat /= 14UL;
                    destinationSpan[currentDestinationIndex + reverseWriteIndex] = ValidAlphabet[
                        remainderValue
                    ];
                }
            }
        );
    }

    /// <summary>
    /// Executes the <c language="csharp">Equals</c> operation.
    /// </summary>
    public bool Equals(LongId logicLong) =>
        HighInt32 == logicLong.HighInt32 && LowInt32 == logicLong.LowInt32;

    /// <summary>
    /// Executes the <c language="csharp">Equals</c> operation.
    /// </summary>
    public override bool Equals(
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? otherObject
    ) => otherObject is LongId logicLong && Equals(logicLong);

    /// <summary>
    /// Gets <c language="csharp">HashCode</c>.
    /// </summary>
    public override int GetHashCode() => HashCode.Combine(HighInt32, LowInt32);

    /// <summary>
    /// Executes the <c language="csharp">ToString</c> operation.
    /// </summary>
    public override string ToString() =>
        CanRepresentAsTag ? ToFormattedString() : $"{HighInt32}-{LowInt32}";

    /// <summary>
    /// Applies the <c language="csharp">==</c> operator.
    /// </summary>
    public static bool operator ==(LongId leftLongId, LongId rightLongId) =>
        leftLongId.Equals(rightLongId);

    /// <summary>
    /// Applies the <c language="csharp">!=</c> operator.
    /// </summary>
    public static bool operator !=(LongId leftLongId, LongId rightLongId) =>
        !leftLongId.Equals(rightLongId);

    /// <summary>
    /// Applies the <c language="csharp">++</c> operator.
    /// </summary>
    public static LongId operator ++(LongId logicLong) => logicLong + 1UL;

    /// <summary>
    /// Applies the <c language="csharp">--</c> operator.
    /// </summary>
    public static LongId operator --(LongId logicLong) => logicLong - 1UL;

    /// <summary>
    /// Applies the <c language="csharp">+</c> operator.
    /// </summary>
    public static LongId operator +(LongId leftLongId, ulong rightValue)
    {
        unchecked
        {
            var addedValue = leftLongId.AsUInt64 + rightValue;
            var newHighInt32 = int.CreateTruncating((addedValue >> 32));
            var newLowInt32 = int.CreateTruncating(addedValue);

            return new LongId(newHighInt32, newLowInt32);
        }
    }

    /// <summary>
    /// Applies the <c language="csharp">-</c> operator.
    /// </summary>
    public static LongId operator -(LongId leftLongId, ulong rightValue)
    {
        unchecked
        {
            var subtractedValue = leftLongId.AsUInt64 - rightValue;
            var newHighInt32 = int.CreateTruncating((subtractedValue >> 32));
            var newLowInt32 = int.CreateTruncating(subtractedValue);

            return new LongId(newHighInt32, newLowInt32);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char NormalizeTagCharacter(char inputCharacter)
    {
        if (uint.CreateTruncating((inputCharacter - 'a')) <= ('z' - 'a'))
            inputCharacter = Convert.ToChar(inputCharacter - 32);

        if (inputCharacter == 'O')
            inputCharacter = '0';

        return inputCharacter;
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
