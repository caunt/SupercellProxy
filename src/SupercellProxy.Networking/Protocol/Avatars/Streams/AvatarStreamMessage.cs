using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Streams;

/// <summary>Delivers the player's inbox (avatar stream) entries to the game log manager.</summary>
/// <param name="Entries">The entries, or <see langword="null"/> when native writes a negative count.</param>
public sealed record AvatarStreamMessage(ReadOnlyMemory<AvatarStreamEntry>? Entries) : CodecMessage<AvatarStreamMessage>, ICodecProvider<AvatarStreamMessage>
{
    /// <inheritdoc />
    public static ICodec<AvatarStreamMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(AvatarStreamEntry.Codec),
        static message => message.Entries,
        static entries => new AvatarStreamMessage(entries)
    );
}
