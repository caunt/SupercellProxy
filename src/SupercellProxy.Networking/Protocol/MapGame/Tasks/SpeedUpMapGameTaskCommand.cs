using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>Pays diamonds to finish the valley pawn's current task.</summary>
public sealed record SpeedUpMapGameTaskCommand : CodecCommand<SpeedUpMapGameTaskCommand>, ICommandCodecProvider<SpeedUpMapGameTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpMapGameTaskCommand> Codec { get; } = Codecs.Command(type: 553, Codecs.Unit(static () => new SpeedUpMapGameTaskCommand()));
}
