using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>Rerolls the renovator customization offers when rolls remain.</summary>
/// <param name="OfferIndex">The offer slot.</param>
public sealed record RerollCustomizationOffersCommand(int OfferIndex) : CodecCommand<RerollCustomizationOffersCommand>, ICommandCodecProvider<RerollCustomizationOffersCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RerollCustomizationOffersCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.OfferIndex, static offerIndex => new RerollCustomizationOffersCommand(offerIndex)),
        CommandLayout.MetadataFirst
    );
}
