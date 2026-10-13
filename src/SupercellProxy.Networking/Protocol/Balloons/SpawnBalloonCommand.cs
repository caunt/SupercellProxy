using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Balloons;

/// <summary>Lets the delivery boy's balloon appear.</summary>
public sealed record SpawnBalloonCommand : CodecCommand<SpawnBalloonCommand>, ICommandCodecProvider<SpawnBalloonCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpawnBalloonCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new SpawnBalloonCommand()));
}
