using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Birthday;

/// <summary>Claims the final leaderboard reward of the birthday event.</summary>
/// <param name="EventId">The event id.</param>
public sealed record ClaimBirthdayEventLeaderboardRewardCommand(int EventId) : CodecCommand<ClaimBirthdayEventLeaderboardRewardCommand>, ICommandCodecProvider<ClaimBirthdayEventLeaderboardRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimBirthdayEventLeaderboardRewardCommand> Codec { get; } = Codecs.Command(
        type: 370,
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static (eventId) => new ClaimBirthdayEventLeaderboardRewardCommand(eventId))
    );
}
