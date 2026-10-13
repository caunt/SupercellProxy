using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.States;

/// <summary>Records that the boat destination tutorial was shown.</summary>
/// <param name="Step">The step.</param>
public sealed record MarkBoatDestinationTutorialSeenCommand(int Step) : CodecCommand<MarkBoatDestinationTutorialSeenCommand>, ICommandCodecProvider<MarkBoatDestinationTutorialSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkBoatDestinationTutorialSeenCommand> Codec { get; } = Codecs.Command(
        type: 2558,
        Codecs.Composite(Codecs.VarInt, static command => command.Step, static (step) => new MarkBoatDestinationTutorialSeenCommand(step)),
        CommandLayout.MetadataFirst
    );
}
