using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Native map-game task structure encoded by the shared 1.72.84 helper at 0x100668c08.
/// Includes active and cooldown timers and the per-participant task states.
/// </summary>
public sealed record MapGameTask
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

    /// <summary>
    /// Gets the <c language="csharp">CompleteCooldownEnabled</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownBoolean2")]
    public bool CompleteCooldownEnabled { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">CooldownTimer</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownPair1")]
    public CommandVarIntPair CooldownTimer { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">ExpirationTime</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown3")]
    public int ExpirationTime { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">ExpireCooldownEnabled</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownBoolean1")]
    public bool ExpireCooldownEnabled { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">ExpireEnabled</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownBoolean0")]
    public bool ExpireEnabled { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">InitialDurationSeconds</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown2")]
    public int InitialDurationSeconds { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">NodeId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown0")]
    public int NodeId { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">PiggyBankRewards</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownPairs1")]
    public ReadOnlyMemory<CommandVarIntPair> PiggyBankRewards { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">RespawnTime</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown4")]
    public int RespawnTime { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">Rewards</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownPairs0")]
    public ReadOnlyMemory<CommandVarIntPair> Rewards { get; init; }

    /// <summary>
    /// Gets the <c language="csharp">State</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("Unknown1")]
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
    [System.Text.Json.Serialization.JsonPropertyName("UnknownPair0")]
    public CommandVarIntPair Timer { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameTask Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int taskGlobalId = stream.ReadVarInt();
        int nodeId = stream.ReadVarInt();
        int state = stream.ReadVarInt();
        int initialDurationSeconds = stream.ReadVarInt();
        int expirationTime = stream.ReadVarInt();
        int respawnTime = stream.ReadVarInt();
        CommandVarIntPair timer = new(stream.ReadVarInt(), stream.ReadVarInt());
        CommandVarIntPair cooldownTimer = new(stream.ReadVarInt(), stream.ReadVarInt());
        bool expireEnabled = stream.ReadBoolean();
        bool expireCooldownEnabled = stream.ReadBoolean();
        bool completeCooldownEnabled = stream.ReadBoolean();
        ReadOnlyMemory<CommandVarIntPair> rewards = CommandVarIntPairArrayField.Decode(stream).Values;
        ReadOnlyMemory<CommandVarIntPair> piggyBankRewards = CommandVarIntPairArrayField.Decode(stream).Values;
        int stateCount = MapGameFieldCodec.ReadCount(stream, name: "task-state");
        MapGameTaskState[] states = new MapGameTaskState[stateCount];

        for (int index = 0; index < states.Length; index++)
            states[index] = MapGameTaskState.Decode(stream, dataResolver);

        return new MapGameTask(
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
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVarInt(TaskGlobalId);
        stream.WriteVarInt(NodeId);
        stream.WriteVarInt(State);
        stream.WriteVarInt(InitialDurationSeconds);
        stream.WriteVarInt(ExpirationTime);
        stream.WriteVarInt(RespawnTime);
        stream.WriteVarInt(Timer.Value0);
        stream.WriteVarInt(Timer.Value1);
        stream.WriteVarInt(CooldownTimer.Value0);
        stream.WriteVarInt(CooldownTimer.Value1);
        stream.WriteBoolean(ExpireEnabled);
        stream.WriteBoolean(ExpireCooldownEnabled);
        stream.WriteBoolean(CompleteCooldownEnabled);
        new CommandVarIntPairArrayField(Rewards).Encode(stream);
        new CommandVarIntPairArrayField(PiggyBankRewards).Encode(stream);
        stream.WriteVarInt(States.Length);

        foreach (MapGameTaskState state in States.Span)
            state.Encode(stream);
    }
}
