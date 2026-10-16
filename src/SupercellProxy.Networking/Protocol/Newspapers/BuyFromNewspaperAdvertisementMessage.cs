using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Newspapers;

/// <summary>Buys an item from a roadside shop advertised in the newspaper.</summary>
/// <param name="AdvertisementIndex">Position of the advertisement in the current newspaper edition.</param>
/// <param name="Price">Price the advertisement showed the player.</param>
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
