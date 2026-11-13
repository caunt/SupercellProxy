using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry.Soil;

/// <summary>One named detail of a Soil analytics event.</summary>
/// <param name="Key">Name of the detail, such as notificationGameId.</param>
/// <param name="Value">Text of the detail, such as a local notification id.</param>
public sealed record SoilProperty(string? Key, string? Value) : ICodecProvider<SoilProperty>
{
    /// <inheritdoc />
    public static ICodec<SoilProperty> Codec { get; } = Codecs.Composite(Codecs.OptionalUtf8String,
        static value => value.Key,
        Codecs.OptionalUtf8String,
        static value => value.Value,
        static (key, @value) => new SoilProperty(key, value));
}
