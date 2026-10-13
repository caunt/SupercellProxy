using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Preserves the board reply's optional six-value supplement.</summary>
public sealed record DerbyBoardSupplement(int Value0, int Value1, int Value2, int Value3, int Value4, int Value5) : ICodecProvider<DerbyBoardSupplement>
{
    /// <inheritdoc />
    public static ICodec<DerbyBoardSupplement> Codec { get; } = Codecs.Composite(
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
        static (value0, value1, value2, value3, value4, value5) => new DerbyBoardSupplement(value0, value1, value2, value3, value4, value5)
    );
}
