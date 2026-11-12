using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes.Details;

/// <summary>Requests the native pair of integer lists for an optional home identifier.</summary>
/// <param name="HomeId">Home whose lists to fetch; the server answers with a <see cref="HomeIntegerListsMessage"/> for the same id. <see langword="null"/> is sent as the absent marker, and such a request is not tracked.</param>
public sealed record RequestHomeIntegerListsMessage(LongId? HomeId) : CodecMessage<RequestHomeIntegerListsMessage>, ICodecProvider<RequestHomeIntegerListsMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestHomeIntegerListsMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.OptionalLongId, static message => message.HomeId, static homeId => new RequestHomeIntegerListsMessage(homeId)),
        trailingDataMessage: "The home integer-list request has trailing data.");

    /// <summary>Omits the home identifier from routine diagnostics.</summary>
    /// <returns>Always the type name <c>RequestHomeIntegerListsMessage</c>, without the requested home id.</returns>
    public override string ToString()
    {
        return nameof(RequestHomeIntegerListsMessage);
    }
}
