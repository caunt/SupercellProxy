using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Tasks;

/// <summary>Pays diamonds to complete an in-progress Farm Pass task.</summary>
/// <param name="TaskIndex">The task index.</param>
public sealed record CompleteFarmPassTaskWithDiamondsCommand(int TaskIndex) : CodecCommand<CompleteFarmPassTaskWithDiamondsCommand>, ICommandCodecProvider<CompleteFarmPassTaskWithDiamondsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteFarmPassTaskWithDiamondsCommand> Codec { get; } = Codecs.Command(
        type: 377,
        Codecs.Composite(Codecs.VarInt, static command => command.TaskIndex, static (taskIndex) => new CompleteFarmPassTaskWithDiamondsCommand(taskIndex))
    );
}
