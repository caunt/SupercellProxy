using System.Globalization;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">HomeSnapshot</c> home data.
/// </summary>
internal sealed record HomeSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">ObjectVersion</c> value.
    /// </summary>
    public int ObjectVersion { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TileMapWidth</c> value.
    /// </summary>
    public int TileMapWidth { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">TileMapHeight</c> value.
    /// </summary>
    public int TileMapHeight { get; init; }

    /// <summary>
    /// Gets or sets the <c language="csharp">Objects</c> value.
    /// </summary>
    public GameObjectSnapshot[] Objects { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">TimeLists</c> value.
    /// </summary>
    public GameObjectTimeListSnapshot[] TimeLists { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">CreatedAnimalProducts</c> value.
    /// </summary>
    public int[] CreatedAnimalProducts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">MineProducts</c> value.
    /// </summary>
    public int[] MineProducts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">SmelterProducts</c> value.
    /// </summary>
    public int[] SmelterProducts { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">ExpansionReadyDatas</c> value.
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
