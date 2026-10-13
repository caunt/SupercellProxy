using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors.Friendship;

/// <summary>Claims a visitor friendship-track reward.</summary>
/// <param name="RewardGlobalId">The reward data id.</param>
public sealed record ClaimVisitorFriendshipRewardCommand(int RewardGlobalId) : CodecCommand<ClaimVisitorFriendshipRewardCommand>, ICommandCodecProvider<ClaimVisitorFriendshipRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimVisitorFriendshipRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.RewardGlobalId,
            static (rewardGlobalId) => new ClaimVisitorFriendshipRewardCommand(rewardGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
