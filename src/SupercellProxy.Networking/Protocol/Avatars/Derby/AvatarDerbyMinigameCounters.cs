using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Derby;

/// <summary>The avatar's bingo and bunny counters for the running and previous derby, in native field order.</summary>
public sealed record AvatarDerbyMinigameCounters : ICodecProvider<AvatarDerbyMinigameCounters>
{
    /// <inheritdoc />
    public static ICodec<AvatarDerbyMinigameCounters> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static counters => counters.CurrentBingoCount,
        Codecs.VarInt,
        static counters => counters.CurrentBingoValue,
        Codecs.VarInt,
        static counters => counters.PreviousBingoCount,
        Codecs.VarInt,
        static counters => counters.PreviousBingoValue,
        Codecs.VarInt,
        static counters => counters.CurrentBunnyCount,
        Codecs.VarInt,
        static counters => counters.PreviousBunnyCount,
        static (currentBingoCount, currentBingoValue, previousBingoCount, previousBingoValue, currentBunnyCount, previousBunnyCount) => new AvatarDerbyMinigameCounters
        {
            CurrentBingoCount = currentBingoCount,
            CurrentBingoValue = currentBingoValue,
            PreviousBingoCount = previousBingoCount,
            PreviousBingoValue = previousBingoValue,
            CurrentBunnyCount = currentBunnyCount,
            PreviousBunnyCount = previousBunnyCount,
        });

    /// <summary>Gets or sets the bingo lines completed in the running derby.</summary>
    /// <value>A non-negative line count, the first VarInt of the block; 0 by default and outside a bingo derby.</value>
    public int CurrentBingoCount { get; init; }

    /// <summary>Gets or sets the bingo value of the running derby.</summary>
    /// <value>The second VarInt of the block; 0 by default. What the value counts beyond the line count is not yet identified.</value>
    public int CurrentBingoValue { get; init; }

    /// <summary>Gets or sets the bunnies found in the running derby.</summary>
    /// <value>A non-negative bunny count, the fifth VarInt of the block; 0 by default and outside a bunny derby.</value>
    public int CurrentBunnyCount { get; init; }

    /// <summary>Gets or sets the bingo lines completed in the previous derby.</summary>
    /// <value>A non-negative line count, the third VarInt of the block; 0 by default and when the previous derby was not a bingo derby.</value>
    public int PreviousBingoCount { get; init; }

    /// <summary>Gets or sets the bingo value of the previous derby.</summary>
    /// <value>The fourth VarInt of the block, the previous derby's counterpart of <see cref="CurrentBingoValue"/>; 0 by default.</value>
    public int PreviousBingoValue { get; init; }

    /// <summary>Gets or sets the bunnies found in the previous derby.</summary>
    /// <value>A non-negative bunny count, the sixth and last VarInt of the block; 0 by default and when the previous derby was not a bunny derby.</value>
    public int PreviousBunnyCount { get; init; }

    /// <summary>Gets the values in native field order.</summary>
    /// <returns>A new six-element array: current bingo count, current bingo value, previous bingo count, previous bingo value, current bunny count, previous bunny count.</returns>
    public int[] ToValues()
    {
        return [CurrentBingoCount, CurrentBingoValue, PreviousBingoCount, PreviousBingoValue, CurrentBunnyCount, PreviousBunnyCount];
    }
}
