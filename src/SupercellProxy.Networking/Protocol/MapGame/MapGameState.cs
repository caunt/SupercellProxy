using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>
/// <para>Native map-game state structure encoded by the shared 1.72.84 helper at 0x10064b114.</para>
/// </summary>
public sealed record MapGameState
{
    /// <summary>
    /// Initializes a new <see cref="MapGameState"/> instance.
    /// </summary>
    public MapGameState(
        ReadOnlyMemory<MapGamePawn> pawns,
        ReadOnlyMemory<MapGameTaskGroup> taskGroups,
        MapGameConfiguration configuration,
        ReadOnlyMemory<MapGameStateEntry> entries,
        int unknownGlobalIdentifier
    )
    {
        Pawns = pawns.ToArray();
        TaskGroups = taskGroups.ToArray();
        Configuration = configuration;
        Entries = entries.ToArray();
        UnknownGlobalIdentifier = unknownGlobalIdentifier;
    }

    /// <summary>
    /// Gets the <c language="csharp">Configuration</c> value.
    /// </summary>
    public MapGameConfiguration Configuration { get; }

    /// <summary>
    /// Gets the <c language="csharp">Entries</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameStateEntry> Entries { get; }

    /// <summary>
    /// Gets the <c language="csharp">Pawns</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGamePawn> Pawns { get; }

    /// <summary>
    /// Gets the <c language="csharp">TaskGroups</c> value.
    /// </summary>
    public ReadOnlyMemory<MapGameTaskGroup> TaskGroups { get; }

    /// <summary>
    /// Gets the <c language="csharp">UnknownGlobalId</c> value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("UnknownGlobalId")]
    public int UnknownGlobalIdentifier { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static MapGameState Decode(MessageStream stream, ICommandDataResolver? dataResolver)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int pawnCount = MapGameFieldCodec.ReadCount(stream, name: "state pawn");
        MapGamePawn[] pawns = new MapGamePawn[pawnCount];

        for (int index = 0; index < pawns.Length; index++)
            pawns[index] = MapGamePawn.Decode(stream);

        int taskGroupCount = MapGameFieldCodec.ReadCount(stream, name: "task group");
        MapGameTaskGroup[] taskGroups = new MapGameTaskGroup[taskGroupCount];

        for (int index = 0; index < taskGroups.Length; index++)
            taskGroups[index] = MapGameTaskGroup.Decode(stream, dataResolver);

        MapGameConfiguration configuration = MapGameConfiguration.Decode(stream);
        int entryCount = MapGameFieldCodec.ReadCount(stream, name: "state entry");
        MapGameStateEntry[] entries = new MapGameStateEntry[entryCount];

        for (int index = 0; index < entries.Length; index++)
            entries[index] = MapGameStateEntry.Decode(stream);

        return new MapGameState(pawns, taskGroups, configuration, entries, stream.ReadVariableInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteVariableInt(Pawns.Length);

        foreach (MapGamePawn pawn in Pawns.Span)
            pawn.Encode(stream);

        stream.WriteVariableInt(TaskGroups.Length);

        foreach (MapGameTaskGroup taskGroup in TaskGroups.Span)
            taskGroup.Encode(stream);

        Configuration.Encode(stream);
        stream.WriteVariableInt(Entries.Length);

        foreach (MapGameStateEntry entry in Entries.Span)
            entry.Encode(stream);

        stream.WriteVariableInt(UnknownGlobalIdentifier);
    }
}
