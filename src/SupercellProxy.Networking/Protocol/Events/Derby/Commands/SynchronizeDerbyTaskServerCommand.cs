using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands;

/// <summary>Synchronizes task metadata without reserving another task allowance.</summary>
public sealed record SynchronizeDerbyTaskServerCommand : CodecServerCommand<SynchronizeDerbyTaskServerCommand>, ICommandCodecProvider<SynchronizeDerbyTaskServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SynchronizeDerbyTaskServerCommand> Codec { get; } = Codecs.Command(
        type: 234,
        Codecs.Composite(
            Codecs.LongId,
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
            static (
                neighborhoodId,
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
                reactivateStatus
            ) => new SynchronizeDerbyTaskServerCommand
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
            }
        )
    );

    /// <summary>Gets whether the task belongs to bingo derby.</summary>
    public bool BingoTask { get; init; }
    /// <summary>Gets whether the task belongs to blossom derby.</summary>
    public bool BlossomTask { get; init; }
    /// <summary>Gets whether the task belongs to bunny derby.</summary>
    public bool BunnyDerbyTask { get; init; }
    /// <summary>Gets whether the task belongs to chill derby.</summary>
    public bool ChillDerbyTask { get; init; }
    /// <summary>Gets the task timer's duration in minutes.</summary>
    public int DurationMinutes { get; init; }
    /// <summary>Gets whether the task belongs to mystery derby.</summary>
    public bool MysteryTask { get; init; }
    /// <summary>Gets the neighborhood identifier.</summary>
    public LongId NeighborhoodId { get; init; }
    /// <summary>Gets the player identifier.</summary>
    public LongId PlayerId { get; init; }
    /// <summary>Gets the task's derby points.</summary>
    public int Points { get; init; }
    /// <summary>Gets the task's reactivation state.</summary>
    public int ReactivateStatus { get; init; }
    /// <summary>Gets the required quantities in task-definition units.</summary>
    public int[] RequiredQuantities { get; init; } = [];
    /// <summary>Gets the accepted board index, or a negative value to clear the task.</summary>
    public int TaskBoardIndex { get; init; }
    /// <summary>Gets the task definition identifier.</summary>
    public int TaskGlobalId { get; init; }
}
