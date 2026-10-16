using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Search;

/// <summary>One play-style tag a neighbourhood search filters on, sent as two numbers.</summary>
/// <param name="High">First number of the tag; its meaning is not identified.</param>
/// <param name="Low">Second number of the tag; its meaning is not identified.</param>
public sealed record NeighborhoodTag(int High, int Low) : ICodecProvider<NeighborhoodTag>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodTag> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static value => value.High,
        Codecs.Int32,
        static value => value.Low,
        static (high, low) => new NeighborhoodTag(high, low)
    );
}
