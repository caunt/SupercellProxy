using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Native map-game task structure encoded by the shared 1.72.84 helper at 0x100668c08.
/// Semantic names for the stripped scalar fields are not yet proven.
/// </summary>
public sealed record LogicMapGameTask
{
    public LogicMapGameTask(
        int taskGlobalId,
        int unknown0,
        int unknown1,
        int unknown2,
        int unknown3,
        int unknown4,
        LogicCommandVarIntPair unknownPair0,
        LogicCommandVarIntPair unknownPair1,
        bool unknownBoolean0,
        bool unknownBoolean1,
        bool unknownBoolean2,
        ReadOnlyMemory<LogicCommandVarIntPair> unknownPairs0,
        ReadOnlyMemory<LogicCommandVarIntPair> unknownPairs1,
        ReadOnlyMemory<LogicMapGameTaskState> states)
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

    public int TaskGlobalId { get; }
    public int Unknown0 { get; }
    public int Unknown1 { get; }
    public int Unknown2 { get; }
    public int Unknown3 { get; }
    public int Unknown4 { get; }
    public LogicCommandVarIntPair UnknownPair0 { get; }
    public LogicCommandVarIntPair UnknownPair1 { get; }
    public bool UnknownBoolean0 { get; }
    public bool UnknownBoolean1 { get; }
    public bool UnknownBoolean2 { get; }
    public ReadOnlyMemory<LogicCommandVarIntPair> UnknownPairs0 { get; }
    public ReadOnlyMemory<LogicCommandVarIntPair> UnknownPairs1 { get; }
    public ReadOnlyMemory<LogicMapGameTaskState> States { get; }

    internal static LogicMapGameTask Decode(SupercellStream stream, ILogicCommandDataResolver? dataResolver)
    {
        var taskGlobalId = stream.ReadVarInt();
        var unknown0 = stream.ReadVarInt();
        var unknown1 = stream.ReadVarInt();
        var unknown2 = stream.ReadVarInt();
        var unknown3 = stream.ReadVarInt();
        var unknown4 = stream.ReadVarInt();
        var unknownPair0 = new LogicCommandVarIntPair(stream.ReadVarInt(), stream.ReadVarInt());
        var unknownPair1 = new LogicCommandVarIntPair(stream.ReadVarInt(), stream.ReadVarInt());
        var unknownBoolean0 = stream.ReadBoolean();
        var unknownBoolean1 = stream.ReadBoolean();
        var unknownBoolean2 = stream.ReadBoolean();
        var unknownPairs0 = LogicCommandVarIntPairArrayField.Decode(stream).Values;
        var unknownPairs1 = LogicCommandVarIntPairArrayField.Decode(stream).Values;
        var stateCount = LogicMapGameWire.ReadCount(stream, "task-state");
        var states = new LogicMapGameTaskState[stateCount];

        for (var i = 0; i < states.Length; i++)
            states[i] = LogicMapGameTaskState.Decode(stream, dataResolver);

        return new LogicMapGameTask(
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
            states);
    }

    internal void Encode(SupercellStream stream)
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
        new LogicCommandVarIntPairArrayField(UnknownPairs0).Encode(stream);
        new LogicCommandVarIntPairArrayField(UnknownPairs1).Encode(stream);
        stream.WriteVarInt(States.Length);

        foreach (var state in States.Span)
            state.Encode(stream);
    }
}
