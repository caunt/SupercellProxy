using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Preserves the board reply's optional six-value supplement.</summary>
/// <param name="Value0">The first var-int of the supplement; its meaning is not yet identified, so it is carried through unchanged.</param>
/// <param name="Value1">The second var-int of the supplement; its meaning is not yet identified.</param>
/// <param name="Value2">The third var-int of the supplement; its meaning is not yet identified.</param>
/// <param name="Value3">The fourth var-int of the supplement; its meaning is not yet identified.</param>
/// <param name="Value4">The fifth var-int of the supplement; its meaning is not yet identified.</param>
/// <param name="Value5">The sixth var-int, which also ends the board reply; its meaning is not yet identified.</param>
public sealed record DerbyBoardSupplement(int Value0, int Value1, int Value2, int Value3, int Value4, int Value5) : ICodecProvider<DerbyBoardSupplement>
{
    /// <inheritdoc />
    public static ICodec<DerbyBoardSupplement> Codec { get; } = Codecs.Composite(Codecs.VarInt,
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
        static (value0, value1, value2, value3, value4, value5) => new DerbyBoardSupplement(value0, value1, value2, value3, value4, value5));
}
