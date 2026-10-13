using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves native reward-choice indices for ordinary, bingo and bunny thresholds.</summary>
public sealed record DerbyRewardSelection(int[] ThresholdIndices, int[] BingoIndices, int[] BunnyIndices) : ICodecProvider<DerbyRewardSelection>
{
    /// <inheritdoc />
    public static ICodec<DerbyRewardSelection> Codec { get; } = Codecs.Composite(
        Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static selection => selection.ThresholdIndices,
        Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static selection => selection.BingoIndices,
        Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static selection => selection.BunnyIndices,
        static (thresholdIndices, bingoIndices, bunnyIndices) => new DerbyRewardSelection(thresholdIndices, bingoIndices, bunnyIndices)
    );
}
