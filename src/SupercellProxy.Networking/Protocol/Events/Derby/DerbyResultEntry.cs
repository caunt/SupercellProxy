using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Preserves the eight ordered values and final flag in an end-of-derby result entry.</summary>
public sealed record DerbyResultEntry(int Value0, int Value1, int Value2, int Value3, int Value4, int Value5, int Value6, int Value7, bool Flag) : ICodecProvider<DerbyResultEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyResultEntry> Codec { get; } = Codecs.Composite(
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
        Codecs.VarInt,
        static value => value.Value7,
        Codecs.Boolean,
        static value => value.Flag,
        static (value0, value1, value2, value3, value4, value5, value6, value7, flag) => new DerbyResultEntry(value0, value1, value2, value3, value4, value5, value6, value7, flag)
    );
}
