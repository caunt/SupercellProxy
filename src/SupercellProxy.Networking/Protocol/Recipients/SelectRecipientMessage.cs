using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Recipients;

/// <summary>Sends the selected recipient's identifier.</summary>
public sealed record SelectRecipientMessage(LongId RecipientId) : CodecMessage<SelectRecipientMessage>, ICodecProvider<SelectRecipientMessage>
{
    /// <inheritdoc />
    public static ICodec<SelectRecipientMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(Codecs.LongId, static message => message.RecipientId, static recipientId => new SelectRecipientMessage(recipientId)),
        trailingDataMessage: "The recipient request has trailing data."
    );

    /// <summary>Omits the private recipient identifier from diagnostics.</summary>
    public override string ToString()
    {
        return nameof(SelectRecipientMessage);
    }
}
