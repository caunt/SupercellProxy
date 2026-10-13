using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports the result of taking or reactivating a task and supplies its authoritative goals.</summary>
public sealed record StartDerbyTaskServerCommand : CodecServerCommand<StartDerbyTaskServerCommand>, ICommandCodecProvider<StartDerbyTaskServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartDerbyTaskServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.NeighborhoodId,
            Codecs.LongId,
            static command => command.PlayerId,
            Codecs.VarInt,
            static command => command.SelectedTaskBoardIndex,
            Codecs.VarInt,
            static command => command.TaskBoardIndex,
            Codecs.VarInt,
            static command => command.TaskGlobalId,
            Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
            static command => command.RequiredQuantities,
            Codecs.VarInt,
            static command => command.DurationMinutes,
            Codecs.VarInt,
            static command => command.Points,
            Codecs.VarInt,
            static command => command.Status,
            TaskKinds.Codec,
            static command => new TaskKinds(command.BingoTask, command.MysteryTask, command.BlossomTask, command.ChillDerbyTask, command.BunnyDerbyTask),
            Codecs.VarInt,
            static command => command.HotPotatoTaskDataGlobalId,
            Codecs.VarInt,
            static command => command.HotPotatoTaskIndex,
            Codecs.VarInt,
            static command => command.MaximumTasks,
            Codecs.VarInt,
            static command => command.ReactivateStatus,
            Codecs.Optional(DerbyTaskEntry.Codec),
            static command => command.Entry,
            Codecs.Array(Codecs.VarInt, CollectionCount.Bounded),
            static command => command.Progress,
            static (
                neighborhoodId,
                playerId,
                selectedTaskBoardIndex,
                taskBoardIndex,
                taskGlobalId,
                requiredQuantities,
                durationMinutes,
                points,
                status,
                taskKinds,
                hotPotatoTaskDataGlobalId,
                hotPotatoTaskIndex,
                maximumTasks,
                reactivateStatus,
                entry,
                progress
            ) => new StartDerbyTaskServerCommand
            {
                NeighborhoodId = neighborhoodId,
                PlayerId = playerId,
                SelectedTaskBoardIndex = selectedTaskBoardIndex,
                TaskBoardIndex = taskBoardIndex,
                TaskGlobalId = taskGlobalId,
                RequiredQuantities = requiredQuantities,
                DurationMinutes = durationMinutes,
                Points = points,
                Status = status,
                BingoTask = taskKinds.BingoTask,
                MysteryTask = taskKinds.MysteryTask,
                BlossomTask = taskKinds.BlossomTask,
                ChillDerbyTask = taskKinds.ChillDerbyTask,
                BunnyDerbyTask = taskKinds.BunnyDerbyTask,
                HotPotatoTaskDataGlobalId = hotPotatoTaskDataGlobalId,
                HotPotatoTaskIndex = hotPotatoTaskIndex,
                MaximumTasks = maximumTasks,
                ReactivateStatus = reactivateStatus,
                Entry = entry,
                Progress = progress,
            }
        )
    );

    /// <summary>Gets whether this is a bingo task.</summary>
    public bool BingoTask { get; init; }
    /// <summary>Gets whether this is a blossom task.</summary>
    public bool BlossomTask { get; init; }
    /// <summary>Gets whether this is a bunny derby task.</summary>
    public bool BunnyDerbyTask { get; init; }
    /// <summary>Gets whether this is a chill derby task.</summary>
    public bool ChillDerbyTask { get; init; }
    /// <summary>Gets the task's duration in minutes.</summary>
    public int DurationMinutes { get; init; }
    /// <summary>Gets the accompanying board-entry update, when supplied.</summary>
    public DerbyTaskEntry? Entry { get; init; }
    /// <summary>Gets the linked hot-potato definition identifier.</summary>
    public int HotPotatoTaskDataGlobalId { get; init; }
    /// <summary>Gets the linked hot-potato task index.</summary>
    public int HotPotatoTaskIndex { get; init; }
    /// <summary>Gets the player's task allowance.</summary>
    public int MaximumTasks { get; init; }
    /// <summary>Gets whether this is a mystery task.</summary>
    public bool MysteryTask { get; init; }
    /// <summary>Gets the neighborhood to which the reply belongs.</summary>
    public LongId NeighborhoodId { get; init; }
    /// <summary>Gets the player taking the task.</summary>
    public LongId PlayerId { get; init; }
    /// <summary>Gets the task's derby points.</summary>
    public int Points { get; init; }
    /// <summary>Gets the required quantities for each goal.</summary>
    public int[] RequiredQuantities { get; init; } = [];
    /// <summary>Gets the reactivation state supplied with the task.</summary>
    public int ReactivateStatus { get; init; }
    /// <summary>Gets restored progress for the accepted task.</summary>
    public int[] Progress { get; init; } = [];
    /// <summary>Gets the requested board slot.</summary>
    public int SelectedTaskBoardIndex { get; init; }
    /// <summary>Gets the native request result; one accepts the task.</summary>
    public int Status { get; init; }
    /// <summary>Gets the accepted task's board slot.</summary>
    public int TaskBoardIndex { get; init; }
    /// <summary>Gets the task definition's global data identifier.</summary>
    public int TaskGlobalId { get; init; }

    // The task kind flags native writes together.
    private readonly record struct TaskKinds(bool BingoTask, bool MysteryTask, bool BlossomTask, bool ChillDerbyTask, bool BunnyDerbyTask)
    {
        public static ICodec<TaskKinds> Codec { get; } = Codecs.Composite(
            Codecs.Boolean,
            static segment => segment.BingoTask,
            Codecs.Boolean,
            static segment => segment.MysteryTask,
            Codecs.Boolean,
            static segment => segment.BlossomTask,
            Codecs.Boolean,
            static segment => segment.ChillDerbyTask,
            Codecs.Boolean,
            static segment => segment.BunnyDerbyTask,
            static (bingoTask, mysteryTask, blossomTask, chillDerbyTask, bunnyDerbyTask) => new TaskKinds(bingoTask, mysteryTask, blossomTask, chillDerbyTask, bunnyDerbyTask)
        );
    }
}
