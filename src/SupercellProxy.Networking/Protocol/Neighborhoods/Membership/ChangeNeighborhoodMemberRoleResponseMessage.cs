using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Answers a member role change; the client refreshes the open neighborhood screen.</summary>
/// <param name="Role">The member's role after the change; the role numbers are not identified.</param>
/// <param name="AvatarId">Account of the member whose role changed.</param>
public sealed record ChangeNeighborhoodMemberRoleResponseMessage(int Role, LongId AvatarId) : CodecMessage<ChangeNeighborhoodMemberRoleResponseMessage>, ICodecProvider<ChangeNeighborhoodMemberRoleResponseMessage>
{
    /// <inheritdoc />
    public static ICodec<ChangeNeighborhoodMemberRoleResponseMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Role,
        Codecs.LongId,
        static message => message.AvatarId,
        static (role, avatarId) => new ChangeNeighborhoodMemberRoleResponseMessage(role, avatarId)
    );
}
