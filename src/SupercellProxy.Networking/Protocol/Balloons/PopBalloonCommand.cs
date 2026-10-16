using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Balloons;

/// <summary>Pops an unpopped balloon identified by its Balloon data row.</summary>
public sealed record PopBalloonCommand(int BalloonDataGlobalId) : CodecCommand<PopBalloonCommand>, ICommandCodecProvider<PopBalloonCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PopBalloonCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.BalloonDataGlobalId, static id => new PopBalloonCommand(id)));
}
