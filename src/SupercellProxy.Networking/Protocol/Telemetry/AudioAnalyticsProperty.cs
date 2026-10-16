using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>One named detail of an audio analytics event.</summary>
/// <param name="Key">Name of the detail, such as music_track_file.</param>
/// <param name="Value">Text of the detail, such as Music/InGameMusic.</param>
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
