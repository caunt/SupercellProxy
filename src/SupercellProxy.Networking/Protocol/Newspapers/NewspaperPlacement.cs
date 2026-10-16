using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// One roadside shop advertisement in a newspaper edition: the advertising farm and the items it advertises.
/// </summary>
/// <param name="Index">Position of the advertisement in the edition.</param>
/// <param name="HomeId">Farm whose roadside shop is advertised; visiting it shows the shop.</param>
/// <param name="Value0">Global id of the first advertised item.</param>
/// <param name="Value1">Global id of the second advertised item.</param>
/// <param name="Value2">Global id of the third advertised item.</param>
/// <param name="Value3">Global id of the fourth advertised item.</param>
/// <param name="Flag">Switch that closes the advertisement; its meaning is not identified.</param>
public sealed record NewspaperPlacement(int Index, [property: JsonPropertyName("HomeId")] LongId HomeId, int Value0, int Value1, int Value2, int Value3, bool Flag) : ICodecProvider<NewspaperPlacement>
{
    /// <inheritdoc />
    public static ICodec<NewspaperPlacement> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Index,
        Codecs.LongId,
        static value => value.HomeId,
        Codecs.VarInt,
        static value => value.Value0,
        Codecs.VarInt,
        static value => value.Value1,
        Codecs.VarInt,
        static value => value.Value2,
        Codecs.VarInt,
        static value => value.Value3,
        Codecs.Boolean,
        static value => value.Flag,
        static (index, homeId, value0, value1, value2, value3, flag) => new NewspaperPlacement(index, homeId, value0, value1, value2, value3, flag)
    );
}
