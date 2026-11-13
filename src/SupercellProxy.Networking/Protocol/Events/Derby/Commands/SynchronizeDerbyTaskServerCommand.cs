using System.Collections.Immutable;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Synchronizes task metadata without reserving another task allowance.</summary>
public sealed record SynchronizeDerbyTaskServerCommand : CodecServerCommand<SynchronizeDerbyTaskServerCommand>, ICommandCodecProvider<SynchronizeDerbyTaskServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SynchronizeDerbyTaskServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.LongId,
        static command => command.NeighborhoodId,
        Codecs.LongId,
        static command => command.PlayerId,
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
        Codecs.Boolean,
        static command => command.BingoTask,
        Codecs.Boolean,
        static command => command.MysteryTask,
        Codecs.Boolean,
        static command => command.BlossomTask,
        Codecs.Boolean,
        static command => command.ChillDerbyTask,
        Codecs.Boolean,
        static command => command.BunnyDerbyTask,
        Codecs.VarInt,
        static command => command.ReactivateStatus,
        static (neighborhoodId,
            playerId,
            taskBoardIndex,
            taskGlobalId,
            requiredQuantities,
            durationMinutes,
            points,
            bingoTask,
            mysteryTask,
            blossomTask,
            chillDerbyTask,
            bunnyDerbyTask,
            reactivateStatus) => new SynchronizeDerbyTaskServerCommand
            {
                NeighborhoodId = neighborhoodId,
                PlayerId = playerId,
                TaskBoardIndex = taskBoardIndex,
                TaskGlobalId = taskGlobalId,
                RequiredQuantities = requiredQuantities,
                DurationMinutes = durationMinutes,
                Points = points,
                BingoTask = bingoTask,
                MysteryTask = mysteryTask,
                BlossomTask = blossomTask,
                ChillDerbyTask = chillDerbyTask,
                BunnyDerbyTask = bunnyDerbyTask,
                ReactivateStatus = reactivateStatus,
            }));

    /// <summary>Gets or sets whether the task belongs to bingo derby.</summary>
    /// <value><see langword="true"/> if the synchronized task is a bingo derby task; otherwise, <see langword="false"/>.</value>
    public bool BingoTask { get; init; }

    /// <summary>Gets or sets whether the task belongs to blossom derby.</summary>
    /// <value><see langword="true"/> if the synchronized task is a blossom derby task; otherwise, <see langword="false"/>.</value>
    public bool BlossomTask { get; init; }

    /// <summary>Gets or sets whether the task belongs to bunny derby.</summary>
    /// <value><see langword="true"/> if the synchronized task is a bunny derby task; otherwise, <see langword="false"/>.</value>
    public bool BunnyDerbyTask { get; init; }

    /// <summary>Gets or sets whether the task belongs to chill derby.</summary>
    /// <value><see langword="true"/> if the synchronized task is a chill derby task; otherwise, <see langword="false"/>.</value>
    public bool ChillDerbyTask { get; init; }

    /// <summary>Gets or sets the task timer's duration in minutes.</summary>
    /// <value>The length of the synchronized task's timer, in minutes; 0 or less for a task without a time limit.</value>
    public int DurationMinutes { get; init; }

    /// <summary>Gets or sets whether the task belongs to mystery derby.</summary>
    /// <value><see langword="true"/> if the synchronized task is a mystery task; otherwise, <see langword="false"/>.</value>
    public bool MysteryTask { get; init; }

    /// <summary>Gets or sets the neighborhood identifier.</summary>
    /// <value>The id of the neighbourhood the synchronized task belongs to.</value>
    public LongId NeighborhoodId { get; init; }

    /// <summary>Gets or sets the player identifier.</summary>
    /// <value>The avatar id of the farm whose task is synchronized.</value>
    public LongId PlayerId { get; init; }

    /// <summary>Gets or sets the task's derby points.</summary>
    /// <value>The points the synchronized task is worth to the neighbourhood when completed.</value>
    public int Points { get; init; }

    /// <summary>Gets or sets the task's reactivation state.</summary>
    /// <value>The native reactivation status; 1 marks a reactivated task.</value>
    public int ReactivateStatus { get; init; }

    /// <summary>Gets or sets the required quantities in task-definition units.</summary>
    /// <value>One target per goal of the synchronized task.</value>
    public ImmutableArray<int> RequiredQuantities { get; init; } = [];

    /// <summary>Gets or sets the accepted board index, or a negative value for no task.</summary>
    /// <value>The board slot of the player's task; a negative value means the player holds no task.</value>
    public int TaskBoardIndex { get; init; }

    /// <summary>Gets or sets the task definition identifier.</summary>
    /// <value>The global id of the task's row in the neighbourhood tasks table.</value>
    public int TaskGlobalId { get; init; }
}
