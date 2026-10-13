using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Collects a received donation of the selected good; zero selects the first donation.</summary>
public sealed record CollectNeighborhoodDonationCommand(int ItemGlobalId) : CodecCommand<CollectNeighborhoodDonationCommand>, ICommandCodecProvider<CollectNeighborhoodDonationCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectNeighborhoodDonationCommand> Codec { get; } = Codecs.Command(
        type: 623,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.ItemGlobalId,
            static (itemGlobalId) => new CollectNeighborhoodDonationCommand(itemGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
