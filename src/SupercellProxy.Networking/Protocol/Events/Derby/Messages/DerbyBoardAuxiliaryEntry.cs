using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Preserves one five-value entry from the board's second optional collection.</summary>
/// <param name="Value0">The first var-int of the entry; its meaning is not yet identified, so it is carried through unchanged.</param>
/// <param name="Value1">The second var-int of the entry; its meaning is not yet identified.</param>
/// <param name="Value2">The third var-int of the entry; its meaning is not yet identified.</param>
/// <param name="Value3">The fourth var-int of the entry; its meaning is not yet identified.</param>
/// <param name="Value4">The fifth and last var-int of the entry; its meaning is not yet identified.</param>
public sealed record DerbyBoardAuxiliaryEntry(int Value0, int Value1, int Value2, int Value3, int Value4) : ICodecProvider<DerbyBoardAuxiliaryEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyBoardAuxiliaryEntry> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static value => value.Value0,
        Codecs.VarInt,
        static value => value.Value1,
        Codecs.VarInt,
        static value => value.Value2,
        Codecs.VarInt,
        static value => value.Value3,
        Codecs.VarInt,
        static value => value.Value4,
        static (value0, value1, value2, value3, value4) => new DerbyBoardAuxiliaryEntry(value0, value1, value2, value3, value4));
}
