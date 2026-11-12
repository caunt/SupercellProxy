using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Requests;

/// <summary>Collects a received donation of the selected good; zero selects the first donation.</summary>
/// <param name="ItemGlobalId">Global id of the donated item to collect, written as a VarInt, or 0 for the first uncollected donation.</param>
public sealed record CollectNeighborhoodDonationCommand(int ItemGlobalId) : CodecCommand<CollectNeighborhoodDonationCommand>, ICommandCodecProvider<CollectNeighborhoodDonationCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectNeighborhoodDonationCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
            static command => command.ItemGlobalId,
            static itemGlobalId => new CollectNeighborhoodDonationCommand(itemGlobalId)),
        CommandLayout.MetadataFirst);
}
