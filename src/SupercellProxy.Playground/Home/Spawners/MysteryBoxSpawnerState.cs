using System.Globalization;
using System.Text.Json;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class MysteryBoxSpawnerState
{
    private const int OwnPlacementOffset = 8;
    private const int OwnPlacementRange = 14;
    private const int MaximumPlacementAttempts = 5_000;
    private readonly DataTableReference _friendMysteryBoxStateData;
    private readonly DataTableReference _mysteryBoxRandomSeedData;
    private bool _loadedBoxReconciled;

    private MysteryBoxSpawnerState(
        GameObjectState gameObject,
        int ownElapsedUpdates,
        int ownInterval,
        int friendElapsedUpdates,
        int friendInterval,
        int spawnedBoxCount,
        AttachedTimerState ownOpenedBoxTimer,
        AttachedTimerState friendOpenedBoxTimer,
        int retainedUpdateTick,
        bool ownLastOpened,
        bool friendLastOpened,
        int ownOpenMinimumInterval,
        int ownOpenMaximumInterval,
        int ownClosedMinimumInterval,
        int ownClosedMaximumInterval,
        int friendOpenMinimumInterval,
        int friendOpenMaximumInterval,
        int friendClosedMinimumInterval,
        int friendClosedMaximumInterval,
        DataTableReference mysteryBoxRandomSeedData,
        int mysteryBoxRandomSeedCount,
        int showMysteryBoxAtFriendCount,
        DataTableReference friendMysteryBoxStateData,
        int friendBoxState,
        int friendCrateState
    )
    {
        GameObject = gameObject;
        OwnElapsedUpdates = ownElapsedUpdates;
        OwnInterval = ownInterval;
        FriendElapsedUpdates = friendElapsedUpdates;
        FriendInterval = friendInterval;
        SpawnedBoxCount = spawnedBoxCount;
        OwnOpenedBoxTimer = ownOpenedBoxTimer;
        FriendOpenedBoxTimer = friendOpenedBoxTimer;
        RetainedUpdateTick = retainedUpdateTick;
        OwnLastOpened = ownLastOpened;
        FriendLastOpened = friendLastOpened;
        OwnOpenMinimumInterval = ownOpenMinimumInterval;
        OwnOpenMaximumInterval = ownOpenMaximumInterval;
        OwnClosedMinimumInterval = ownClosedMinimumInterval;
        OwnClosedMaximumInterval = ownClosedMaximumInterval;
        FriendOpenMinimumInterval = friendOpenMinimumInterval;
        FriendOpenMaximumInterval = friendOpenMaximumInterval;
        FriendClosedMinimumInterval = friendClosedMinimumInterval;
        FriendClosedMaximumInterval = friendClosedMaximumInterval;
        this._mysteryBoxRandomSeedData = mysteryBoxRandomSeedData;
        MysteryBoxRandomSeedCount = mysteryBoxRandomSeedCount;
        ShowMysteryBoxAtFriendCount = showMysteryBoxAtFriendCount;
        this._friendMysteryBoxStateData = friendMysteryBoxStateData;
        FriendBoxState = friendBoxState;
        FriendCrateState = friendCrateState;
    }

    public GameObjectState GameObject { get; }
    public int OwnElapsedUpdates { get; private set; }
    public int OwnInterval { get; private set; }
    public int FriendElapsedUpdates { get; private set; }
    public int FriendInterval { get; private set; }
    public int SpawnedBoxCount { get; private set; }
    public AttachedTimerState OwnOpenedBoxTimer { get; }
    public AttachedTimerState FriendOpenedBoxTimer { get; }
    public int RetainedUpdateTick { get; private set; }
    public bool OwnLastOpened { get; private set; }
    public bool FriendLastOpened { get; private set; }
    public int OwnOpenMinimumInterval { get; }
    public int OwnOpenMaximumInterval { get; }
    public int OwnClosedMinimumInterval { get; }
    public int OwnClosedMaximumInterval { get; }
    public int FriendOpenMinimumInterval { get; }
    public int FriendOpenMaximumInterval { get; }
    public int FriendClosedMinimumInterval { get; }
    public int FriendClosedMaximumInterval { get; }
    public int MysteryBoxRandomSeedCount { get; private set; }
    public int ShowMysteryBoxAtFriendCount { get; }
    public int FriendBoxState { get; private set; }
    public int FriendCrateState { get; }
    public bool ReplacementEnabled => ShowMysteryBoxAtFriendCount > 0;
    public IntPair? ReconciledPosition { get; private set; }

    public static MysteryBoxSpawnerState Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom,
        InventoryState inventory
    )
    {
        var gameObject = gameObjects.Single(static gameObject => gameObject.Data.TableId is 54);
        var intervals = ResolveIntervals(gameObject, dataTableResolver, constructorRandom);
        var loaded = ResolveLoadedState(gameObject.Snapshot);
        var inventoryState = ResolveInventoryState(dataTableResolver, inventory);

        return new MysteryBoxSpawnerState(
            gameObject,
            loaded.OwnElapsedUpdates,
            intervals.OwnInterval,
            loaded.FriendElapsedUpdates,
            intervals.FriendInterval,
            loaded.SpawnedBoxCount,
            loaded.OwnOpenedBoxTimer,
            loaded.FriendOpenedBoxTimer,
            inventoryState.ShowMysteryBoxAtFriendCount,
            loaded.OwnLastOpened,
            loaded.FriendLastOpened,
            intervals.OwnOpen.Minimum,
            intervals.OwnOpen.Maximum,
            intervals.OwnClosed.Minimum,
            intervals.OwnClosed.Maximum,
            intervals.FriendOpen.Minimum,
            intervals.FriendOpen.Maximum,
            intervals.FriendClosed.Minimum,
            intervals.FriendClosed.Maximum,
            inventoryState.MysteryBoxRandomSeedData,
            inventoryState.MysteryBoxRandomSeedCount,
            inventoryState.ShowMysteryBoxAtFriendCount,
            inventoryState.FriendMysteryBoxStateData,
            inventoryState.FriendBoxState,
            inventoryState.FriendCrateState
        );
    }

    private static (
        (int Minimum, int Maximum) OwnOpen,
        (int Minimum, int Maximum) OwnClosed,
        (int Minimum, int Maximum) FriendOpen,
        (int Minimum, int Maximum) FriendClosed,
        int OwnInterval,
        int FriendInterval
    ) ResolveIntervals(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver,
        GameRandom constructorRandom
    )
    {
        var ownOpen = ResolveIntervalBounds(
            gameObject,
            dataTableResolver,
            "MinSpawnTimeOpened",
            "MaxSpawnTimeOpened"
        );
        var ownClosed = ResolveIntervalBounds(
            gameObject,
            dataTableResolver,
            "MinSpawnTimeClosed",
            "MaxSpawnTimeClosed"
        );
        var friendOpen = ResolveIntervalBounds(
            gameObject,
            dataTableResolver,
            "MinSpawnTimeOpenedFriend",
            "MaxSpawnTimeOpenedFriend"
        );
        var friendClosed = ResolveIntervalBounds(
            gameObject,
            dataTableResolver,
            "MinSpawnTimeClosedFriend",
            "MaxSpawnTimeClosedFriend"
        );
        var ownConfiguration = ResolveSnapshotBoolean(gameObject.Snapshot, "LastOpened");
        var friendConfiguration = ResolveSnapshotBoolean(gameObject.Snapshot, "LastOpenedFriend");
        return (
            ownOpen,
            ownClosed,
            friendOpen,
            friendClosed,
            SelectInterval(ownConfiguration ? ownOpen : ownClosed, constructorRandom),
            SelectInterval(friendConfiguration ? friendOpen : friendClosed, constructorRandom)
        );
    }

    private static (
        int OwnElapsedUpdates,
        int FriendElapsedUpdates,
        int SpawnedBoxCount,
        AttachedTimerState OwnOpenedBoxTimer,
        AttachedTimerState FriendOpenedBoxTimer,
        bool OwnLastOpened,
        bool FriendLastOpened
    ) ResolveLoadedState(GameObjectSnapshot snapshot)
    {
        return (
            ResolveSnapshotInteger(snapshot.Timer, "Timer"),
            ResolveSnapshotInteger(ResolveSnapshotValue(snapshot, "TimerFriend"), "TimerFriend"),
            ResolveSnapshotInteger(
                ResolveSnapshotValue(snapshot, "SpawnedBoxCount"),
                "SpawnedBoxCount"
            ),
            AttachedTimerState.Create(
                active: true,
                ResolveSnapshotValue(snapshot, "OpenedBoxTimer")
            ),
            AttachedTimerState.Create(
                active: true,
                ResolveSnapshotValue(snapshot, "OpenedBoxTimerFriend")
            ),
            ResolveSnapshotBoolean(snapshot, "LastOpened"),
            ResolveSnapshotBoolean(snapshot, "LastOpenedFriend")
        );
    }

    private static (
        DataTableReference MysteryBoxRandomSeedData,
        int MysteryBoxRandomSeedCount,
        int ShowMysteryBoxAtFriendCount,
        DataTableReference FriendMysteryBoxStateData,
        int FriendBoxState,
        int FriendCrateState
    ) ResolveInventoryState(DataTableResolver dataTableResolver, InventoryState inventory)
    {
        var mysteryBoxRandomSeed = ResolveInventoryEntry(
            dataTableResolver,
            inventory,
            "MysteryBoxRandomSeed"
        );
        var showMysteryBox = ResolveInventoryEntry(
            dataTableResolver,
            inventory,
            "ShowMysteryBoxAtFriend"
        );
        var friendBoxState = ResolveInventoryEntry(
            dataTableResolver,
            inventory,
            "FriendMysteryBoxState"
        );

        if (showMysteryBox.Value < 0)
            throw new InvalidDataException("ShowMysteryBoxAtFriend cannot be negative.");

        return (
            mysteryBoxRandomSeed.Data,
            mysteryBoxRandomSeed.Value,
            showMysteryBox.Value,
            friendBoxState.Data,
            friendBoxState.Value,
            ResolveInventoryValue(dataTableResolver, inventory, "FriendMysteryCrateState")
        );
    }

    public void PreUpdate(GameRandom random, InventoryState inventory)
    {
        ArgumentNullException.ThrowIfNull(random);
        ArgumentNullException.ThrowIfNull(inventory);

        MysteryBoxRandomSeedCount = SetInventoryCount(
            inventory,
            _mysteryBoxRandomSeedData,
            MysteryBoxRandomSeedCount,
            desiredCount: 1
        );

        OwnOpenedBoxTimer.Advance(updateCount: 1);
        FriendOpenedBoxTimer.Advance(updateCount: 1);

        if (ShowMysteryBoxAtFriendCount is not 0 and not 1 || FriendCrateState is not 0)
        {
            throw new NotSupportedException(
                "The retained mystery-box pre-update state is not implemented."
            );
        }

        if (ShowMysteryBoxAtFriendCount is 1)
        {
            FriendOpenedBoxTimer.SetStartSeconds(86_400);
            FriendLastOpened = true;
            FriendInterval = SelectInterval(
                (FriendOpenMinimumInterval, FriendOpenMaximumInterval),
                random
            );
            FriendBoxState = SetInventoryCount(
                inventory,
                _friendMysteryBoxStateData,
                FriendBoxState,
                desiredCount: 0
            );
        }

        MysteryBoxRandomSeedCount = SetInventoryCount(
            inventory,
            _mysteryBoxRandomSeedData,
            MysteryBoxRandomSeedCount,
            86_400 - FriendOpenedBoxTimer.GetRemainingSeconds()
        );
    }

    public void NormalUpdate(GameObjectState[] gameObjects)
    {
        ArgumentNullException.ThrowIfNull(gameObjects);

        var loadedBoxCount = gameObjects.Count(static gameObject => gameObject.Data.TableId is 53);

        if (loadedBoxCount is not 1)
        {
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Mystery-box normal update requires one loaded box; found {loadedBoxCount}."
                )
            );
        }

        if (ShowMysteryBoxAtFriendCount > 0)
            return;

        throw new NotSupportedException(
            "Mystery-box friend elapsed threshold processing is not implemented."
        );
    }

    private static int ResolveInventoryValue(
        DataTableResolver dataTableResolver,
        InventoryState inventory,
        string name
    )
    {
        if (!dataTableResolver.TryResolve(GameAssetFiles.Money, name, out var data))
            throw new InvalidDataException($"Unable to resolve {name} from data/money.csv.");

        inventory.TryGetValue(0, data, out var value);
        return value;
    }

    private static (DataTableReference Data, int Value) ResolveInventoryEntry(
        DataTableResolver dataTableResolver,
        InventoryState inventory,
        string name
    )
    {
        if (!dataTableResolver.TryResolve(GameAssetFiles.Money, name, out var data))
            throw new InvalidDataException($"Unable to resolve {name} from data/money.csv.");

        inventory.TryGetValue(0, data, out var value);
        return (data, value);
    }

    private static int SetInventoryCount(
        InventoryState inventory,
        DataTableReference data,
        int currentCount,
        int desiredCount
    )
    {
        inventory.Add(0, data, checked(desiredCount - currentCount));
        return desiredCount;
    }

    public void ReconcileLoadedBox(GameObjectState[] gameObjects, GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(gameObjects);
        ArgumentNullException.ThrowIfNull(random);

        if (_loadedBoxReconciled)
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
            _loadedBoxReconciled = true;
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
            _loadedBoxReconciled = true;
            return;
        }

        throw new NotSupportedException(
            $"Mystery-box reconciliation found no free position in {MaximumPlacementAttempts} attempts."
        );
    }

    private static (int Minimum, int Maximum) ResolveIntervalBounds(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver,
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

        return (minimum, maximum);
    }

    private static int SelectInterval((int Minimum, int Maximum) bounds, GameRandom random)
    {
        return bounds.Minimum + random.NextInt(bounds.Maximum - bounds.Minimum);
    }

    private static JsonElement ResolveSnapshotValue(GameObjectSnapshot snapshot, string name)
    {
        return snapshot.Data.TryGetValue(name, out var value) ? value : default;
    }

    private static int ResolveSnapshotInteger(JsonElement value, string name)
    {
        if (value.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
            return 0;

        return value.TryGetInt32(out var result)
            ? result
            : throw new InvalidDataException($"Mystery-box spawner {name} is not an integer.");
    }

    private static bool ResolveSnapshotBoolean(GameObjectSnapshot snapshot, string name)
    {
        var value = ResolveSnapshotValue(snapshot, name);

        return value.ValueKind switch
        {
            JsonValueKind.Undefined or JsonValueKind.Null => false,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Object
            or JsonValueKind.Array
            or JsonValueKind.String
            or JsonValueKind.Number => throw new InvalidDataException(
                $"Mystery-box spawner {name} is not a Boolean."
            ),
            _ => throw new InvalidDataException($"Mystery-box spawner {name} is not a Boolean."),
        };
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
