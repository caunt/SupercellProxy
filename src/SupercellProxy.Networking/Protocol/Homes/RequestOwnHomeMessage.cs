using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// <para>Requests the player's own home data.</para>
/// </summary>
public sealed record RequestOwnHomeMessage : CodecMessage<RequestOwnHomeMessage>, ICodecProvider<RequestOwnHomeMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestOwnHomeMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.Unknown0,
            Codecs.String,
            static message => message.UnknownString0,
            static (unknown0, unknownString0) => new RequestOwnHomeMessage
            {
                Unknown0 = unknown0,
                UnknownString0 = unknownString0,
            }
        ),
        trailingDataMessage: "Unexpected trailing RequestOwnHomeMessage data."
    );

    /// <summary>
    /// Number sent first; its meaning is not identified. The client sends -1.
    /// </summary>
    public int Unknown0 { get; init; } = -1;

    /// <summary>
    /// Text sent after it; its meaning is not identified. The client sends an empty string.
    /// </summary>
    public string UnknownString0 { get; init; } = string.Empty;
}
