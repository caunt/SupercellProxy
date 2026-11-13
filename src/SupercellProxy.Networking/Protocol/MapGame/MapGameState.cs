using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>The whole Valley board as the server sends it: the players' cars, the tasks on the nodes, counters and emojis.</summary>
public sealed record MapGameState : ICodecProvider<MapGameState>
{
    /// <summary>Initializes a new instance of the <see cref="MapGameState"/> record. Creates the board state from its decoded parts.</summary>
    /// <param name="pawns">Cars of the board's players.</param>
    /// <param name="taskGroups">Tasks on the board, grouped by the player they belong to.</param>
    /// <param name="configuration">The board's daily counters and chicken log.</param>
    /// <param name="emojis">Emojis players left on the nodes.</param>
    /// <param name="mapGlobalId">Global id of the board's map table entry.</param>
    public MapGameState(ReadOnlyMemory<MapGamePawn> pawns,
        ReadOnlyMemory<MapGameTaskGroup> taskGroups,
        MapGameConfiguration configuration,
        ReadOnlyMemory<MapGameNodeEmoji> emojis,
        int mapGlobalId)
    {
        // Cars and task groups are copied so the record keeps no reference to the caller's buffers.
        Pawns = pawns.ToArray();
        TaskGroups = taskGroups.ToArray();

        // Then the board-wide counters, the node emojis and the map the board uses.
        Configuration = configuration;
        Emojis = emojis.ToArray();
        MapGlobalId = mapGlobalId;
    }

    /// <inheritdoc />
    public static ICodec<MapGameState> Codec { get; } = Codecs.Composite(Codecs.Memory(MapGamePawn.Codec),
        static state => state.Pawns,
        Codecs.Memory(MapGameTaskGroup.Codec),
        static state => state.TaskGroups,
        MapGameConfiguration.Codec,
        static state => state.Configuration,
        Codecs.Memory(MapGameNodeEmoji.Codec),
        static state => state.Emojis,
        Codecs.VarInt,
        static state => state.MapGlobalId,
        static (pawns, taskGroups, configuration, emojis, mapGlobalId) => new MapGameState(pawns, taskGroups, configuration, emojis, mapGlobalId));

    /// <summary>Gets the board's daily spawn and delivery counters and its chicken log.</summary>
    /// <value>The board-wide configuration block, sent after <see cref="TaskGroups"/>.</value>
    public MapGameConfiguration Configuration { get; }

    /// <summary>Gets or sets the emojis players left on the board's nodes (saved as <c>Entries</c>).</summary>
    /// <value>Emojis in board order, sent after <see cref="Configuration"/>. Empty when no node has one.</value>
    [JsonPropertyName("Entries")]
    public ReadOnlyMemory<MapGameNodeEmoji> Emojis { get; init; }

    /// <summary>Gets the global id of the board's map table entry.</summary>
    /// <value>The global id of the Valley maps row whose node graph the board uses, the last field on the wire.</value>
    public int MapGlobalId { get; }

    /// <summary>Gets the cars of the board's players.</summary>
    /// <value>One car per player in the Valley game, the first field on the wire.</value>
    public ReadOnlyMemory<MapGamePawn> Pawns { get; }

    /// <summary>Gets the tasks on the board, grouped by the player they belong to.</summary>
    /// <value>One group per owner, each holding that owner's tasks, sent after <see cref="Pawns"/>.</value>
    public ReadOnlyMemory<MapGameTaskGroup> TaskGroups { get; }
}
