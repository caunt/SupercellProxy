using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Native map-game state structure encoded by the shared 1.72.84 helper at 0x10064b114.</para>
/// </summary>
internal sealed record MapGameState
{
    /// <summary>
    /// Initializes a new <see cref="MapGameState"/> instance.
    /// </summary>
    public MapGameState(
        ReadOnlyMemory<MapGamePawn> pawns,
        ReadOnlyMemory<MapGameTaskGroup> taskGroups,
        MapGameConfiguration configuration,
        ReadOnlyMemory<MapGameStateEntry> entries,
        int unknownGlobalId
    )
    {
        Pawns = pawns.ToArray();
        TaskGroups = taskGroups.ToArray();
        Configuration = configuration;
        Entries = entries.ToArray();
        UnknownGlobalId = unknownGlobalId;
    }

    /// <summary>
    /// Gets the <c language="csharp">Pawns</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGamePawn> Pawns { get; }

    /// <summary>
    /// Gets the <c language="csharp">TaskGroups</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTaskGroup> TaskGroups { get; }

    /// <summary>
    /// Gets the <c language="csharp">Configuration</c> value.
    /// </summary>
    public MapGameConfiguration Configuration { get; }

    /// <summary>
    /// Gets the <c language="csharp">Entries</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameStateEntry> Entries { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalId</c> value.
    /// </summary>
    public int UnknownGlobalId { get; }

    internal static MapGameState Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        var pawnCount = MapGameWire.ReadCount(stream, "state pawn");
        var pawns = new MapGamePawn[pawnCount];

        for (var i = 0; i < pawns.Length; i++)
            pawns[i] = MapGamePawn.Decode(stream);

        var taskGroupCount = MapGameWire.ReadCount(stream, "task group");
        var taskGroups = new MapGameTaskGroup[taskGroupCount];

        for (var i = 0; i < taskGroups.Length; i++)
            taskGroups[i] = MapGameTaskGroup.Decode(stream, dataResolver);

        var configuration = MapGameConfiguration.Decode(stream);
        var entryCount = MapGameWire.ReadCount(stream, "state entry");
        var entries = new MapGameStateEntry[entryCount];

        for (var i = 0; i < entries.Length; i++)
            entries[i] = MapGameStateEntry.Decode(stream);

        return new MapGameState(pawns, taskGroups, configuration, entries, stream.ReadVarInt());
    }

    internal void Encode(MessageStream stream)
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
