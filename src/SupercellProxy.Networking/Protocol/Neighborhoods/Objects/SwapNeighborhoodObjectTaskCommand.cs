using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Objects;

/// <summary>Swaps a neighborhood object task for another one with diamonds.</summary>
/// <param name="TaskIndex">The task index.</param>
public sealed record SwapNeighborhoodObjectTaskCommand(int TaskIndex) : CodecCommand<SwapNeighborhoodObjectTaskCommand>, ICommandCodecProvider<SwapNeighborhoodObjectTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SwapNeighborhoodObjectTaskCommand> Codec { get; } = Codecs.Command(
        type: 611,
        Codecs.Composite(Codecs.VarInt, static command => command.TaskIndex, static (taskIndex) => new SwapNeighborhoodObjectTaskCommand(taskIndex)),
        CommandLayout.MetadataFirst
    );
}
