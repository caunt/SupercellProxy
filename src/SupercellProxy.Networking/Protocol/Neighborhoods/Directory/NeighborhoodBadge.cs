using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Directory;

/// <summary>The neighborhood badge.</summary>
/// <param name="Background">The background.</param>
/// <param name="Pattern">The pattern.</param>
/// <param name="Symbol">The symbol.</param>
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
