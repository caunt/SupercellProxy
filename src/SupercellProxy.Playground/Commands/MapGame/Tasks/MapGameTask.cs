using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// Native map-game task structure encoded by the shared 1.72.84 helper at 0x100668c08.
/// Semantic names for the stripped scalar fields are not yet proven.
/// </summary>
internal sealed record MapGameTask
{
    /// <summary>
    /// Initializes a new <see cref="MapGameTask"/> instance.
    /// </summary>
    public MapGameTask(
        int taskGlobalId,
        int unknown0,
        int unknown1,
        int unknown2,
        int unknown3,
        int unknown4,
        CommandVarIntPair unknownPair0,
        CommandVarIntPair unknownPair1,
        bool unknownBoolean0,
        bool unknownBoolean1,
        bool unknownBoolean2,
        ReadOnlyMemory<CommandVarIntPair> unknownPairs0,
        ReadOnlyMemory<CommandVarIntPair> unknownPairs1,
        ReadOnlyMemory<MapGameTaskState> states
    )
    {
        TaskGlobalId = taskGlobalId;
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
    /// Gets the <c language="csharp">TaskGlobalId</c> value.
    /// </summary>
    public int TaskGlobalId { get; }

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
    /// Gets the <c language="csharp">UnknownPair0</c> value.
    /// </summary>
    public CommandVarIntPair UnknownPair0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownPair1</c> value.
    /// </summary>
    public CommandVarIntPair UnknownPair1 { get; }

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
    /// Gets the <c language="csharp">UnknownPairs0</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandVarIntPair> UnknownPairs0 { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownPairs1</c> value.
    /// </summary>
    public ReadOnlyMemory<CommandVarIntPair> UnknownPairs1 { get; }

    /// <summary>
    /// Gets the <c language="csharp">States</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTaskState> States { get; }

    internal static MapGameTask Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        var taskGlobalId = stream.ReadVarInt();
        var unknown0 = stream.ReadVarInt();
        var unknown1 = stream.ReadVarInt();
        var unknown2 = stream.ReadVarInt();
        var unknown3 = stream.ReadVarInt();
        var unknown4 = stream.ReadVarInt();
        var unknownPair0 = new CommandVarIntPair(stream.ReadVarInt(), stream.ReadVarInt());
        var unknownPair1 = new CommandVarIntPair(stream.ReadVarInt(), stream.ReadVarInt());
        var unknownBoolean0 = stream.ReadBoolean();
        var unknownBoolean1 = stream.ReadBoolean();
        var unknownBoolean2 = stream.ReadBoolean();
        var unknownPairs0 = CommandVarIntPairArrayField.Decode(stream).Values;
        var unknownPairs1 = CommandVarIntPairArrayField.Decode(stream).Values;
        var stateCount = MapGameWire.ReadCount(stream, "task-state");
        var states = new MapGameTaskState[stateCount];

        for (var i = 0; i < states.Length; i++)
            states[i] = MapGameTaskState.Decode(stream, dataResolver);

        return new MapGameTask(
            taskGlobalId,
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

    internal void Encode(MessageStream stream)
    {
        stream.WriteVarInt(TaskGlobalId);
        stream.WriteVarInt(Unknown0);
        stream.WriteVarInt(Unknown1);
        stream.WriteVarInt(Unknown2);
        stream.WriteVarInt(Unknown3);
        stream.WriteVarInt(Unknown4);
        stream.WriteVarInt(UnknownPair0.Value0);
        stream.WriteVarInt(UnknownPair0.Value1);
        stream.WriteVarInt(UnknownPair1.Value0);
        stream.WriteVarInt(UnknownPair1.Value1);
        stream.WriteBoolean(UnknownBoolean0);
        stream.WriteBoolean(UnknownBoolean1);
        stream.WriteBoolean(UnknownBoolean2);
        new CommandVarIntPairArrayField(UnknownPairs0).Encode(stream);
        new CommandVarIntPairArrayField(UnknownPairs1).Encode(stream);
        stream.WriteVarInt(States.Length);

        foreach (var state in States.Span)
            state.Encode(stream);
    }
}
