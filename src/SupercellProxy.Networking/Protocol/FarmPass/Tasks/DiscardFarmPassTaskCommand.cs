using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Tasks;

/// <summary>Discards a Farm Pass daily task and starts its replacement cooldown.</summary>
/// <param name="TaskIndex">The daily task index.</param>
public sealed record DiscardFarmPassTaskCommand(int TaskIndex) : CodecCommand<DiscardFarmPassTaskCommand>, ICommandCodecProvider<DiscardFarmPassTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DiscardFarmPassTaskCommand> Codec { get; } = Codecs.Command(
        type: 374,
        Codecs.Composite(Codecs.VarInt, static command => command.TaskIndex, static (taskIndex) => new DiscardFarmPassTaskCommand(taskIndex))
    );
}
