using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Tasks;

/// <summary>Pays diamonds to complete an in-progress Farm Pass task.</summary>
/// <param name="TaskIndex">Position of the task in its task list.</param>
public sealed record CompleteFarmPassTaskWithDiamondsCommand(int TaskIndex) : CodecCommand<CompleteFarmPassTaskWithDiamondsCommand>, ICommandCodecProvider<CompleteFarmPassTaskWithDiamondsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteFarmPassTaskWithDiamondsCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.TaskIndex, static taskIndex => new CompleteFarmPassTaskWithDiamondsCommand(taskIndex))
    );
}
