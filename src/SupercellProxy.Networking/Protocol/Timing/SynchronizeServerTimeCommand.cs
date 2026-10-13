using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Timing;

/// <summary>Stores the current server time and notifies the time listener when it changed.</summary>
public sealed record SynchronizeServerTimeCommand : CodecCommand<SynchronizeServerTimeCommand>, ICommandCodecProvider<SynchronizeServerTimeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SynchronizeServerTimeCommand> Codec { get; } = Codecs.Command(type: 676, Codecs.Unit(static () => new SynchronizeServerTimeCommand()));
}
