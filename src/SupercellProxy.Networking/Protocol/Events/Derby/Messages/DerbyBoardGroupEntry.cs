using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Preserves one board group and its associated player identifiers.</summary>
/// <param name="Value0">The first var-int of the group; its meaning is not yet identified, so it is carried through unchanged.</param>
/// <param name="Value1">The second var-int of the group; its meaning is not yet identified.</param>
/// <param name="Value2">The third var-int of the group; its meaning is not yet identified.</param>
/// <param name="Value3">The fourth var-int of the group; its meaning is not yet identified.</param>
/// <param name="Value4">The fifth var-int of the group; its meaning is not yet identified.</param>
/// <param name="Value5">The sixth var-int of the group; its meaning is not yet identified.</param>
/// <param name="Value6">The seventh var-int of the group, written just before <paramref name="PlayerIds"/>; its meaning is not yet identified.</param>
/// <param name="PlayerIds">The account identifiers listed for the group, in wire order; an empty array when the group names nobody, and at most 65536 entries.</param>
public sealed record DerbyBoardGroupEntry(int Value0, int Value1, int Value2, int Value3, int Value4, int Value5, int Value6, ImmutableArray<LongId> PlayerIds) : ICodecProvider<DerbyBoardGroupEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyBoardGroupEntry> Codec { get; } = Codecs.Composite(Codecs.VarInt,
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
        static (value0, value1, value2, value3, value4, value5, value6, playerIds) => new DerbyBoardGroupEntry(value0, value1, value2, value3, value4, value5, value6, playerIds));

    /// <summary>Omits identifiers from diagnostic text.</summary>
    /// <returns>The type name <c>DerbyBoardGroupEntry</c> alone, without any field values, so player identifiers never reach logs.</returns>
    public override string ToString()
    {
        return nameof(DerbyBoardGroupEntry);
    }
}
