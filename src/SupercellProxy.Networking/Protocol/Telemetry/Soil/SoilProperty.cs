using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Soil;

/// <summary>One soil property entry.</summary>
/// <param name="Key">The key.</param>
/// <param name="Value">The value.</param>
public sealed record SoilProperty(string? Key, string? Value) : ICodecProvider<SoilProperty>
{
    /// <inheritdoc />
    public static ICodec<SoilProperty> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static value => value.Key,
        Codecs.OptionalString,
        static value => value.Value,
        static (key, @value) => new SoilProperty(key, value)
    );
}
