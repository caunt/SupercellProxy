using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Ends the current derby and supplies placement and league results.</summary>
public sealed record EndDerbyServerCommand(int Rank, int LeagueIndex, int ParticipantCount, int NextLeagueIndex, LongId? InstanceId, DerbyResultEntries? Results) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.EndDerbyServerCommandType;

    /// <summary>Decodes the result before its server-command metadata.</summary>
    public static EndDerbyServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadOptionalLongId(),
            stream.ReadBoolean() ? DerbyResultEntries.Decode(stream) : null
        );
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(Rank);
        stream.WriteVarInt(LeagueIndex);
        stream.WriteVarInt(ParticipantCount);
        stream.WriteVarInt(NextLeagueIndex);
        stream.WriteOptionalLongId(InstanceId);
        stream.WriteBoolean(Results is not null);
        Results?.Encode(stream);
    }
}
