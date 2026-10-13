using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Recipients;

/// <summary>Acknowledges the most recently selected recipient.</summary>
public sealed record RecipientSelectedMessage : CodecMessage<RecipientSelectedMessage>, ICodecProvider<RecipientSelectedMessage>
{
    /// <inheritdoc />
    public static ICodec<RecipientSelectedMessage> Codec { get; } = Codecs.Complete(
        Codecs.Unit(static () => new RecipientSelectedMessage()),
        trailingDataMessage: "The recipient acknowledgement has trailing data."
    );
}
