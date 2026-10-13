using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>Calls the lumberjack.</summary>
public sealed record CallLumberjackCommand : CodecCommand<CallLumberjackCommand>, ICommandCodecProvider<CallLumberjackCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CallLumberjackCommand> Codec { get; } = Codecs.Command(type: 685, Codecs.Unit(static () => new CallLumberjackCommand()));
}
