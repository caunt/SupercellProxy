using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Helpers.Boy;

/// <summary>Buys one of the boy's search results.</summary>
/// <param name="OfferIndex">The chosen search result index.</param>
public sealed record SelectBoyOfferCommand(int OfferIndex) : CodecCommand<SelectBoyOfferCommand>, ICommandCodecProvider<SelectBoyOfferCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectBoyOfferCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.OfferIndex, static offerIndex => new SelectBoyOfferCommand(offerIndex))
    );
}
