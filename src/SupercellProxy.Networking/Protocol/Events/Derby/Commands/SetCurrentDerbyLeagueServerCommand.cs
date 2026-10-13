using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Replaces the current derby league index without changing its assignment or progress.</summary>
public sealed record SetCurrentDerbyLeagueServerCommand(int LeagueIndex) : CodecServerCommand<SetCurrentDerbyLeagueServerCommand>, ICommandCodecProvider<SetCurrentDerbyLeagueServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetCurrentDerbyLeagueServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.LeagueIndex,
            static (leagueIndex) => new SetCurrentDerbyLeagueServerCommand(leagueIndex)
        )
    );
}
