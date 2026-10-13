using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Search;

/// <summary>Asks which of the given external friends can be added.</summary>
/// <param name="FacebookIds">The facebook ids.</param>
/// <param name="GamecenterIds">The gamecenter ids.</param>
public sealed record AskForAddableFriendsMessage(ReadOnlyMemory<string?>? FacebookIds, ReadOnlyMemory<string?>? GamecenterIds) : CodecMessage<AskForAddableFriendsMessage>, ICodecProvider<AskForAddableFriendsMessage>
{
    /// <inheritdoc />
    public static ICodec<AskForAddableFriendsMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(Codecs.OptionalString),
        static message => message.FacebookIds,
        Codecs.NullableMemory(Codecs.OptionalString),
        static message => message.GamecenterIds,
        static (facebookIds, gamecenterIds) => new AskForAddableFriendsMessage(facebookIds, gamecenterIds)
    );
}
