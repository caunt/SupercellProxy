using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Tasks;

/// <summary>Refreshes the Farm Pass building tasks.</summary>
public sealed record RefreshFarmPassTasksCommand : CodecCommand<RefreshFarmPassTasksCommand>, ICommandCodecProvider<RefreshFarmPassTasksCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RefreshFarmPassTasksCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new RefreshFarmPassTasksCommand()));
}
