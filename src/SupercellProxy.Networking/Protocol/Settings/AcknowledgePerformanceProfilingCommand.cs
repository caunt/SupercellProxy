using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Settings;

/// <summary>Acknowledges the performance profiling request.</summary>
public sealed record AcknowledgePerformanceProfilingCommand : CodecCommand<AcknowledgePerformanceProfilingCommand>, ICommandCodecProvider<AcknowledgePerformanceProfilingCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AcknowledgePerformanceProfilingCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new AcknowledgePerformanceProfilingCommand()));
}
