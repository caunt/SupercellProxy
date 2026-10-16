using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Analytics;

/// <summary>One analytics property; native writes at most 32 key characters and 64 value characters.</summary>
/// <param name="Key">The property key, such as <c>session_name</c>.</param>
/// <param name="Value">Text of the property, such as launch for <c>session_name</c>.</param>
public sealed record AnalyticsProperty(string? Key, string? Value) : ICodecProvider<AnalyticsProperty>
{
    /// <inheritdoc />
    public static ICodec<AnalyticsProperty> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static value => value.Key,
        Codecs.OptionalString,
        static value => value.Value,
        static (key, @value) => new AnalyticsProperty(key, value)
    );

    /// <summary>Reads and writes a counted list of properties.</summary>
    internal static ICodec<ReadOnlyMemory<AnalyticsProperty>> ListCodec => field ??= Codecs.Memory(Codec);
}
