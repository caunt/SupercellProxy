using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>Completes a finished Neighborhood Object task in the selected active slot.</summary>
/// <param name="TaskSlotIndex">Zero-based index into the saved task slots of the task to turn in, written as a VarInt.</param>
public sealed record CompleteNeighborhoodObjectTaskCommand(int TaskSlotIndex) : CodecCommand<CompleteNeighborhoodObjectTaskCommand>, ICommandCodecProvider<CompleteNeighborhoodObjectTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CompleteNeighborhoodObjectTaskCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
            static command => command.TaskSlotIndex,
            static taskSlotIndex => new CompleteNeighborhoodObjectTaskCommand(taskSlotIndex)),
        CommandLayout.MetadataFirst);
}
