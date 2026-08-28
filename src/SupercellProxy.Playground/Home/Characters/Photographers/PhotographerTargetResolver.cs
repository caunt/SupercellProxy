using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class PhotographerTargetResolver
{
    private const int DecorationEventBuildingTableId = 293;
    private const int DecorationTableId = 3;
    private const int ExpLevelGlobalId = 1_900_002;
    private const int ObjectMoveModeUnlockLevel = 3;
    private readonly GameObjectState[] _gameObjects;
    private readonly int _gridHeight;
    private readonly int _gridWidth;
    private readonly InventoryState _inventory;
    private readonly DataTableResolver _dataTableResolver;

    public PhotographerTargetResolver(
        GameObjectState[] gameObjects,
        int gridWidth,
        int gridHeight,
        InventoryState inventory,
        DataTableResolver dataTableResolver
    )
    {
        ArgumentNullException.ThrowIfNull(gameObjects);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gridWidth);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gridHeight);
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(dataTableResolver);
        _gameObjects = gameObjects;
        _gridWidth = gridWidth;
        _gridHeight = gridHeight;
        _inventory = inventory;
        _dataTableResolver = dataTableResolver;
    }

    public IntPair? Resolve(GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);
        var target = ResolveTargetObject(random);

        if (target is null || !TryResolvePresentationTile(target, out var tileX, out var tileY))
            return null;

        return new IntPair(
            checked(tileX * GameObjectState.TileSize + NextOriginOffset(random)),
            checked(tileY * GameObjectState.TileSize + NextOriginOffset(random))
        );
    }

    private GameObjectState? ResolveTargetObject(GameRandom random)
    {
        if (IsObjectMoveModeUnlocked())
        {
            var candidates = _gameObjects
                .Where(static gameObject => gameObject.Data.TableId is DecorationTableId)
                .ToArray();

            if (candidates.Length > 0)
                return candidates[random.NextInt(candidates.Length)];
        }

        return _gameObjects.FirstOrDefault(static gameObject =>
            gameObject.Data.TableId is DecorationEventBuildingTableId
        );
    }

    private bool IsObjectMoveModeUnlocked()
    {
        if (!_dataTableResolver.TryResolve(ExpLevelGlobalId, out var experienceLevelData))
            throw new InvalidDataException("Unable to resolve money.csv ExpLevel data.");

        return _inventory.GetTotalValue(experienceLevelData) >= ObjectMoveModeUnlockLevel;
    }

    private bool TryResolvePresentationTile(
        GameObjectState target,
        out int targetTileX,
        out int targetTileY
    )
    {
        var absoluteX = 0;
        var absoluteY = 0;

        for (var current = target; current is not null; current = current.Parent)
        {
            absoluteX = checked(absoluteX + current.PositionX);
            absoluteY = checked(absoluteY + current.PositionY);
        }

        var startTileX = absoluteX >> 9;
        var startTileY = absoluteY >> 9;
        var width = RequireDimension(target.TileWidth, target, "width");
        var height = RequireDimension(target.TileHeight, target, "height");

        if (target.Mirrored)
            (width, height) = (height, width);

        for (var y = startTileY; y < checked(startTileY + height); y++)
        {
            for (var x = startTileX; x < checked(startTileX + width); x++)
            {
                if (x < 0 || y < 0 || x >= _gridWidth || y >= _gridHeight)
                    continue;

                targetTileX = x;
                targetTileY = y;
                return true;
            }
        }

        targetTileX = default;
        targetTileY = default;
        return false;
    }

    private static int RequireDimension(int? value, GameObjectState target, string name)
    {
        return value is > 0
            ? value.Value
            : throw new InvalidDataException(
                $"Decoration {target.Data.Name} has no valid tile {name}."
            );
    }

    private static int NextOriginOffset(GameRandom random)
    {
        return checked(
            (4 * GameObjectState.TileCenter + GameObjectState.TileSize * random.NextInt(7)) / 10
        );
    }
}
