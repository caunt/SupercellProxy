using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Releases;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Configures the native platform profiler and its segment reporting.</summary>
public sealed record PerformanceProfilingSettingsMessage(bool UseDefaultJsonCallback, int Mode, bool Enabled)
    : CodecMessage<PerformanceProfilingSettingsMessage>, ICodecProvider<PerformanceProfilingSettingsMessage>
{
    /// <inheritdoc />
    public static ICodec<PerformanceProfilingSettingsMessage> Codec { get; } = Codecs.Versioned(
        Release17381.Version,
        Codecs.Composite(
            Codecs.Boolean,
            static message => message.UseDefaultJsonCallback,
            Codecs.VarInt,
            static message => message.Mode,
            Codecs.Boolean,
            static message => message.Enabled,
            static (useDefaultJsonCallback, mode, enabled) => new PerformanceProfilingSettingsMessage(useDefaultJsonCallback, mode, enabled)
        ),
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.Mode,
            Codecs.Boolean,
            static message => message.Enabled,
            Codecs.Boolean,
            static message => message.UseDefaultJsonCallback,
            static (mode, enabled, useDefaultJsonCallback) => new PerformanceProfilingSettingsMessage(useDefaultJsonCallback, mode, enabled)
        )
    );
}
