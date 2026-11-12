using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// One task on a Valley board node, with its active and cooldown timers, its rewards and each player's progress on it.
/// </summary>
public sealed record MapGameTask : ICodecProvider<MapGameTask>
{
    /// <summary>Initializes a new instance of the <see cref="MapGameTask"/> record. Creates a task from its decoded fields.</summary>
    /// <param name="taskGlobalId">Global id of the task's table entry.</param>
    /// <param name="nodeId">Board node the task sits on.</param>
    /// <param name="state">Where the task is: 0 waiting, 1 active, 2 expired, 3 completed, 4 finished.</param>
    /// <param name="initialDurationSeconds">Length of the task's active time in seconds.</param>
    /// <param name="expirationTime">When the task expires.</param>
    /// <param name="respawnTime">Unix time in seconds when the task comes back after its cooldown.</param>
    /// <param name="timer">Countdown of the active task, as start seconds and updates left.</param>
    /// <param name="cooldownTimer">Countdown before the task comes back, as start seconds and updates left.</param>
    /// <param name="expireEnabled">Whether the task expires when its countdown ends.</param>
    /// <param name="expireCooldownEnabled">Whether an expired task comes back after a cooldown.</param>
    /// <param name="completeCooldownEnabled">Whether a completed task comes back after a cooldown.</param>
    /// <param name="rewards">Valley coins the task gives, each as a coin table entry and an amount.</param>
    /// <param name="piggyBankRewards">Valley coins the task adds to the piggy bank, each as a coin table entry and an amount.</param>
    /// <param name="states">Each player's progress on the task.</param>
    public MapGameTask(int taskGlobalId,
        int nodeId,
        int state,
        int initialDurationSeconds,
        int expirationTime,
        int respawnTime,
        CommandVarIntPair timer,
        CommandVarIntPair cooldownTimer,
        bool expireEnabled,
        bool expireCooldownEnabled,
        bool completeCooldownEnabled,
        ReadOnlyMemory<CommandVarIntPair> rewards,
        ReadOnlyMemory<CommandVarIntPair> piggyBankRewards,
        ReadOnlyMemory<MapGameTaskState> states)
    {
        TaskGlobalId = taskGlobalId;
        NodeId = nodeId;
        State = state;
        InitialDurationSeconds = initialDurationSeconds;
        ExpirationTime = expirationTime;
        RespawnTime = respawnTime;
        Timer = timer;
        CooldownTimer = cooldownTimer;
        ExpireEnabled = expireEnabled;
        ExpireCooldownEnabled = expireCooldownEnabled;
        CompleteCooldownEnabled = completeCooldownEnabled;
        Rewards = rewards.ToArray();
        PiggyBankRewards = piggyBankRewards.ToArray();
        States = states.ToArray();
    }

