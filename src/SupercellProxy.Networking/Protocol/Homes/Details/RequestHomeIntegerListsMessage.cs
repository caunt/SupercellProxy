using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes.Details;

/// <summary>Requests the native pair of integer lists for an optional home identifier.</summary>
public sealed record RequestHomeIntegerListsMessage(LongId? HomeId) : CodecMessage<RequestHomeIntegerListsMessage>, ICodecProvider<RequestHomeIntegerListsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestHomeIntegerListsMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(Codecs.OptionalLongId, static message => message.HomeId, static (homeId) => new RequestHomeIntegerListsMessage(homeId)),
        trailingDataMessage: "The home integer-list request has trailing data."
    );

    /// <summary>Omits the home identifier from routine diagnostics.</summary>
    public override string ToString()
    {
        return nameof(RequestHomeIntegerListsMessage);
    }
}
