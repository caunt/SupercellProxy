using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>Represents a map-game participant emblem.</summary>
public sealed record MapGamePawnEmblem(int Background, int Pattern, int Symbol) : ICodecProvider<MapGamePawnEmblem>
{
    /// <inheritdoc />
    public static ICodec<MapGamePawnEmblem> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Background,
        Codecs.VarInt,
        static value => value.Pattern,
        Codecs.VarInt,
        static value => value.Symbol,
        static (background, pattern, symbol) => new MapGamePawnEmblem(background, pattern, symbol)
    );
}
