using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Visitors.Friendship;

/// <summary>Collects the friendship points of a farm visitor.</summary>
/// <param name="VisitorGlobalId">The visitor data id.</param>
public sealed record CollectVisitorFriendshipPointsCommand(int VisitorGlobalId) : CodecCommand<CollectVisitorFriendshipPointsCommand>, ICommandCodecProvider<CollectVisitorFriendshipPointsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectVisitorFriendshipPointsCommand> Codec { get; } = Codecs.Command(
        type: 559,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.VisitorGlobalId,
            static (visitorGlobalId) => new CollectVisitorFriendshipPointsCommand(visitorGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
