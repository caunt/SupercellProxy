using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Assigns a derby instance and its two native league selectors.</summary>
public sealed record AssignDerbyServerCommand(int LeagueType, int SeasonId, LongId? InstanceId, int LeagueIndex, int ParticipantCount) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.AssignDerbyServerCommandType;

    /// <summary>Decodes the assignment before its server-command metadata.</summary>
    public static AssignDerbyServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadOptionalLongId(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(LeagueType);
        stream.WriteVarInt(SeasonId);
        stream.WriteOptionalLongId(InstanceId);
        stream.WriteVarInt(LeagueIndex);
        stream.WriteVarInt(ParticipantCount);
    }
}
