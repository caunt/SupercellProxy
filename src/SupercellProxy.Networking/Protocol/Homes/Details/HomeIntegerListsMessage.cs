using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes.Details;

/// <summary>Returns two optional, parallel integer lists associated with one home.</summary>
public sealed record HomeIntegerListsMessage(int[]? FirstValues, int[]? SecondValues, LongId? HomeId) : CodecMessage<HomeIntegerListsMessage>, ICodecProvider<HomeIntegerListsMessage>
{
    /// <inheritdoc />
    public static ICodec<HomeIntegerListsMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.Optional(Codecs.Array(Codecs.VarInt, CollectionCount.Bounded)),
            static message => message.FirstValues,
            Codecs.Optional(Codecs.Array(Codecs.VarInt, CollectionCount.Bounded)),
            static message => message.SecondValues,
            Codecs.OptionalLongId,
            static message => message.HomeId,
            static (firstValues, secondValues, homeId) => new HomeIntegerListsMessage(firstValues, secondValues, homeId)
        ),
        trailingDataMessage: "The home integer lists have trailing data."
    );

    /// <summary>Omits the home identifier and list contents from routine diagnostics.</summary>
    public override string ToString()
    {
        return nameof(HomeIntegerListsMessage);
    }
}
