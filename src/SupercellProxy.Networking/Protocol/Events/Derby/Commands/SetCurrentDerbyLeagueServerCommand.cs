using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Replaces the current derby league index without changing its assignment or progress.</summary>
public sealed record SetCurrentDerbyLeagueServerCommand(int LeagueIndex) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.SetCurrentDerbyLeagueServerCommandType;

    /// <summary>Decodes the league index before server-command metadata.</summary>
    public static SetCurrentDerbyLeagueServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(LeagueIndex);
    }
}
