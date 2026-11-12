using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Represents a map-game participant emblem.</summary>
/// <param name="Background">Number of the background option the player chose, sent first.</param>
/// <param name="Pattern">Number of the pattern option drawn over the background, sent second.</param>
/// <param name="Symbol">Number of the symbol option shown on top, sent last.</param>
public sealed record MapGamePawnEmblem(int Background, int Pattern, int Symbol) : ICodecProvider<MapGamePawnEmblem>
{
    /// <inheritdoc />
    public static ICodec<MapGamePawnEmblem> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static value => value.Background,
        Codecs.VarInt,
        static value => value.Pattern,
        Codecs.VarInt,
        static value => value.Symbol,
        static (background, pattern, symbol) => new MapGamePawnEmblem(background, pattern, symbol));
}
