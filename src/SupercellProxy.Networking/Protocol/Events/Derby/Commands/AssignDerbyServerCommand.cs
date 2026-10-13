using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Assigns a derby instance and its two native league selectors.</summary>
public sealed record AssignDerbyServerCommand(int LeagueType, int SeasonId, LongId? InstanceId, int LeagueIndex, int ParticipantCount) : CodecServerCommand<AssignDerbyServerCommand>, ICommandCodecProvider<AssignDerbyServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<AssignDerbyServerCommand> Codec { get; } = Codecs.Command(
        type: 167,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.LeagueType,
            Codecs.VarInt,
            static command => command.SeasonId,
            Codecs.OptionalLongId,
            static command => command.InstanceId,
            Codecs.VarInt,
            static command => command.LeagueIndex,
            Codecs.VarInt,
            static command => command.ParticipantCount,
            static (leagueType, seasonId, instanceId, leagueIndex, participantCount) => new AssignDerbyServerCommand(leagueType, seasonId, instanceId, leagueIndex, participantCount)
        )
    );
}
