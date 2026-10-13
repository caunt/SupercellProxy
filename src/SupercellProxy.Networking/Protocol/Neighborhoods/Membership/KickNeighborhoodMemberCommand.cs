using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Membership;

/// <summary>Removes a member from the neighborhood when the avatar's role has the kick right.</summary>
/// <param name="MemberHomeId">The member's home id.</param>
public sealed record KickNeighborhoodMemberCommand(LongId MemberHomeId) : CodecCommand<KickNeighborhoodMemberCommand>, ICommandCodecProvider<KickNeighborhoodMemberCommand>
{
    /// <inheritdoc />
    public static CommandCodec<KickNeighborhoodMemberCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.LongId, static command => command.MemberHomeId, static (memberHomeId) => new KickNeighborhoodMemberCommand(memberHomeId))
    );
}
