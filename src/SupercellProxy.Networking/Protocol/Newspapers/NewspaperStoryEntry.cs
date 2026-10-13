using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// Defines the Newspaper Story Entry contract.
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
/// Defines the Flag contract.
/// </summary>
/// <summary>
/// Defines the Text0 contract.
/// </summary>
/// <summary>
/// Defines the Text1 contract.
/// </summary>
/// <summary>
/// Defines the Text2 contract.
/// </summary>
/// <summary>
/// Defines the Text3 contract.
/// </summary>
/// <summary>
/// Defines the Text4 contract.
/// </summary>
public sealed record NewspaperStoryEntry(int Value0, int Value1, int Value2, bool Flag, string? Text0, string? Text1, string? Text2, string? Text3, string? Text4) : ICodecProvider<NewspaperStoryEntry>
{
    /// <inheritdoc />
    public static ICodec<NewspaperStoryEntry> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Value0,
        Codecs.VarInt,
        static value => value.Value1,
        Codecs.VarInt,
        static value => value.Value2,
        Codecs.Boolean,
        static value => value.Flag,
        Codecs.OptionalString,
        static value => value.Text0,
        Codecs.OptionalString,
        static value => value.Text1,
        Codecs.OptionalString,
        static value => value.Text2,
        Codecs.OptionalString,
        static value => value.Text3,
        Codecs.OptionalString,
        static value => value.Text4,
        static (value0, value1, value2, flag, text0, text1, text2, text3, text4) => new NewspaperStoryEntry(value0, value1, value2, flag, text0, text1, text2, text3, text4)
    );
}
