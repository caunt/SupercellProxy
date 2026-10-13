using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.States;

/// <summary>Pays diamonds to bring the boat back immediately.</summary>
public sealed record SpeedUpBoatCommand : CodecCommand<SpeedUpBoatCommand>, ICommandCodecProvider<SpeedUpBoatCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpBoatCommand> Codec { get; } = Codecs.Command(type: 639, Codecs.Unit(static () => new SpeedUpBoatCommand()));
}
