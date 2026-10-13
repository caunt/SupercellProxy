using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors.Friendship;

/// <summary>Gives a friendship-book gift to a farm visitor.</summary>
/// <param name="VisitorGlobalId">The visitor (people) data id.</param>
/// <param name="GiftGlobalId">The gift data id.</param>
public sealed record GiveVisitorFriendshipGiftCommand(int VisitorGlobalId, int GiftGlobalId) : CodecCommand<GiveVisitorFriendshipGiftCommand>, ICommandCodecProvider<GiveVisitorFriendshipGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<GiveVisitorFriendshipGiftCommand> Codec { get; } = Codecs.Command(
        type: 617,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.VisitorGlobalId,
            Codecs.VarInt,
            static command => command.GiftGlobalId,
            static (visitorGlobalId, giftGlobalId) => new GiveVisitorFriendshipGiftCommand(visitorGlobalId, giftGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
