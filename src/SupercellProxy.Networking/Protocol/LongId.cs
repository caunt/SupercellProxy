using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol;

/// <summary>
/// The game's 64-bit id, sent as two 32-bit words: accounts, homes, neighbourhoods and many objects are identified this way. Account
/// and home ids with a small high word also have a player-facing tag form such as <c>#L8RRJJLGV</c>.
/// </summary>
/// <remarks>
/// The high word is the shard (database) the id lives on and the low word the row within it. A tag is the number
/// <c>low * 256 + high</c> written in base 14 with the letters <c>0289PYLQGRJCUV</c>.
/// </remarks>
[StructLayout(LayoutKind.Auto)]
public readonly record struct LongId : IEquatable<LongId>
{
    /// <summary>
    /// The all-zero id, which the protocol uses for "no id".
    /// </summary>
    public static readonly LongId Empty;
    /// <summary>Initializes a new instance of the <see cref="LongId"/> struct. Creates an id from its two 32-bit words.</summary>
    /// <param name="highInt32">The shard word, sent first on the wire; any value is accepted, but only 0 to 255 leaves the id a player tag form.</param>
    /// <param name="lowInt32">The row word within the shard, sent second; any value is accepted and is read as unsigned when the id is combined into 64 bits.</param>
    public LongId(int highInt32, int lowInt32)
    {
        HighInt32 = highInt32;
        LowInt32 = lowInt32;
    }

    private static ReadOnlySpan<char> ValidAlphabet => "0289PYLQGRJCUV";

    /// <summary>
    /// Gets the first word on the wire: the shard the id belongs to.
    /// </summary>
    /// <value>The shard number as a signed 32-bit integer; 0 for <see cref="Empty"/>, and small non-negative numbers for real account and home ids.</value>
    public int HighInt32 { get; }

    /// <summary>
    /// Gets the second word on the wire: the id's number within its shard.
    /// </summary>
    /// <value>The row number as a signed 32-bit integer; rows past <see cref="int.MaxValue"/> appear negative here and positive in <see cref="LowUInt32"/>.</value>
    public int LowInt32 { get; }

    /// <summary>
    /// Gets the first (shard) word read as an unsigned number.
    /// </summary>
    /// <value>The same 32 bits as <see cref="HighInt32"/>, from 0 to <see cref="uint.MaxValue"/>; a value of at most 255 lets the id be written as a tag.</value>
    public uint HighUInt32 => uint.CreateTruncating(HighInt32);

    /// <summary>
    /// Gets the second (row) word read as an unsigned number.
    /// </summary>
    /// <value>The same 32 bits as <see cref="LowInt32"/>, from 0 to <see cref="uint.MaxValue"/>; it fills the lower half of <see cref="AsInt64"/> and <see cref="AsUInt64"/>.</value>
    public uint LowUInt32 => uint.CreateTruncating(LowInt32);

    /// <summary>
    /// Gets both words as one signed number, high word first; the form the code stores account and home ids in.
    /// </summary>
    /// <value>The shard word shifted into the upper 32 bits with the unsigned row word below it; negative only when the shard word is negative.</value>
    public long AsInt64 => (long.CreateTruncating(HighInt32) << 32) | LowUInt32;

    /// <summary>
    /// Gets both words as one unsigned number, high word first; used for arithmetic on ids.
    /// </summary>
    /// <value>The unsigned shard word in the upper 32 bits and the unsigned row word below it, so adding 1 to a row of <see cref="uint.MaxValue"/> carries into the shard.</value>
    public ulong AsUInt64 => (ulong.CreateTruncating(HighUInt32) << 32) | LowUInt32;

    /// <summary>
    /// Gets whether the id has a player tag form, which needs a shard number of at most 255.
    /// </summary>
    /// <value><see langword="true"/> if <see cref="HighUInt32"/> is 255 or less; otherwise, <see langword="false"/>.</value>
    /// <remarks>When the value is <see langword="false"/>, formatting the id as a tag fails.</remarks>
    public bool CanRepresentAsTag => HighUInt32 <= 255;

    /// <summary>
    /// Gets the number a player tag encodes: row times 256 plus shard.
    /// </summary>
    /// <value>The value whose base-14 digits form the tag, computed from <see cref="LowUInt32"/> and <see cref="HighUInt32"/>; 0 for <see cref="Empty"/>.</value>
    /// <exception cref="InvalidOperationException">The shard number is above 255, so the id has no tag.</exception>
    [JsonPropertyName("TagInternalId")]
    public ulong TagInternalId => !CanRepresentAsTag
                ? throw new InvalidOperationException(message: "HighUInt32 must be less than or equal to 255 to be representable as an public tag ID.")
                : (LowUInt32 * 256UL) + HighUInt32;

    /// <summary>
    /// Moves an id forward by an amount, treating both words as one 64-bit number, so the row number carries into the shard word.
    /// Wraps around instead of overflowing.
    /// </summary>
    /// <param name="leftLongId">Starting id.</param>
    /// <param name="rightValue">How far to move.</param>
    /// <returns>The id whose 64-bit value is <paramref name="leftLongId"/>'s plus <paramref name="rightValue"/>, modulo 2^64.</returns>
    public static LongId operator +(LongId leftLongId, ulong rightValue)
    {
        unchecked
        {
            ulong addedValue = leftLongId.AsUInt64 + rightValue;
            int newHighInt32 = int.CreateTruncating(addedValue >> 32);
            int newLowInt32 = int.CreateTruncating(addedValue);

            return new LongId(newHighInt32, newLowInt32);
        }
    }

    /// <summary>
    /// The next id after this one, as a 64-bit number.
    /// </summary>
    /// <param name="logicLong">Starting id.</param>
    /// <returns>The id one above <paramref name="logicLong"/>; a row word of all ones rolls over to 0 and raises the shard word by one.</returns>
    public static LongId operator ++(LongId logicLong)
    {
        return logicLong + 1UL;
    }

    /// <summary>
    /// Moves an id back by an amount, treating both words as one 64-bit number; wraps around instead of overflowing.
    /// </summary>
    /// <param name="leftLongId">Starting id.</param>
    /// <param name="rightValue">How far to move back.</param>
    /// <returns>The id whose 64-bit value is <paramref name="leftLongId"/>'s minus <paramref name="rightValue"/>, modulo 2^64.</returns>
    public static LongId operator -(LongId leftLongId, ulong rightValue)
    {
        unchecked
        {
            ulong subtractedValue = leftLongId.AsUInt64 - rightValue;
            int newHighInt32 = int.CreateTruncating(subtractedValue >> 32);
            int newLowInt32 = int.CreateTruncating(subtractedValue);

            return new LongId(newHighInt32, newLowInt32);
        }
    }

    /// <summary>
    /// The id just before this one, as a 64-bit number.
    /// </summary>
    /// <param name="logicLong">Starting id.</param>
    /// <returns>The id one below <paramref name="logicLong"/>; a row word of 0 rolls back to all ones and lowers the shard word by one.</returns>
    public static LongId operator --(LongId logicLong)
    {
        return logicLong - 1UL;
    }

    /// <summary>
    /// Reads a player tag such as <c>#L8RRJJLGV</c> into an id. The hash is optional, lower case is accepted, and the letter O is
    /// read as zero.
    /// </summary>
    /// <param name="tagStringSpan">Tag text as a player would type or share it.</param>
    /// <returns>The id the tag encodes: the tag number modulo 256 as the shard word and the quotient as the row word.</returns>
    /// <exception cref="FormatException">The text has no tag letters, contains other characters, or encodes a number too large.</exception>
    public static LongId Parse(ReadOnlySpan<char> tagStringSpan)
    {
        return !TryParse(tagStringSpan, out LongId logicLong)
            ? throw new FormatException($"The provided tag string '{tagStringSpan}' is not in a valid format.")
            : logicLong;
    }

    /// <summary>
    /// Reads a list of player tags one at a time as the result is enumerated, quietly skipping <see langword="null"/> entries and text that is not a
    /// valid tag.
    /// </summary>
    /// <param name="inputTagStrings">Tags in any of the forms <see cref="Parse"/> accepts.</param>
    /// <returns>A deferred sequence with one id per valid tag, in input order; it is empty when no entry is a valid tag.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="inputTagStrings"/> is <see langword="null"/>; because the method is an iterator, this is raised when enumeration starts, not at the call.</exception>
    public static IEnumerable<LongId> ParseLazily(IEnumerable<string> inputTagStrings)
    {
        ArgumentNullException.ThrowIfNull(inputTagStrings);

        foreach (string? currentTagString in inputTagStrings)
        {
            if (currentTagString is not null && TryParse(currentTagString.AsSpan(), out LongId logicLong))
                yield return logicLong;
        }
    }

    /// <summary>
    /// Reads a player tag into an id without throwing; accepts the same forms as <see cref="Parse"/>.
    /// </summary>
    /// <param name="tagStringSpan">Tag text as a player would type or share it.</param>
    /// <param name="result">The id; <see cref="Empty"/> when the text is not a valid tag.</param>
    /// <returns><see langword="true"/> if the text holds at least one character besides <c>#</c>, every such character is a tag letter, and the number fits an id; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> tagStringSpan, out LongId result)
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

        uint highInternalId = uint.CreateTruncating(totalCalculatedValue % 256UL);
        ulong lowInternalId = totalCalculatedValue / 256UL;

        if (lowInternalId > uint.MaxValue)
        {
            result = default;

            return false;
        }

        result = new LongId(int.CreateTruncating(highInternalId), int.CreateTruncating(lowInternalId));

        return true;
    }

    /// <summary>Adds an unsigned offset to this id.</summary>
    /// <param name="value">How far to move forward; any value is accepted, and the sum wraps around past the largest id.</param>
    /// <returns>A new id equal to this one plus <paramref name="value"/>, with the row word carrying into the shard word; this id is unchanged.</returns>
    public LongId Add(ulong value)
    {
        return this + value;
    }

    /// <summary>Returns the id decremented by one.</summary>
    /// <returns>A new id one below this one, borrowing from the shard word when the row word is 0; <see cref="Empty"/> wraps to the all-ones id.</returns>
    public LongId Decrement()
    {
        return this - 1UL;
    }

    /// <summary>
    /// Two ids are the same when both their shard and row words match.
    /// </summary>
    /// <param name="logicLong">Id to compare with.</param>
    /// <returns><see langword="true"/> if <paramref name="logicLong"/> has the same shard word and the same row word as this id; otherwise, <see langword="false"/>.</returns>
    public bool Equals(LongId logicLong)
    {
        return HighInt32 == logicLong.HighInt32 && LowInt32 == logicLong.LowInt32;
    }

    /// <summary>
    /// Hash built from both words, consistent with <see cref="Equals(LongId)"/>, for use as a dictionary key.
    /// </summary>
    /// <returns>A hash that is equal for equal ids within one process; it is randomized per process, so it must not be stored or sent.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(HighInt32, LowInt32);
    }

    /// <summary>Returns the id incremented by one.</summary>
    /// <returns>A new id one above this one, carrying into the shard word when the row word is all ones; this id is unchanged.</returns>
    public LongId Increment()
    {
        return this + 1UL;
    }

    /// <summary>Subtracts an unsigned offset from this id.</summary>
    /// <param name="value">How far to move back; any value is accepted, and the difference wraps around below <see cref="Empty"/>.</param>
    /// <returns>A new id equal to this one minus <paramref name="value"/>, borrowing from the shard word as needed; this id is unchanged.</returns>
    public LongId Subtract(ulong value)
    {
        return this - value;
    }

    /// <summary>
    /// Writes the id as a player tag, such as <c>#L8RRJJLGV</c>.
    /// </summary>
    /// <param name="includeHashPrefix">Whether to start the tag with <c>#</c>, as the game displays it.</param>
    /// <returns>The base-14 tag in the letters <c>0289PYLQGRJCUV</c>, with a leading <c>#</c> when <paramref name="includeHashPrefix"/> is set; <see cref="Empty"/> gives <c>#0</c> or <c>0</c>.</returns>
    /// <exception cref="InvalidOperationException">The shard number is above 255, so the id has no tag.</exception>
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

        return string.Create(neededTotalLength,
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
            });
    }

    /// <summary>
    /// The player tag with its hash when the id has one; otherwise both words as <c>high-low</c>.
    /// </summary>
    /// <returns>A tag such as <c>#L8RRJJLGV</c> when the shard word is at most 255, or the two signed words joined by a hyphen otherwise; this method never throws.</returns>
    public override string ToString()
    {
        return CanRepresentAsTag ? ToFormattedString() : $"{HighInt32}-{LowInt32}";
    }

    /// <summary>
    /// Writes the player tag into a caller-supplied buffer without allocating.
    /// </summary>
    /// <param name="destinationSpan">Buffer for the tag; 16 characters always suffice.</param>
    /// <param name="totalCharactersWritten">Characters written; 0 on failure.</param>
    /// <param name="includeHashPrefix">Whether to start the tag with <c>#</c>.</param>
    /// <returns><see langword="true"/> if the id has a tag form, because its shard word is 255 or less, and the whole tag fit into <paramref name="destinationSpan"/>; otherwise, <see langword="false"/>.</returns>
    public bool TryFormat(Span<char> destinationSpan, out int totalCharactersWritten, bool includeHashPrefix = true)
    {
        if (!CanRepresentAsTag)
        {
            totalCharactersWritten = 0;

            return false;
        }

        ulong totalCalculatedValue = (LowUInt32 * 256UL) + HighUInt32;

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
