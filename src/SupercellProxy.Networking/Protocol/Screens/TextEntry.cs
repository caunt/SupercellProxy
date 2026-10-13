using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Screens;

/// <summary>One text entry entry.</summary>
/// <param name="Text0">The text0.</param>
/// <param name="Text1">The text1.</param>
/// <param name="Text2">The text2.</param>
/// <param name="Text3">The text3.</param>
/// <param name="Text4">The text4.</param>
/// <param name="Text5">The text5.</param>
/// <param name="Text6">The text6.</param>
/// <param name="Value">The value.</param>
public sealed record TextEntry(string? Text0, string? Text1, string? Text2, string? Text3, string? Text4, string? Text5, string? Text6, int Value) : ICodecProvider<TextEntry>
{
    /// <inheritdoc />
    public static ICodec<TextEntry> Codec { get; } = Codecs.Composite(
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
        Codecs.OptionalString,
        static value => value.Text5,
        Codecs.OptionalString,
        static value => value.Text6,
        Codecs.Int32,
        static value => value.Value,
        static (text0, text1, text2, text3, text4, text5, text6, @value) => new TextEntry(text0, text1, text2, text3, text4, text5, text6, value)
    );
}
