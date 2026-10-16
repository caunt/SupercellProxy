using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Changes the role of a neighborhood member.</summary>
/// <param name="Role">New role for the member; the role numbers are not identified.</param>
/// <param name="AvatarId">Account of the member whose role changes.</param>
public sealed record ChangeNeighborhoodMemberRoleMessage(int Role, LongId AvatarId) : CodecMessage<ChangeNeighborhoodMemberRoleMessage>, ICodecProvider<ChangeNeighborhoodMemberRoleMessage>
{
    /// <inheritdoc />
    public static ICodec<ChangeNeighborhoodMemberRoleMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Role,
        Codecs.LongId,
        static message => message.AvatarId,
        static (role, avatarId) => new ChangeNeighborhoodMemberRoleMessage(role, avatarId)
    );
}
