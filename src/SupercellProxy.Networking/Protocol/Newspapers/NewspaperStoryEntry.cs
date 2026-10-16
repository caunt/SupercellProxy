using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>
/// One story printed in a newspaper edition. The managed code does not read stories; the field roles are not identified.
/// </summary>
/// <param name="Value0">First number of the story.</param>
/// <param name="Value1">Second number of the story.</param>
/// <param name="Value2">Third number of the story.</param>
/// <param name="Flag">Switch sent after the numbers.</param>
/// <param name="Text0">First text of the story.</param>
/// <param name="Text1">Second text of the story.</param>
/// <param name="Text2">Third text of the story.</param>
/// <param name="Text3">Fourth text of the story.</param>
/// <param name="Text4">Fifth text of the story.</param>
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
