using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Defines the Newspaper Placement contract.
/// </summary>
/// <summary>
/// Defines the Index contract.
/// </summary>
/// <summary>
/// Defines the Home Id contract.
/// </summary>
/// <summary>
/// Defines the Value0 contract.
/// </summary>
/// <summary>
/// Defines the Value1 contract.
/// </summary>
/// <summary>
/// Defines the Value2 contract.
/// </summary>
/// <summary>
/// Defines the Value3 contract.
/// </summary>
/// <summary>
/// Defines the Flag contract.
/// </summary>
public sealed record NewspaperPlacement(
    int Index,
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeId")] LongId HomeId,
    int Value0,
    int Value1,
    int Value2,
    int Value3,
    bool Flag
) : ICodecProvider<NewspaperPlacement>
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
