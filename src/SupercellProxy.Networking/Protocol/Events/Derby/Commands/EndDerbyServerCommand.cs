using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Ends the current derby and supplies placement and league results.</summary>
/// <param name="Rank">The neighbourhood's final placement in the ended derby, the first field.</param>
/// <param name="LeagueIndex">The league the ended derby was played in.</param>
/// <param name="ParticipantCount">The participant count of the ended derby.</param>
/// <param name="NextLeagueIndex">The league the player is placed in for the next derby.</param>
/// <param name="InstanceId">The id of the derby that ended, or <see langword="null"/> when the server sends none.</param>
/// <param name="Results">The final standings sent with the result, or <see langword="null"/> when the server sends none.</param>
public sealed record EndDerbyServerCommand(int Rank, int LeagueIndex, int ParticipantCount, int NextLeagueIndex, LongId? InstanceId, DerbyResultEntries? Results) : CodecServerCommand<EndDerbyServerCommand>, ICommandCodecProvider<EndDerbyServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<EndDerbyServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.Rank,
        Codecs.VarInt,
        static command => command.LeagueIndex,
        Codecs.VarInt,
        static command => command.ParticipantCount,
        Codecs.VarInt,
        static command => command.NextLeagueIndex,
        Codecs.OptionalLongId,
        static command => command.InstanceId,
        Codecs.Optional(DerbyResultEntries.Codec),
        static command => command.Results,
        static (rank, leagueIndex, participantCount, nextLeagueIndex, instanceId, results) => new EndDerbyServerCommand(rank, leagueIndex, participantCount, nextLeagueIndex, instanceId, results)));
}
