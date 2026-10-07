using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports the result of completing or discarding a player's derby task.</summary>
public sealed record ResolveDerbyTaskServerCommand(LongId NeighborhoodId, LongId PlayerId, int TaskBoardIndex, int Status, bool Discarded, int ResultValue, bool SeenExpiredTask) : ServerCommand
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ResolveDerbyTaskServerCommandType;

    /// <summary>Decodes the task result before its server-command metadata.</summary>
    public static ResolveDerbyTaskServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadLongId(),
            stream.ReadLongId(),
            stream.ReadVarInt(),
            stream.ReadVarInt(),
            stream.ReadBoolean(),
            stream.ReadVarInt(),
            stream.ReadBoolean()
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
        stream.WriteVarInt(ResultValue);
        stream.WriteBoolean(SeenExpiredTask);
    }
}
