using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Reports the result of taking or reactivating a task and supplies its authoritative goals.</summary>
public sealed record StartDerbyTaskServerCommand : CodecServerCommand<StartDerbyTaskServerCommand>, ICommandCodecProvider<StartDerbyTaskServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartDerbyTaskServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.LongId,
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
        static (neighborhoodId,
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
            progress) => new StartDerbyTaskServerCommand
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
            }));

    /// <summary>Gets or sets whether this is a bingo task.</summary>
    /// <value><see langword="true"/> if the task is one of a bingo derby's board tasks; otherwise, <see langword="false"/>.</value>
    public bool BingoTask { get; init; }

    /// <summary>Gets or sets whether this is a blossom task.</summary>
    /// <value><see langword="true"/> if the task belongs to a blossom derby; otherwise, <see langword="false"/>.</value>
    public bool BlossomTask { get; init; }

    /// <summary>Gets or sets whether this is a bunny derby task.</summary>
    /// <value><see langword="true"/> if the task belongs to a bunny derby; otherwise, <see langword="false"/>.</value>
    public bool BunnyDerbyTask { get; init; }

    /// <summary>Gets or sets whether this is a chill derby task.</summary>
    /// <value><see langword="true"/> if the task belongs to a chill derby; otherwise, <see langword="false"/>.</value>
    public bool ChillDerbyTask { get; init; }

    /// <summary>Gets or sets the task's duration in minutes.</summary>
    /// <value>The task timer's length in minutes; 0 or less for a task without a time limit.</value>
    public int DurationMinutes { get; init; }

    /// <summary>Gets or sets the accompanying board-entry update, when supplied.</summary>
    /// <value>The board entry for the task as the neighbourhood sees it, or <see langword="null"/> when the reply carries none.</value>
    public DerbyTaskEntry? Entry { get; init; }

    /// <summary>Gets or sets the linked hot-potato definition identifier.</summary>
    /// <value>The data-table global id of the hot-potato task linked to this one; the value used when there is no link is not yet identified.</value>
    public int HotPotatoTaskDataGlobalId { get; init; }

    /// <summary>Gets or sets the linked hot-potato task index.</summary>
    /// <value>The index of the linked hot-potato task; the value used when there is no link is not yet identified.</value>
    public int HotPotatoTaskIndex { get; init; }

    /// <summary>Gets or sets the player's task allowance.</summary>
    /// <value>How many derby tasks the player may take in this derby.</value>
    public int MaximumTasks { get; init; }

    /// <summary>Gets or sets whether this is a mystery task.</summary>
    /// <value><see langword="true"/> if the task is a mystery task; otherwise, <see langword="false"/>.</value>
    public bool MysteryTask { get; init; }

    /// <summary>Gets or sets the neighborhood to which the reply belongs.</summary>
    /// <value>The id of the neighbourhood whose derby board holds the task.</value>
    public LongId NeighborhoodId { get; init; }

    /// <summary>Gets or sets the player taking the task.</summary>
    /// <value>The avatar id of the player taking the task.</value>
    public LongId PlayerId { get; init; }

    /// <summary>Gets or sets the task's derby points.</summary>
    /// <value>The points the task is worth to the neighbourhood when completed.</value>
    public int Points { get; init; }

    /// <summary>Gets or sets restored progress for the accepted task.</summary>
    /// <value>Progress per goal restored for the task; an empty array by default.</value>
    public ImmutableArray<int> Progress { get; init; } = [];

    /// <summary>Gets or sets the reactivation state supplied with the task.</summary>
    /// <value>A native reactivation status; 1 means a previously taken task is being reactivated.</value>
    public int ReactivateStatus { get; init; }

    /// <summary>Gets or sets the required quantities for each goal.</summary>
    /// <value>One target per goal of the task, in task-definition units.</value>
    public ImmutableArray<int> RequiredQuantities { get; init; } = [];

    /// <summary>Gets or sets the requested board slot.</summary>
    /// <value>The task-board slot the player selected when asking for the task.</value>
    public int SelectedTaskBoardIndex { get; init; }

    /// <summary>Gets or sets the native request result; one accepts the task.</summary>
    /// <value>The result code of the request; 1 when the server accepted the task.</value>
    public int Status { get; init; }

    /// <summary>Gets or sets the accepted task's board slot.</summary>
    /// <value>The task-board slot the accepted task occupies.</value>
    public int TaskBoardIndex { get; init; }

    /// <summary>Gets or sets the task definition's global data identifier.</summary>
    /// <value>The global id of the task's row in the neighbourhood tasks table.</value>
    public int TaskGlobalId { get; init; }

    // The task kind flags native writes together.
    private readonly record struct TaskKinds(bool BingoTask, bool MysteryTask, bool BlossomTask, bool ChillDerbyTask, bool BunnyDerbyTask)
    {
        public static ICodec<TaskKinds> Codec { get; } = Codecs.Composite(Codecs.Boolean,
            static segment => segment.BingoTask,
            Codecs.Boolean,
            static segment => segment.MysteryTask,
            Codecs.Boolean,
            static segment => segment.BlossomTask,
            Codecs.Boolean,
            static segment => segment.ChillDerbyTask,
            Codecs.Boolean,
            static segment => segment.BunnyDerbyTask,
            static (bingoTask, mysteryTask, blossomTask, chillDerbyTask, bunnyDerbyTask) => new TaskKinds(bingoTask, mysteryTask, blossomTask, chillDerbyTask, bunnyDerbyTask));
    }
}
