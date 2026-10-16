using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.MapGame.Tasks;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame;

/// <summary>The whole Valley board as the server sends it: the players' cars, the tasks on the nodes, counters and emojis.</summary>
public sealed record MapGameState : ICodecProvider<MapGameState>
{
    /// <summary>Creates the board state from its decoded parts.</summary>
    /// <param name="pawns">Cars of the board's players.</param>
    /// <param name="taskGroups">Tasks on the board, grouped by the player they belong to.</param>
    /// <param name="configuration">The board's daily counters and chicken log.</param>
    /// <param name="emojis">Emojis players left on the nodes.</param>
    /// <param name="mapGlobalId">Global id of the board's map table entry.</param>
    public MapGameState(
        ReadOnlyMemory<MapGamePawn> pawns,
        ReadOnlyMemory<MapGameTaskGroup> taskGroups,
        MapGameConfiguration configuration,
        ReadOnlyMemory<MapGameNodeEmoji> emojis,
        int mapGlobalId
    )
    {
        Pawns = pawns.ToArray();
        TaskGroups = taskGroups.ToArray();
        Configuration = configuration;
        Emojis = emojis.ToArray();
        MapGlobalId = mapGlobalId;
    }

    /// <inheritdoc />
    public static ICodec<MapGameState> Codec { get; } = Codecs.Composite(
        Codecs.Memory(MapGamePawn.Codec),
        static state => state.Pawns,
        Codecs.Memory(MapGameTaskGroup.Codec),
        static state => state.TaskGroups,
        MapGameConfiguration.Codec,
        static state => state.Configuration,
        Codecs.Memory(MapGameNodeEmoji.Codec),
        static state => state.Emojis,
        Codecs.VarInt,
        static state => state.MapGlobalId,
        static (pawns, taskGroups, configuration, emojis, mapGlobalId) => new MapGameState(pawns, taskGroups, configuration, emojis, mapGlobalId)
    );

    /// <summary>The board's daily spawn and delivery counters and its chicken log.</summary>
    public MapGameConfiguration Configuration { get; }

    /// <summary>Emojis players left on the board's nodes (saved as <c>Entries</c>).</summary>
    [JsonPropertyName("Entries")]
    public ReadOnlyMemory<MapGameNodeEmoji> Emojis { get; init; }

    /// <summary>Global id of the board's map table entry.</summary>
    public int MapGlobalId { get; }

    /// <summary>Cars of the board's players.</summary>
    public ReadOnlyMemory<MapGamePawn> Pawns { get; }

    /// <summary>Tasks on the board, grouped by the player they belong to.</summary>
    public ReadOnlyMemory<MapGameTaskGroup> TaskGroups { get; }
}
