using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Decorations;

/// <summary>A player-made decoration design: its picture and who made it.</summary>
/// <param name="Image">Encoded picture of the design; null when the design is sent without one.</param>
/// <param name="Author">Display name of the player who made the design.</param>
/// <param name="Value0">First number after the author name; its meaning is not identified.</param>
/// <param name="Value1">Second number after the author name; not identified.</param>
/// <param name="Value2">Third number after the author name; not identified.</param>
/// <param name="AuthorId">Account of the player who made the design, if known.</param>
/// <param name="Value3">Number that closes the design; not identified.</param>
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
