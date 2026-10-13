using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.ChainOffers.Chapters;

/// <summary>Records that the chain offer chapters were shown.</summary>
/// <param name="ChainOfferId">The chain offer id.</param>
public sealed record MarkChainOfferSeenCommand(int ChainOfferId) : CodecCommand<MarkChainOfferSeenCommand>, ICommandCodecProvider<MarkChainOfferSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkChainOfferSeenCommand> Codec { get; } = Codecs.Command(
        type: 624,
        Codecs.Composite(Codecs.VarInt, static command => command.ChainOfferId, static (chainOfferId) => new MarkChainOfferSeenCommand(chainOfferId)),
        CommandLayout.MetadataFirst
    );
}
