using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves native reward-choice indices for ordinary, bingo and bunny thresholds.</summary>
/// <param name="ThresholdIndices">The chosen reward indices for the ordinary point thresholds, one var-int per threshold in order; up to 65536 entries, and an empty array when none are chosen.</param>
/// <param name="BingoIndices">The chosen reward indices for the bingo-line rewards, written after the threshold list in the same var-int form.</param>
/// <param name="BunnyIndices">The chosen reward indices for the bunny rewards, the last of the three lists on the wire.</param>
public sealed record DerbyRewardSelection(ImmutableArray<int> ThresholdIndices, ImmutableArray<int> BingoIndices, ImmutableArray<int> BunnyIndices) : ICodecProvider<DerbyRewardSelection>
{
    /// <inheritdoc />
    public static ICodec<DerbyRewardSelection> Codec { get; } = Codecs.Composite(Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static selection => selection.ThresholdIndices,
        Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static selection => selection.BingoIndices,
        Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
        static selection => selection.BunnyIndices,
        static (thresholdIndices, bingoIndices, bunnyIndices) => new DerbyRewardSelection(thresholdIndices, bingoIndices, bunnyIndices));
}
