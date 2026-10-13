using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Answers a member role change; the client refreshes the open neighborhood screen.</summary>
/// <param name="Role">The role.</param>
/// <param name="AvatarId">The avatar id.</param>
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
