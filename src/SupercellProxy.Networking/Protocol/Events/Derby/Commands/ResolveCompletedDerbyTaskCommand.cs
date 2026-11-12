using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Records leaderboard credit or submits a completed task identified by its board slot and definition.</summary>
/// <param name="TaskBoardIndex">The task-board slot of the player's active task, the first field.</param>
/// <param name="TaskGlobalId">The global id of the active task's definition.</param>
/// <param name="Action"><see cref="DerbyTaskCompletionAction.RecordLeaderboardPoints"/> to record the task's leaderboard points, or <see cref="DerbyTaskCompletionAction.Submit"/> to submit the finished task.</param>
public sealed record ResolveCompletedDerbyTaskCommand(int TaskBoardIndex, int TaskGlobalId, DerbyTaskCompletionAction Action) : CodecCommand<ResolveCompletedDerbyTaskCommand>, ICommandCodecProvider<ResolveCompletedDerbyTaskCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ResolveCompletedDerbyTaskCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.TaskBoardIndex,
        Codecs.VarInt,
        static command => command.TaskGlobalId,
        Codecs.Enum<DerbyTaskCompletionAction>(Codecs.VarInt),
        static command => command.Action,
        static (taskBoardIndex, taskGlobalId, action) => new ResolveCompletedDerbyTaskCommand(taskBoardIndex, taskGlobalId, action)));
}
