using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Customization;

/// <summary>Requests the initial renovator customization offers.</summary>
/// <param name="OfferIndex">The offer slot.</param>
public sealed record RequestInitialCustomizationOffersCommand(int OfferIndex) : CodecCommand<RequestInitialCustomizationOffersCommand>, ICommandCodecProvider<RequestInitialCustomizationOffersCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RequestInitialCustomizationOffersCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.OfferIndex,
            static (offerIndex) => new RequestInitialCustomizationOffersCommand(offerIndex)
        )
    );
}
