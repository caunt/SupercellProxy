using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Search;

/// <summary>One neighborhood tag entry.</summary>
/// <param name="High">The high.</param>
/// <param name="Low">The low.</param>
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
