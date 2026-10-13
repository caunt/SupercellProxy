using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>One audio analytics property entry.</summary>
/// <param name="Key">The key.</param>
/// <param name="Value">The value.</param>
public sealed record AudioAnalyticsProperty(string? Key, string? Value) : ICodecProvider<AudioAnalyticsProperty>
{
    /// <inheritdoc />
    public static ICodec<AudioAnalyticsProperty> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static value => value.Key,
        Codecs.OptionalString,
        static value => value.Value,
        static (key, @value) => new AudioAnalyticsProperty(key, value)
    );
}
