using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Circus;

/// <summary>Receives the circus train once its arrival timer has finished.</summary>
public sealed record ReceiveCircusTrainCommand : CodecCommand<ReceiveCircusTrainCommand>, ICommandCodecProvider<ReceiveCircusTrainCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ReceiveCircusTrainCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new ReceiveCircusTrainCommand()));
}
