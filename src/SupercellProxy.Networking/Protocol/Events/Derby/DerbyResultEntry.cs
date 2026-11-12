using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves the eight ordered values and final flag in an end-of-derby result entry.</summary>
/// <param name="Value0">The first var-int of the entry; its meaning is not yet identified, so it is carried through unchanged.</param>
/// <param name="Value1">The second var-int of the entry, read right after <paramref name="Value0"/>; its meaning is not yet identified.</param>
/// <param name="Value2">The third var-int of the entry; its meaning is not yet identified.</param>
/// <param name="Value3">The fourth var-int of the entry; its meaning is not yet identified.</param>
/// <param name="Value4">The fifth var-int of the entry; its meaning is not yet identified.</param>
/// <param name="Value5">The sixth var-int of the entry; its meaning is not yet identified.</param>
/// <param name="Value6">The seventh var-int of the entry; its meaning is not yet identified.</param>
/// <param name="Value7">The eighth and last var-int of the entry, written just before <paramref name="Flag"/>; its meaning is not yet identified.</param>
/// <param name="Flag">The Boolean byte that closes the entry; what native uses it for is not yet identified.</param>
public sealed record DerbyResultEntry(int Value0, int Value1, int Value2, int Value3, int Value4, int Value5, int Value6, int Value7, bool Flag) : ICodecProvider<DerbyResultEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyResultEntry> Codec { get; } = Codecs.Composite(Codecs.VarInt,
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
        Codecs.VarInt,
        static value => value.Value7,
        Codecs.Boolean,
        static value => value.Flag,
        static (value0, value1, value2, value3, value4, value5, value6, value7, flag) => new DerbyResultEntry(value0, value1, value2, value3, value4, value5, value6, value7, flag));
}
