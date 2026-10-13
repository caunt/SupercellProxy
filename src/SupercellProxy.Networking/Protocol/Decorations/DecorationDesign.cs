using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Decorations;

/// <summary>The decoration design.</summary>
/// <param name="Image">The image.</param>
/// <param name="Author">The author.</param>
/// <param name="Value0">The value0.</param>
/// <param name="Value1">The value1.</param>
/// <param name="Value2">The value2.</param>
/// <param name="AuthorId">The author id.</param>
/// <param name="Value3">The value3.</param>
public sealed record DecorationDesign(ReadOnlyMemory<byte>? Image, string? Author, int Value0, int Value1, int Value2, LongId? AuthorId, int Value3) : ICodecProvider<DecorationDesign>
{
    /// <inheritdoc />
    public static ICodec<DecorationDesign> Codec { get; } = Codecs.Composite(
        Codecs.Flagged(Codecs.OptionalByteArray, absent: null, static image => image is not null),
        static value => value.Image,
        Codecs.OptionalString,
        static value => value.Author,
        Codecs.VarInt,
        static value => value.Value0,
        Codecs.VarInt,
        static value => value.Value1,
        Codecs.VarInt,
        static value => value.Value2,
        Codecs.OptionalLongId,
        static value => value.AuthorId,
        Codecs.VarInt,
        static value => value.Value3,
        static (image, author, value0, value1, value2, authorId, value3) => new DecorationDesign(image, author, value0, value1, value2, authorId, value3)
    );
}
