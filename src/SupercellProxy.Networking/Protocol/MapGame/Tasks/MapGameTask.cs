using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Native map-game task structure encoded by the shared 1.72.84 helper at 0x100668c08.
/// Includes active and cooldown timers and the per-participant task states.
/// </summary>
public sealed record MapGameTask : ICodecProvider<MapGameTask>
{
    /// <summary>
    /// Initializes a new <see cref="MapGameTask"/> instance.
    /// </summary>
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

    /// <summary>
    /// Gets the <c language="csharp">CompleteCooldownEnabled</c> value.
    /// </summary>
    public bool CompleteCooldownEnabled { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">CooldownTimer</c> value.
    /// </summary>
    public CommandVarIntPair CooldownTimer { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">ExpirationTime</c> value.
    /// </summary>
    public int ExpirationTime { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">ExpireCooldownEnabled</c> value.
    /// </summary>
    public bool ExpireCooldownEnabled { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">ExpireEnabled</c> value.
    /// </summary>
    public bool ExpireEnabled { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">InitialDurationSeconds</c> value.
    /// </summary>
    public int InitialDurationSeconds { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">NodeId</c> value.
    /// </summary>
    public int NodeId { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">PiggyBankRewards</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandVarIntPair> PiggyBankRewards { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">RespawnTime</c> value.
    /// </summary>
    public int RespawnTime { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">Rewards</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandVarIntPair> Rewards { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">State</c> value.
    /// </summary>
    public int State { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">States</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTaskState> States { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">TaskGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("TaskGlobalId")]
    public int TaskGlobalId { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">Timer</c> value.
    /// </summary>
    public CommandVarIntPair Timer { get; init; }
}
