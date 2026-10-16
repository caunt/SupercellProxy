using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// One task on a Valley board node, with its active and cooldown timers, its rewards and each player's progress on it.
/// </summary>
public sealed record MapGameTask : ICodecProvider<MapGameTask>
{
    /// <summary>Creates a task from its decoded fields.</summary>
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
    public MapGameTask(
        int taskGlobalId,
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
        ReadOnlyMemory<MapGameTaskState> states
    )
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
    public static ICodec<MapGameTask> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
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
        static (
            taskGlobalId,
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
            states
        ) => new MapGameTask(
            taskGlobalId,
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
            states
        )
    );

    /// <summary>Whether a completed task comes back after a cooldown.</summary>
    public bool CompleteCooldownEnabled { get; init; }

    /// <summary>Countdown before the task comes back, as start seconds and updates left.</summary>
    public CommandVarIntPair CooldownTimer { get; init; }

    /// <summary>When the task expires.</summary>
    public int ExpirationTime { get; init; }

    /// <summary>Whether an expired task comes back after a cooldown.</summary>
    public bool ExpireCooldownEnabled { get; init; }

    /// <summary>Whether the task expires when its countdown ends.</summary>
    public bool ExpireEnabled { get; init; }

    /// <summary>Length of the task's active time in seconds.</summary>
    public int InitialDurationSeconds { get; init; }

    /// <summary>Board node the task sits on.</summary>
    public int NodeId { get; init; }

    /// <summary>Valley coins the task adds to the piggy bank, each as a coin table entry and an amount.</summary>
    public ReadOnlyMemory<CommandVarIntPair> PiggyBankRewards { get; init; }

    /// <summary>Unix time in seconds when the task comes back after its cooldown.</summary>
    public int RespawnTime { get; init; }

    /// <summary>Valley coins the task gives, each as a coin table entry and an amount.</summary>
    public ReadOnlyMemory<CommandVarIntPair> Rewards { get; init; }

    /// <summary>Where the task is: 0 waiting, 1 active, 2 expired, 3 completed, 4 finished.</summary>
    public int State { get; init; }

    /// <summary>Each player's progress on the task.</summary>
    public ReadOnlyMemory<MapGameTaskState> States { get; init; }

    /// <summary>Global id of the task's table entry.</summary>
    [JsonPropertyName("TaskGlobalId")]
    public int TaskGlobalId { get; init; }

    /// <summary>Countdown of the active task, as start seconds and updates left.</summary>
    public CommandVarIntPair Timer { get; init; }
}
