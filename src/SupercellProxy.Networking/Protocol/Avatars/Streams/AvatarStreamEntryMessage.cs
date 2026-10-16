using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Streams;

/// <summary>Adds one inbox (avatar stream) entry in the game log manager.</summary>
/// <param name="Entry">The new inbox entry, typed by its kind (neighbourhood invitation, kick-out and so on).</param>
public sealed record AvatarStreamEntryMessage(AvatarStreamEntry Entry) : CodecMessage<AvatarStreamEntryMessage>, ICodecProvider<AvatarStreamEntryMessage>
{
    /// <inheritdoc />
    public static ICodec<AvatarStreamEntryMessage> Codec { get; } = Codecs.Composite(AvatarStreamEntry.Codec, static message => message.Entry, static entry => new AvatarStreamEntryMessage(entry));
}
