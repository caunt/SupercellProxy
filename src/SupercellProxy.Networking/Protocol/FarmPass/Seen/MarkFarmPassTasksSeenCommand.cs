using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Seen;

/// <summary>Marks the Farm Pass tasks as seen.</summary>
public sealed record MarkFarmPassTasksSeenCommand : CodecCommand<MarkFarmPassTasksSeenCommand>, ICommandCodecProvider<MarkFarmPassTasksSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkFarmPassTasksSeenCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new MarkFarmPassTasksSeenCommand()));
}
