using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Circus;

/// <summary>Sends the finished circus train away and resets its parts.</summary>
public sealed record DispatchCircusTrainCommand : CodecCommand<DispatchCircusTrainCommand>, ICommandCodecProvider<DispatchCircusTrainCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DispatchCircusTrainCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new DispatchCircusTrainCommand()));
}
