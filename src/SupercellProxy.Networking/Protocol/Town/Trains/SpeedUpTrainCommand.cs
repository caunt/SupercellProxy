using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Trains;

/// <summary>Pays to bring the arriving town train in immediately.</summary>
public sealed record SpeedUpTrainCommand : CodecCommand<SpeedUpTrainCommand>, ICommandCodecProvider<SpeedUpTrainCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpTrainCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new SpeedUpTrainCommand()));
}
