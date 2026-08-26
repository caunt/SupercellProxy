using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>HomeSnapshot</c> home data.
/// </summary>
public sealed record HomeSnapshot
{
    /// <summary>
    /// Gets or sets the <c>ObjectVersion</c> value.
    /// </summary>
    public int ObjectVersion { get; init; }

    /// <summary>
    /// Gets or sets the <c>TileMapWidth</c> value.
    /// </summary>
    public int TileMapWidth { get; init; }

    /// <summary>
    /// Gets or sets the <c>TileMapHeight</c> value.
    /// </summary>
    public int TileMapHeight { get; init; }

    /// <summary>
    /// Gets or sets the <c>Objects</c> value.
    /// </summary>
    public GameObjectSnapshot[] Objects { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>TimeLists</c> value.
    /// </summary>
    public GameObjectTimeListSnapshot[] TimeLists { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>CreatedAnimalProducts</c> value.
    /// </summary>
    public int[] CreatedAnimalProducts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>MineProducts</c> value.
    /// </summary>
    public int[] MineProducts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>SmelterProducts</c> value.
    /// </summary>
    public int[] SmelterProducts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c>ExpansionReadyDatas</c> value.
    /// </summary>
    public ExpansionReadyDataSnapshot[] ExpansionReadyDatas { get; init; } = [];

    internal GameObjectState[] ResolveGameObjects(DataTableResolver dataTableResolver)
    {
        var instanceCounts = new Dictionary<int, int>();
        var gameObjects = new GameObjectState[Objects.Length];

        for (var i = 0; i < Objects.Length; i++)
        {
            var gameObject = Objects[i];
            var tableId = gameObject.DataGlobalId / DataTableResolver.GlobalIdTableSize;
            instanceCounts.TryGetValue(tableId, out var instanceId);
            instanceCounts[tableId] = instanceId + 1;

            if (!dataTableResolver.TryResolve(gameObject.DataGlobalId, out var data))
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Unresolved game-object data global ID {gameObject.DataGlobalId}."
                    )
                );

            var dimensions = GameObjectDimensionsResolver.Resolve(data, dataTableResolver);

            gameObjects[i] = new GameObjectState(
                tableId * DataTableResolver.GlobalIdTableSize + instanceId,
                gameObject,
                data,
                dimensions.Width,
                dimensions.Height
            );
        }

        return gameObjects;
    }
}
