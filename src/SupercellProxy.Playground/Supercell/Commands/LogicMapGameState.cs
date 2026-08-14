using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Supercell.Commands;

/// <summary>
/// Native map-game state structure encoded by the shared 1.72.84 helper at 0x10064b114.
/// </summary>
public sealed record LogicMapGameState
{
    public LogicMapGameState(
        ReadOnlyMemory<LogicMapGamePawn> pawns,
        ReadOnlyMemory<LogicMapGameTaskGroup> taskGroups,
        LogicMapGameConfiguration configuration,
        ReadOnlyMemory<LogicMapGameStateEntry> entries,
        int unknownGlobalId)
    {
        Pawns = pawns.ToArray();
        TaskGroups = taskGroups.ToArray();
        Configuration = configuration;
        Entries = entries.ToArray();
        UnknownGlobalId = unknownGlobalId;
    }

    public ReadOnlyMemory<LogicMapGamePawn> Pawns { get; }
    public ReadOnlyMemory<LogicMapGameTaskGroup> TaskGroups { get; }
    public LogicMapGameConfiguration Configuration { get; }
    public ReadOnlyMemory<LogicMapGameStateEntry> Entries { get; }
    public int UnknownGlobalId { get; }

    internal static LogicMapGameState Decode(SupercellStream stream, ILogicCommandDataResolver? dataResolver)
    {
        var pawnCount = LogicMapGameWire.ReadCount(stream, "state pawn");
        var pawns = new LogicMapGamePawn[pawnCount];

        for (var i = 0; i < pawns.Length; i++)
            pawns[i] = LogicMapGamePawn.Decode(stream);

        var taskGroupCount = LogicMapGameWire.ReadCount(stream, "task group");
        var taskGroups = new LogicMapGameTaskGroup[taskGroupCount];

        for (var i = 0; i < taskGroups.Length; i++)
            taskGroups[i] = LogicMapGameTaskGroup.Decode(stream, dataResolver);

        var configuration = LogicMapGameConfiguration.Decode(stream);
        var entryCount = LogicMapGameWire.ReadCount(stream, "state entry");
        var entries = new LogicMapGameStateEntry[entryCount];

        for (var i = 0; i < entries.Length; i++)
            entries[i] = LogicMapGameStateEntry.Decode(stream);

        return new LogicMapGameState(pawns, taskGroups, configuration, entries, stream.ReadVarInt());
    }

    internal void Encode(SupercellStream stream)
    {
        stream.WriteVarInt(Pawns.Length);

        foreach (var pawn in Pawns.Span)
            pawn.Encode(stream);

        stream.WriteVarInt(TaskGroups.Length);

        foreach (var taskGroup in TaskGroups.Span)
            taskGroup.Encode(stream);

        Configuration.Encode(stream);
        stream.WriteVarInt(Entries.Length);

        foreach (var entry in Entries.Span)
            entry.Encode(stream);

        stream.WriteVarInt(UnknownGlobalId);
    }
}
