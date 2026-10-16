using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats.States;

/// <summary>Records that the boat destination tutorial was shown.</summary>
/// <param name="Step">Which page of the destination tutorial the player reached.</param>
public sealed record MarkBoatDestinationTutorialSeenCommand(int Step) : CodecCommand<MarkBoatDestinationTutorialSeenCommand>, ICommandCodecProvider<MarkBoatDestinationTutorialSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkBoatDestinationTutorialSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.Step, static step => new MarkBoatDestinationTutorialSeenCommand(step)),
        CommandLayout.MetadataFirst
    );
}