    /// <inheritdoc />
    public static ICodec<MapGameTask> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static task => task.TaskGlobalId,
        Codecs.VarInt,
        static task => task.NodeId,
        Codecs.VarInt,
        static task => task.State,
        Codecs.VarInt,
        static task => task.InitialDurationSeconds,
        Codecs.VarInt,
        static task => task.ExpirationTime,
        Codecs.VarInt,
        static task => task.RespawnTime,
        CommandVarIntPair.Codec,
        static task => task.Timer,
        CommandVarIntPair.Codec,
        static task => task.CooldownTimer,
        Codecs.Boolean,
        static task => task.ExpireEnabled,
        Codecs.Boolean,
        static task => task.ExpireCooldownEnabled,
        Codecs.Boolean,
        static task => task.CompleteCooldownEnabled,
        Codecs.Memory(CommandVarIntPair.Codec, CommandVarIntPair.ArrayCount),
        static task => task.Rewards,
        Codecs.Memory(CommandVarIntPair.Codec, CommandVarIntPair.ArrayCount),
        static task => task.PiggyBankRewards,
        Codecs.Memory(MapGameTaskState.Codec),
        static task => task.States,
        static (taskGlobalId,
            nodeId,
            state,
            initialDurationSeconds,
            expirationTime,
            respawnTime,
            timer,
            cooldownTimer,
            expireEnabled,
            expireCooldownEnabled,
            completeCooldownEnabled,
            rewards,
            piggyBankRewards,
            states) => new MapGameTask(taskGlobalId,
                nodeId,
                state,
                initialDurationSeconds,
                expirationTime,
                respawnTime,
                timer,
                cooldownTimer,
                expireEnabled,
                expireCooldownEnabled,
                completeCooldownEnabled,
                rewards,
                piggyBankRewards,
                states));

    /// <summary>Gets or sets whether a completed task comes back after a cooldown.</summary>
    /// <value><see langword="true"/> if a cooldown follows the task's completion; otherwise, <see langword="false"/>.</value>
    public bool CompleteCooldownEnabled { get; init; }

    /// <summary>Gets or sets the countdown before the task comes back, as start seconds and updates left.</summary>
    /// <value>The cooldown length in seconds paired with the timer updates still left.</value>
    public CommandVarIntPair CooldownTimer { get; init; }

    /// <summary>Gets or sets when the task expires.</summary>
    /// <value>The expiry value the server sent, kept for re-encoding.</value>
    public int ExpirationTime { get; init; }

    /// <summary>Gets or sets whether an expired task comes back after a cooldown.</summary>
    /// <value><see langword="true"/> if a cooldown follows the task's expiry; otherwise, <see langword="false"/>.</value>
    public bool ExpireCooldownEnabled { get; init; }

    /// <summary>Gets or sets whether the task expires when its countdown ends.</summary>
    /// <value><see langword="true"/> if <see cref="Timer"/> is the task's time limit; otherwise, <see langword="false"/>.</value>
    public bool ExpireEnabled { get; init; }

    /// <summary>Gets or sets the length of the task's active time in seconds.</summary>
    /// <value>The full duration the task was given when it spawned, in seconds; 0 for a task without a time limit.</value>
    public int InitialDurationSeconds { get; init; }

    /// <summary>Gets or sets the board node the task sits on.</summary>
    /// <value>The id of a node of the season's Valley map, sent right after <see cref="TaskGlobalId"/>.</value>
    public int NodeId { get; init; }

    /// <summary>Gets or sets the Valley coins the task adds to the piggy bank, each as a coin table entry and an amount.</summary>
    /// <value>Pairs of a Valley coin row's global id and an amount; empty when the task feeds none.</value>
    public ReadOnlyMemory<CommandVarIntPair> PiggyBankRewards { get; init; }

    /// <summary>Gets or sets the Unix time in seconds when the task comes back after its cooldown.</summary>
    /// <value>Seconds since 1970-01-01 UTC.</value>
    public int RespawnTime { get; init; }

    /// <summary>Gets or sets the Valley coins the task gives, each as a coin table entry and an amount.</summary>
    /// <value>Pairs of a Valley coin row's global id and an amount, sent before <see cref="PiggyBankRewards"/>; empty when the task gives no coins.</value>
    public ReadOnlyMemory<CommandVarIntPair> Rewards { get; init; }

    /// <summary>Gets or sets where the task is: 0 waiting, 1 active, 2 expired, 3 completed, 4 finished.</summary>
    /// <value>The task's state code.</value>
    public int State { get; init; }

    /// <summary>Gets or sets each player's progress on the task.</summary>
    /// <value>One entry per participant, each with its own completion flag and task-type payload.</value>
    public ReadOnlyMemory<MapGameTaskState> States { get; init; }

    /// <summary>Gets or sets the global id of the task's table entry.</summary>
    /// <value>The global id of the Valley task row, whose <c>TaskType</c> decides the kind of payload in <see cref="States"/>; the first field on the wire.</value>
    [JsonPropertyName("TaskGlobalId")]
    public int TaskGlobalId { get; init; }

    /// <summary>Gets or sets the countdown of the active task, as start seconds and updates left.</summary>
    /// <value>The active duration in seconds paired with the timer updates still left.</value>
    public CommandVarIntPair Timer { get; init; }
}
