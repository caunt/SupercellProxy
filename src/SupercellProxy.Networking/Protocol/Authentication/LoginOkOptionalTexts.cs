using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Authentication;

/// <summary>
/// The Release17286-family third LoginOkMessage block: four texts, each behind
/// its own presence flag. The analyzed current Clash Royale 20963 layout has
/// five texts before an optional permission set and four after it; see
/// docs/clash-royale/loginok-20963.md.
/// </summary>
/// <param name="First">The first text.</param>
/// <param name="Second">The second text.</param>
/// <param name="Third">The third text.</param>
/// <param name="Fourth">The fourth text.</param>
public sealed record LoginOkOptionalTexts(string? First, string? Second, string? Third, string? Fourth) : ICodecProvider<LoginOkOptionalTexts>
{
    /// <inheritdoc />
    public static ICodec<LoginOkOptionalTexts> Codec { get; } = Codecs.Composite(
        Codecs.Flagged(Codecs.OptionalString, absent: null, static text => text is not null),
        static value => value.First,
        Codecs.Flagged(Codecs.OptionalString, absent: null, static text => text is not null),
        static value => value.Second,
        Codecs.Flagged(Codecs.OptionalString, absent: null, static text => text is not null),
        static value => value.Third,
        Codecs.Flagged(Codecs.OptionalString, absent: null, static text => text is not null),
        static value => value.Fourth,
        static (first, second, third, fourth) => new LoginOkOptionalTexts(first, second, third, fourth)
    );
}
