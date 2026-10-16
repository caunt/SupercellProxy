using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Objects;

/// <summary>Swaps a neighborhood object task for another one with diamonds.</summary>
/// <param name="TaskIndex">Slot of the task to swap.</param>
public sealed record SwapNeighborhoodObjectTaskCommand(int TaskIndex) : CodecCommand<SwapNeighborhoodObjectTaskCommand>, ICommandCodecProvider<SwapNeighborhoodObjectTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SwapNeighborhoodObjectTaskCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.TaskIndex, static taskIndex => new SwapNeighborhoodObjectTaskCommand(taskIndex)),
        CommandLayout.MetadataFirst
    );
}
