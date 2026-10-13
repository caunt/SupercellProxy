using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Derby;

/// <summary>The avatar's bingo and bunny counters for the running and previous derby, in native field order.</summary>
public sealed record AvatarDerbyMinigameCounters : ICodecProvider<AvatarDerbyMinigameCounters>
{
    /// <inheritdoc />
    public static ICodec<AvatarDerbyMinigameCounters> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
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
        }
    );

    /// <summary>Gets the bingo lines completed in the running derby.</summary>
    public int CurrentBingoCount { get; init; }

    /// <summary>Gets the bingo value of the running derby.</summary>
    public int CurrentBingoValue { get; init; }

    /// <summary>Gets the bunnies found in the running derby.</summary>
    public int CurrentBunnyCount { get; init; }

    /// <summary>Gets the bingo lines completed in the previous derby; native requires a previous derby instance when it is positive.</summary>
    public int PreviousBingoCount { get; init; }

    /// <summary>Gets the bingo value of the previous derby.</summary>
    public int PreviousBingoValue { get; init; }

    /// <summary>Gets the bunnies found in the previous derby; native requires a previous derby instance when it is positive.</summary>
    public int PreviousBunnyCount { get; init; }

    /// <summary>Gets the values in native field order.</summary>
    public int[] ToValues()
    {
        return [CurrentBingoCount, CurrentBingoValue, PreviousBingoCount, PreviousBingoValue, CurrentBunnyCount, PreviousBunnyCount];
    }
}
