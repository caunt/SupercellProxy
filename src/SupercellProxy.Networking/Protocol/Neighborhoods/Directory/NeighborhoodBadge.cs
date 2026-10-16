using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Directory;

/// <summary>A neighbourhood's emblem, built from a background, a pattern and a symbol.</summary>
/// <param name="Background">Emblem background the neighbourhood picked.</param>
/// <param name="Pattern">Emblem pattern the neighbourhood picked.</param>
/// <param name="Symbol">Emblem symbol the neighbourhood picked.</param>
public sealed record NeighborhoodBadge(int Background, int Pattern, int Symbol) : ICodecProvider<NeighborhoodBadge>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodBadge> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Background,
        Codecs.VarInt,
        static value => value.Pattern,
        Codecs.VarInt,
        static value => value.Symbol,
        static (background, pattern, symbol) => new NeighborhoodBadge(background, pattern, symbol)
    );
}
