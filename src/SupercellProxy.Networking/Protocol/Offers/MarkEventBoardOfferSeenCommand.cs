using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers;

/// <summary>Records that an event board offer was shown.</summary>
/// <param name="OfferId">The offer id.</param>
public sealed record MarkEventBoardOfferSeenCommand(int OfferId) : CodecCommand<MarkEventBoardOfferSeenCommand>, ICommandCodecProvider<MarkEventBoardOfferSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkEventBoardOfferSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.OfferId, static (offerId) => new MarkEventBoardOfferSeenCommand(offerId)),
        CommandLayout.MetadataFirst
    );
}
