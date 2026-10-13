using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Circus;

/// <summary>Moves a ready circus train part to its completed state.</summary>
/// <param name="CircusTrainPartGlobalId">Gets the circus train part game object id.</param>
public sealed record CompleteCircusTrainPartCommand(int CircusTrainPartGlobalId) : CodecCommand<CompleteCircusTrainPartCommand>, ICommandCodecProvider<CompleteCircusTrainPartCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteCircusTrainPartCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.CircusTrainPartGlobalId,
            static (circusTrainPartGlobalId) => new CompleteCircusTrainPartCommand(circusTrainPartGlobalId)
        )
    );
}
