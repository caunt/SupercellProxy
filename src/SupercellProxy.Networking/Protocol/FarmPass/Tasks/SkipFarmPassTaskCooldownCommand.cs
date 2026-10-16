using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Tasks;

/// <summary>Pays diamonds to end the cooldown of a discarded Farm Pass daily task.</summary>
/// <param name="TaskIndex">The daily task index.</param>
public sealed record SkipFarmPassTaskCooldownCommand(int TaskIndex) : CodecCommand<SkipFarmPassTaskCooldownCommand>, ICommandCodecProvider<SkipFarmPassTaskCooldownCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SkipFarmPassTaskCooldownCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.TaskIndex, static taskIndex => new SkipFarmPassTaskCooldownCommand(taskIndex))
    );
}
