using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Resolves a timed task only when the reply still describes the active task.</summary>
public sealed record ExpireDerbyTaskServerCommand(
    LongId NeighborhoodId,
    LongId PlayerId,
    int TaskBoardIndex,
    int Status,
    bool Discarded,
    int Points,
    int DurationMinutes,
    int TaskGlobalId
) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ExpireDerbyTaskServerCommandType;

    /// <summary>Decodes the task result before its server-command metadata.</summary>
    public static ExpireDerbyTaskServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadLongId(),
            stream.ReadLongId(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadVarInt()
        );
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(NeighborhoodId);
        stream.WriteLongId(PlayerId);
        stream.WriteVarInt(TaskBoardIndex);
        stream.WriteVarInt(Status);
        stream.WriteBoolean(Discarded);
        stream.WriteVarInt(Points);
        stream.WriteVarInt(DurationMinutes);
        stream.WriteVarInt(TaskGlobalId);
    }
}
