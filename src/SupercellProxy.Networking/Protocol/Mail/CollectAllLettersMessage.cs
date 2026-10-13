using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Mail;

/// <summary>Collects all letters in the mailbox.</summary>
/// <param name="Shuffle">The shuffle.</param>
public sealed record CollectAllLettersMessage(bool Shuffle) : CodecMessage<CollectAllLettersMessage>, ICodecProvider<CollectAllLettersMessage>
{
    /// <inheritdoc />
    public static ICodec<CollectAllLettersMessage> Codec { get; } = Codecs.Composite(Codecs.Boolean, static message => message.Shuffle, static (shuffle) => new CollectAllLettersMessage(shuffle));
}
