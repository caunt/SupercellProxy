using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.CollectionFields;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame.Tasks;

/// <summary>
/// Native map-game task structure encoded by the shared 1.72.84 helper at 0x100668c08.
/// Semantic names for the stripped scalar fields are not yet proven.
/// </summary>
public sealed record MapGameTask
{
    /// <summary>
    /// Initializes a new <see cref="MapGameTask"/> instance.
    /// </summary>
    public MapGameTask(
        int taskGlobalIdentifier,
        int unknown0,
        int unknown1,
        int unknown2,
        int unknown3,
        int unknown4,
        CommandVariableIntPair unknownPair0,
        CommandVariableIntPair unknownPair1,
        bool unknownBoolean0,
        bool unknownBoolean1,
        bool unknownBoolean2,
        ReadOnlyMemory<CommandVariableIntPair> unknownPairs0,
        ReadOnlyMemory<CommandVariableIntPair> unknownPairs1,
        ReadOnlyMemory<MapGameTaskState> states
    )
    {
        TaskGlobalIdentifier = taskGlobalIdentifier;
        Unknown0 = unknown0;
        Unknown1 = unknown1;
        Unknown2 = unknown2;
        Unknown3 = unknown3;
        Unknown4 = unknown4;
        UnknownPair0 = unknownPair0;
        UnknownPair1 = unknownPair1;
        UnknownBoolean0 = unknownBoolean0;
        UnknownBoolean1 = unknownBoolean1;
        UnknownBoolean2 = unknownBoolean2;
        UnknownPairs0 = unknownPairs0.ToArray();
        UnknownPairs1 = unknownPairs1.ToArray();
        States = states.ToArray();
    }

    /// <summary>
    /// Gets the <c language="csharp">States</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTaskState> States { get; }

    /// <summary>
    /// Gets the <c language="csharp">TaskGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("TaskGlobalId")]
    public int TaskGlobalIdentifier { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown0</c> value.
    /// </summary>
    public int Unknown0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown1</c> value.
    /// </summary>
    public int Unknown1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown2</c> value.
    /// </summary>
    public int Unknown2 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown3</c> value.
    /// </summary>
    public int Unknown3 { get; }

    /// <summary>
    /// Gets the <c language="csharp">Unknown4</c> value.
    /// </summary>
    public int Unknown4 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownBoolean0</c> value.
    /// </summary>
    public bool UnknownBoolean0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownBoolean1</c> value.
    /// </summary>
    public bool UnknownBoolean1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownBoolean2</c> value.
    /// </summary>
    public bool UnknownBoolean2 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownPair0</c> value.
    /// </summary>
    public CommandVariableIntPair UnknownPair0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownPair1</c> value.
    /// </summary>
    public CommandVariableIntPair UnknownPair1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownPairs0</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandVariableIntPair> UnknownPairs0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownPairs1</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandVariableIntPair> UnknownPairs1 { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameTask Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int taskGlobalIdentifier = stream.ReadVariableInt();
        int unknown0 = stream.ReadVariableInt();
        int unknown1 = stream.ReadVariableInt();
        int unknown2 = stream.ReadVariableInt();
        int unknown3 = stream.ReadVariableInt();
        int unknown4 = stream.ReadVariableInt();
        CommandVariableIntPair unknownPair0 = new(stream.ReadVariableInt(), stream.ReadVariableInt());
        CommandVariableIntPair unknownPair1 = new(stream.ReadVariableInt(), stream.ReadVariableInt());
        bool unknownBoolean0 = stream.ReadBoolean();
        bool unknownBoolean1 = stream.ReadBoolean();
        bool unknownBoolean2 = stream.ReadBoolean();
        ReadOnlyMemory<CommandVariableIntPair> unknownPairs0 = CommandVariableIntPairArrayField.Decode(stream).Values;
        ReadOnlyMemory<CommandVariableIntPair> unknownPairs1 = CommandVariableIntPairArrayField.Decode(stream).Values;
        int stateCount = MapGameFieldCodec.ReadCount(stream, name: "task-state");
        MapGameTaskState[] states = new MapGameTaskState[stateCount];

        for (int index = 0; index < states.Length; index++)
            states[index] = MapGameTaskState.Decode(stream, dataResolver);

        return new MapGameTask(
            taskGlobalIdentifier,
            unknown0,
            unknown1,
            unknown2,
            unknown3,
            unknown4,
            unknownPair0,
            unknownPair1,
            unknownBoolean0,
            unknownBoolean1,
            unknownBoolean2,
            unknownPairs0,
            unknownPairs1,
            states
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(TaskGlobalIdentifier);
        stream.WriteVariableInt(Unknown0);
        stream.WriteVariableInt(Unknown1);
        stream.WriteVariableInt(Unknown2);
        stream.WriteVariableInt(Unknown3);
        stream.WriteVariableInt(Unknown4);
        stream.WriteVariableInt(UnknownPair0.Value0);
        stream.WriteVariableInt(UnknownPair0.Value1);
        stream.WriteVariableInt(UnknownPair1.Value0);
        stream.WriteVariableInt(UnknownPair1.Value1);
        stream.WriteBoolean(UnknownBoolean0);
        stream.WriteBoolean(UnknownBoolean1);
        stream.WriteBoolean(UnknownBoolean2);
        new CommandVariableIntPairArrayField(UnknownPairs0).Encode(stream);
        new CommandVariableIntPairArrayField(UnknownPairs1).Encode(stream);
        stream.WriteVariableInt(States.Length);

        foreach (MapGameTaskState state in States.Span)
            state.Encode(stream);
    }
}
