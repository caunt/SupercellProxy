using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Configures the native platform profiler and its segment reporting.</summary>
/// <param name="UseDefaultJsonCallback">Whether the profiler reports through its default JSON callback, as a Boolean; written first from 1.73.81 on and last in older releases.</param>
/// <param name="Mode">The profiler mode, written as a VarInt; the meaning of its individual values is not yet identified.</param>
/// <param name="Enabled">Whether the server switches the profiler on, as a Boolean that directly follows <paramref name="Mode"/> in both layouts.</param>
public sealed record PerformanceProfilingSettingsMessage(bool UseDefaultJsonCallback, int Mode, bool Enabled)
    : CodecMessage<PerformanceProfilingSettingsMessage>, ICodecProvider<PerformanceProfilingSettingsMessage>
{
    /// <inheritdoc />
    public static ICodec<PerformanceProfilingSettingsMessage> Codec { get; } = Codecs.Versioned(Release17381.Version,
        Codecs.Composite(Codecs.Boolean,
            static message => message.UseDefaultJsonCallback,
            Codecs.VarInt,
            static message => message.Mode,
            Codecs.Boolean,
            static message => message.Enabled,
            static (useDefaultJsonCallback, mode, enabled) => new PerformanceProfilingSettingsMessage(useDefaultJsonCallback, mode, enabled)),
        Codecs.Composite(Codecs.VarInt,
            static message => message.Mode,
            Codecs.Boolean,
            static message => message.Enabled,
            Codecs.Boolean,
            static message => message.UseDefaultJsonCallback,
            static (mode, enabled, useDefaultJsonCallback) => new PerformanceProfilingSettingsMessage(useDefaultJsonCallback, mode, enabled)));
}
