using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>Buys an item from a roadside shop advertised in the newspaper.</summary>
/// <param name="AdvertisementIndex">The advertisement index.</param>
/// <param name="Price">The price.</param>
public sealed record BuyFromNewspaperAdvertisementMessage(int AdvertisementIndex, int Price) : CodecMessage<BuyFromNewspaperAdvertisementMessage>, ICodecProvider<BuyFromNewspaperAdvertisementMessage>
{
    /// <inheritdoc />
    public static ICodec<BuyFromNewspaperAdvertisementMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.AdvertisementIndex,
        Codecs.VarInt,
        static message => message.Price,
        static (advertisementIndex, price) => new BuyFromNewspaperAdvertisementMessage(advertisementIndex, price)
    );
}
