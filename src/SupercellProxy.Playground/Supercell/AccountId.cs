using System.Runtime.CompilerServices;

namespace SupercellProxy.Playground.Supercell;

public readonly struct AccountId(int highInt32, int lowInt32) : IEquatable<AccountId>
{
    public static readonly AccountId Empty = default;

    private static ReadOnlySpan<char> ValidAlphabet => "0289PYLQGRJCUV";

    public int HighInt32 => highInt32;
    public int LowInt32 => lowInt32;

    public uint HighUInt32 => (uint)highInt32;
    public uint LowUInt32 => (uint)lowInt32;

    public long AsInt64 => ((long)highInt32 << 32) | LowUInt32;
    public ulong AsUInt64 => ((ulong)HighUInt32 << 32) | LowUInt32;

    public bool CanRepresentAsTag => HighUInt32 <= 255;

    public ulong TagInternalId
    {
        get
        {
            if (!CanRepresentAsTag)
                throw new InvalidOperationException("HighUInt32 must be less than or equal to 255 to be representable as an internal tag ID.");

            return LowUInt32 * 256UL + HighUInt32;
        }
    }

    public static AccountId Parse(ReadOnlySpan<char> tagStringSpan)
    {
        if (!TryParse(tagStringSpan, out var accountId))
            throw new FormatException($"The provided tag string '{tagStringSpan}' is not in a valid format.");

        return accountId;
    }

    public static bool TryParse(ReadOnlySpan<char> tagStringSpan, out AccountId resultAccountId)
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
                resultAccountId = default;
                return false;
            }

            var nextCalculatedValue = totalCalculatedValue * 14UL + (ulong)alphabetIndex;

            if (nextCalculatedValue < totalCalculatedValue)
            {
                resultAccountId = default;
                return false;
            }

            totalCalculatedValue = nextCalculatedValue;
        }

        if (!foundAnyValidDigits)
        {
            resultAccountId = default;
            return false;
        }

        var highInternalId = (uint)(totalCalculatedValue % 256UL);
        var lowInternalId = totalCalculatedValue / 256UL;

        if (lowInternalId > uint.MaxValue)
        {
            resultAccountId = default;
            return false;
        }

        resultAccountId = new AccountId((int)highInternalId, (int)lowInternalId);
        return true;
    }

    public static IEnumerable<AccountId> ParseLazily(IEnumerable<string> inputTagStrings)
    {
        foreach (var currentTagString in inputTagStrings)
        {
            if (currentTagString is not null && TryParse(currentTagString.AsSpan(), out var parsedAccountId))
                yield return parsedAccountId;
        }
    }

    public bool TryFormat(Span<char> destinationSpan, out int totalCharactersWritten, bool includeHashPrefix = true)
    {
        if (!CanRepresentAsTag)
        {
            totalCharactersWritten = 0;
            return false;
        }

        var totalCalculatedValue = (ulong)LowUInt32 * 256UL + HighUInt32;

        if (totalCalculatedValue == 0)
        {
            var neededLengthForZeroValue = includeHashPrefix ? 2 : 1;

            if (destinationSpan.Length < neededLengthForZeroValue)
            {
                totalCharactersWritten = 0;
                return false;
            }

            var currentWriteIndex = 0;

            if (includeHashPrefix)
            {
                destinationSpan[currentWriteIndex] = '#';
                currentWriteIndex++;
            }

            destinationSpan[currentWriteIndex] = '0';
            totalCharactersWritten = neededLengthForZeroValue;
            return true;
        }

        var temporaryDigitsSpan = (stackalloc char[16]);
        var currentDigitCount = 0;

        while (totalCalculatedValue > 0)
        {
            var remainderValue = (int)(totalCalculatedValue % 14UL);
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

        for (var reverseReadIndex = currentDigitCount - 1; reverseReadIndex >= 0; reverseReadIndex--)
        {
            destinationSpan[currentDestinationIndex] = temporaryDigitsSpan[reverseReadIndex];
            currentDestinationIndex++;
        }

        totalCharactersWritten = neededTotalLength;
        return true;
    }

    public string ToFormattedString(bool includeHashPrefix = true)
    {
        if (!CanRepresentAsTag)
            throw new InvalidOperationException("This account ID cannot be represented as a tag.");

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

        return string.Create(neededTotalLength, (totalCalculatedValue, includeHashPrefix, calculatedDigitCount), static (destinationSpan, formatState) =>
        {
            var currentDestinationIndex = 0;

            if (formatState.includeHashPrefix)
            {
                destinationSpan[currentDestinationIndex] = '#';
                currentDestinationIndex++;
            }

            var currentValueToFormat = formatState.totalCalculatedValue;

            for (var reverseWriteIndex = formatState.calculatedDigitCount - 1; reverseWriteIndex >= 0; reverseWriteIndex--)
            {
                var remainderValue = (int)(currentValueToFormat % 14UL);
                currentValueToFormat /= 14UL;
                destinationSpan[currentDestinationIndex + reverseWriteIndex] = ValidAlphabet[remainderValue];
            }
        });
    }

    public bool Equals(AccountId otherAccountId) => highInt32 == otherAccountId.HighInt32 && lowInt32 == otherAccountId.LowInt32;
    public override bool Equals(object? otherObject) => otherObject is AccountId otherAccountId && Equals(otherAccountId);
    public override int GetHashCode() => HashCode.Combine(highInt32, lowInt32);
    public override string ToString() => CanRepresentAsTag ? ToFormattedString() : $"{highInt32}-{lowInt32}";

    public static bool operator ==(AccountId leftAccountId, AccountId rightAccountId) => leftAccountId.Equals(rightAccountId);
    public static bool operator !=(AccountId leftAccountId, AccountId rightAccountId) => !leftAccountId.Equals(rightAccountId);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char NormalizeTagCharacter(char inputCharacter)
    {
        if ((uint)(inputCharacter - 'a') <= ('z' - 'a'))
            inputCharacter = (char)(inputCharacter - 32);

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
            _ => -1
        };

        return alphabetIndex >= 0;
    }
}
