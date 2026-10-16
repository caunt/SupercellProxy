using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Clientbound answer to a visit or home request that could not be served, for example a farm id that does not exist.
/// </summary>
/// <param name="Reason">Why the farm could not be loaded.</param>
public sealed record HomeLoadFailedMessage(HomeVisitFailureReason Reason) : CodecMessage<HomeLoadFailedMessage>, ICodecProvider<HomeLoadFailedMessage>
{
    /// <inheritdoc />
    public static ICodec<HomeLoadFailedMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.Enum<HomeVisitFailureReason>(Codecs.VarInt),
            static message => message.Reason,
            static reason => new HomeLoadFailedMessage(reason)
        ),
        trailingDataMessage: "Home-load failure has trailing data."
    );
}
