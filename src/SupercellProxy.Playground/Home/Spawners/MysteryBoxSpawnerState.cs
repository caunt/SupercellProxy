using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed record MysteryBoxSpawnerState(
    GameObjectState GameObject,
    int OwnSpawnInterval,
    int FriendSpawnInterval,
    bool ReplacementEnabled
)
{
    private const int OwnPlacementOffset = 8;
    private const int OwnPlacementRange = 14;
    private const int MaximumPlacementAttempts = 5_000;
    private bool loadedBoxReconciled;

    public IntPair? ReconciledPosition { get; private set; }

    public static MysteryBoxSpawnerState Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom,
        InventoryState inventory
    )
    {
        var gameObject = gameObjects.Single(static gameObject => gameObject.Data.TableId is 54);
        var ownSpawnInterval = ResolveSpawnInterval(
            gameObject,
            dataTableResolver,
            constructorRandom,
            "MinSpawnTimeOpened",
            "MaxSpawnTimeOpened"
        );
        var friendSpawnInterval = ResolveSpawnInterval(
            gameObject,
            dataTableResolver,
            constructorRandom,
            "MinSpawnTimeOpenedFriend",
            "MaxSpawnTimeOpenedFriend"
        );

        if (
            !dataTableResolver.TryResolve(
                "data/money.csv",
                "ShowMysteryBoxAtFriend",
                out var showMysteryBoxAtFriend
            )
        )
        {
            throw new InvalidDataException(
                "Unable to resolve ShowMysteryBoxAtFriend from data/money.csv."
            );
        }

        inventory.TryGetValue(0, showMysteryBoxAtFriend, out var showMysteryBox);

        if (showMysteryBox < 0)
            throw new InvalidDataException("ShowMysteryBoxAtFriend cannot be negative.");

        return new MysteryBoxSpawnerState(
            gameObject,
            ownSpawnInterval,
            friendSpawnInterval,
            showMysteryBox > 0
        );
    }

    public void ReconcileLoadedBox(GameObjectState[] gameObjects, GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(gameObjects);
        ArgumentNullException.ThrowIfNull(random);

        if (loadedBoxReconciled)
            throw new InvalidOperationException(
                "The loaded mystery box has already been reconciled."
            );

        var loadedBoxes = gameObjects
            .Where(static gameObject => gameObject.Data.TableId is 53)
            .ToArray();

        if (
            loadedBoxes.Length is not 1
            || loadedBoxes[0].TileWidth is not 2
            || loadedBoxes[0].TileHeight is not 2
        )
        {
            throw new NotSupportedException(
                $"Mystery-box reconciliation requires one loaded 2x2 box; found {loadedBoxes.Length}."
            );
        }

        if (!ReplacementEnabled)
        {
            loadedBoxReconciled = true;
            return;
        }

        for (var attempt = 0; attempt < MaximumPlacementAttempts; attempt++)
        {
            var x = OwnPlacementOffset + random.NextInt(OwnPlacementRange);
            var y = OwnPlacementOffset + random.NextInt(OwnPlacementRange);

            if (
                gameObjects.Any(gameObject =>
                    gameObject.Data.TableId is not 49 and not 53 && Overlaps(gameObject, x, y, 2, 2)
                )
            )
            {
                continue;
            }

            loadedBoxes[0].MoveTo(x << 9, y << 9);
            loadedBoxes[0].SetMirrored(mirrored: false);
            ReconciledPosition = new IntPair(x, y);
            loadedBoxReconciled = true;
            return;
        }

        throw new NotSupportedException(
            $"Mystery-box reconciliation found no free position in {MaximumPlacementAttempts} attempts."
        );
    }

    private static int ResolveSpawnInterval(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom,
        string minimumFieldName,
        string maximumFieldName
    )
    {
        if (
            !dataTableResolver.TryResolveInt(
                gameObject.Data.GlobalId,
                minimumFieldName,
                out var minimum
            )
            || !dataTableResolver.TryResolveInt(
                gameObject.Data.GlobalId,
                maximumFieldName,
                out var maximum
            )
            || maximum < minimum
        )
        {
            throw new InvalidDataException(
                $"Mystery-box spawner {gameObject.Data.Name} has an invalid native spawn-time range."
            );
        }

        return minimum + constructorRandom.NextInt(maximum - minimum);
    }

    private static bool Overlaps(GameObjectState gameObject, int x, int y, int width, int height)
    {
        if (gameObject.TileWidth is not > 0 || gameObject.TileHeight is not > 0)
            return false;

        var objectWidth = gameObject.Mirrored
            ? gameObject.TileHeight.Value
            : gameObject.TileWidth.Value;
        var objectHeight = gameObject.Mirrored
            ? gameObject.TileWidth.Value
            : gameObject.TileHeight.Value;
        var logicX = 0;
        var logicY = 0;

        for (var current = gameObject; current is not null; current = current.Parent)
        {
            logicX = checked(logicX + current.PositionX);
            logicY = checked(logicY + current.PositionY);
        }

        var objectX = logicX >> 9;
        var objectY = logicY >> 9;
        return x < objectX + objectWidth
            && objectX < x + width
            && y < objectY + objectHeight
            && objectY < y + height;
    }
}
