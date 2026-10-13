using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Preserves one board group and its associated player identifiers.</summary>
public sealed record DerbyBoardGroupEntry(int Value0, int Value1, int Value2, int Value3, int Value4, int Value5, int Value6, LongId[] PlayerIds) : ICodecProvider<DerbyBoardGroupEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyBoardGroupEntry> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Value0,
        Codecs.VarInt,
        static value => value.Value1,
        Codecs.VarInt,
        static value => value.Value2,
        Codecs.VarInt,
        static value => value.Value3,
        Codecs.VarInt,
        static value => value.Value4,
        Codecs.VarInt,
        static value => value.Value5,
        Codecs.VarInt,
        static value => value.Value6,
        Codecs.Array(Codecs.LongId, CollectionCount.Bounded),
        static value => value.PlayerIds,
        static (value0, value1, value2, value3, value4, value5, value6, playerIds) => new DerbyBoardGroupEntry(value0, value1, value2, value3, value4, value5, value6, playerIds)
    );

    /// <summary>Omits identifiers from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(DerbyBoardGroupEntry);
    }
}
