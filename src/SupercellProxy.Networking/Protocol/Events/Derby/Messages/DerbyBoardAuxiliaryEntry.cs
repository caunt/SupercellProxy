using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Messages;

/// <summary>Preserves one five-value entry from the board's second optional collection.</summary>
public sealed record DerbyBoardAuxiliaryEntry(int Value0, int Value1, int Value2, int Value3, int Value4) : ICodecProvider<DerbyBoardAuxiliaryEntry>
{
    /// <inheritdoc />
    public static ICodec<DerbyBoardAuxiliaryEntry> Codec { get; } = Codecs.Composite(
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
        static (value0, value1, value2, value3, value4) => new DerbyBoardAuxiliaryEntry(value0, value1, value2, value3, value4)
    );
}
