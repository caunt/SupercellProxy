using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Assigns a derby instance and its two native league selectors.</summary>
/// <param name="LeagueType">The row index in the derby leagues table of the assigned derby's league type, the first field.</param>
/// <param name="SeasonId">The derby season the instance belongs to.</param>
/// <param name="InstanceId">The id of the derby instance the player joins, or <see langword="null"/> when the server assigns none.</param>
/// <param name="LeagueIndex">The zero-based league the player competes in for this derby, an index into the derby leagues table that names the league.</param>
/// <param name="ParticipantCount">The participant count the server reports for the assigned derby; whether it counts neighbourhoods or players is not confirmed.</param>
public sealed record AssignDerbyServerCommand(int LeagueType, int SeasonId, LongId? InstanceId, int LeagueIndex, int ParticipantCount) : CodecServerCommand<AssignDerbyServerCommand>, ICommandCodecProvider<AssignDerbyServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AssignDerbyServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.LeagueType,
        Codecs.VarInt,
        static command => command.SeasonId,
        Codecs.OptionalLongId,
        static command => command.InstanceId,
        Codecs.VarInt,
        static command => command.LeagueIndex,
        Codecs.VarInt,
        static command => command.ParticipantCount,
        static (leagueType, seasonId, instanceId, leagueIndex, participantCount) => new AssignDerbyServerCommand(leagueType, seasonId, instanceId, leagueIndex, participantCount)));
}
