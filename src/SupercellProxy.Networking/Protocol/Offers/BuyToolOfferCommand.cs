using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers;

/// <summary>Buys the current tool offer.</summary>
public sealed record BuyToolOfferCommand : CodecCommand<BuyToolOfferCommand>, ICommandCodecProvider<BuyToolOfferCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyToolOfferCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new BuyToolOfferCommand()));
}
