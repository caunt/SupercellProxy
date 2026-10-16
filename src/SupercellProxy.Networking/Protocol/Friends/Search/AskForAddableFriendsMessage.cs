using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Search;

/// <summary>Asks which of the given external friends can be added.</summary>
/// <param name="FacebookIds">Facebook user ids of the player's Facebook friends; null when none are sent.</param>
/// <param name="GamecenterIds">Game Center player ids of the player's Game Center friends; null when none are sent.</param>
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
