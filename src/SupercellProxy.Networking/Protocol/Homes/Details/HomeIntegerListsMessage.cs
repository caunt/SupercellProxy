using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes.Details;

/// <summary>Returns two optional, parallel integer lists associated with one home.</summary>
/// <param name="FirstValues">First value of each pair, in order; <see langword="null"/> when the wire marks the list absent. The meaning of the values is not yet identified.</param>
/// <param name="SecondValues">Second value of each pair, matched to <see cref="FirstValues"/> by index; pairs stop at the shorter of the two lists.</param>
/// <param name="HomeId">Home the lists belong to, answering an earlier <see cref="RequestHomeIntegerListsMessage"/>; <see langword="null"/> when absent.</param>
public sealed record HomeIntegerListsMessage(int[]? FirstValues, int[]? SecondValues, LongId? HomeId) : CodecMessage<HomeIntegerListsMessage>, ICodecProvider<HomeIntegerListsMessage>
{
    /// <inheritdoc />
    public static ICodec<HomeIntegerListsMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.Optional(Codecs.Array(Codecs.VarInt, CollectionCount.Bounded)),
            static message => message.FirstValues,
            Codecs.Optional(Codecs.Array(Codecs.VarInt, CollectionCount.Bounded)),
            static message => message.SecondValues,
            Codecs.OptionalLongId,
            static message => message.HomeId,
            static (firstValues, secondValues, homeId) => new HomeIntegerListsMessage(firstValues, secondValues, homeId)),
        trailingDataMessage: "The home integer lists have trailing data.");

    /// <summary>Omits the home identifier and list contents from routine diagnostics.</summary>
    /// <returns>Always the type name <c>HomeIntegerListsMessage</c>, whatever the message holds.</returns>
    public override string ToString()
    {
        return nameof(HomeIntegerListsMessage);
    }
}
