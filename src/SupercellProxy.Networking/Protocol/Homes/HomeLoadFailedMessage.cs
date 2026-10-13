using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Defines the Home Load Failed Message contract.
/// </summary>
/// <summary>
/// Defines the Reason contract.
/// </summary>
public sealed record HomeLoadFailedMessage(HomeVisitFailureReason Reason) : CodecMessage<HomeLoadFailedMessage>, ICodecProvider<HomeLoadFailedMessage>
{
    /// <inheritdoc />
    public static ICodec<HomeLoadFailedMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.Enum<HomeVisitFailureReason>(Codecs.VarInt),
            static message => message.Reason,
            static (reason) => new HomeLoadFailedMessage(reason)
        ),
        trailingDataMessage: "Home-load failure has trailing data."
    );
}
