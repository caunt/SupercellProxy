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
    /// Gets or sets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; init; } = -1;

    /// <summary>
    /// Gets or sets the <c language="csharp">UnknownString0</c> value.
    /// </summary>
    public string UnknownString0 { get; init; } = string.Empty;
}
