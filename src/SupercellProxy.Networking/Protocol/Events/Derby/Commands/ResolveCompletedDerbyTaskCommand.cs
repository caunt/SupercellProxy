using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Records leaderboard credit or submits a completed task identified by its board slot and definition.</summary>
public sealed record ResolveCompletedDerbyTaskCommand(int TaskBoardIndex, int TaskGlobalId, DerbyTaskCompletionAction Action) : Command
{
    /// <inheritdoc />
    public override int Type => CommandRegistry.ResolveCompletedDerbyTaskCommandType;

    /// <summary>Decodes the task identity and completion stage before the command metadata.</summary>
    public static ResolveCompletedDerbyTaskCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadVarInt(), stream.ReadVarInt(), (DerbyTaskCompletionAction)stream.ReadVarInt());
    }

    /// <inheritdoc />
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteVarInt(TaskBoardIndex);
        stream.WriteVarInt(TaskGlobalId);
        stream.WriteVarInt((int)Action);
    }
}
