using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Balloons;

/// <summary>Pops an unpopped balloon identified by its Balloon data row.</summary>
/// <param name="BalloonDataGlobalId">The global id of the balloon's row in the Balloon data table, written as a VarInt; it names which floating balloon the player popped.</param>
public sealed record PopBalloonCommand(int BalloonDataGlobalId) : CodecCommand<PopBalloonCommand>, ICommandCodecProvider<PopBalloonCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PopBalloonCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt, static command => command.BalloonDataGlobalId, static id => new PopBalloonCommand(id)));
}
