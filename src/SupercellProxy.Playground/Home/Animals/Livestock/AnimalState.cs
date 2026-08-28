using System.Globalization;
using System.Text.Json;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class AnimalState
{
    private const int DisabledRandomSentinel = -66666666;
    private const int TargetAttemptCount = 15;

    private readonly AnimalHabitatGrid _habitatGrid;
    private readonly int _idleMinimum;
    private readonly int _idleMaximum;
    private readonly int _movementSpeed;
    private readonly bool _passable;
    private bool _moving;
    private bool _postLoadSetupCompleted;
    private int _targetX;
    private int _targetY;
    private int _stateMilliseconds;

    private AnimalState(
        GameObjectState gameObject,
        AnimalHabitatGrid habitatGrid,
        int idleMinimum,
        int idleMaximum,
        int movementSpeed,
        bool passable
    )
    {
        GameObject = gameObject;
        this._habitatGrid = habitatGrid;
        this._idleMinimum = idleMinimum;
        this._idleMaximum = idleMaximum;
        this._movementSpeed = movementSpeed;
        this._passable = passable;
        _stateMilliseconds = unchecked(idleMinimum + DisabledRandomSentinel);
    }

    public GameObjectState GameObject { get; }
    public int StateMilliseconds => _stateMilliseconds;

    public static AnimalState[] Resolve(
        GameObjectState[] gameObjects,
        AnimalHabitatState[] animalHabitats,
        AnimalHabitatPieceState[] animalHabitatPieces,
        DataTableResolver dataTableResolver
    )
    {
        if (!dataTableResolver.TryGetTableId(GameAssetFiles.Animals, out var animalTableId))
            throw new InvalidOperationException(
                $"{GameAssetFiles.Animals} is not registered as a native data table."
            );

        var grids = animalHabitats.ToDictionary(
            static habitat => habitat.GameObject.GlobalId,
            habitat => AnimalHabitatGrid.Create(habitat, animalHabitatPieces, dataTableResolver)
        );
        var animals = gameObjects
            .Where(gameObject => gameObject.Data.TableId == animalTableId)
            .ToArray();

        var states = new AnimalState[animals.Length];

        for (var i = 0; i < animals.Length; i++)
            states[i] = CreateState(animals[i], grids, dataTableResolver);

        return states;
    }

    private static AnimalState CreateState(
        GameObjectState animal,
        Dictionary<int, AnimalHabitatGrid> grids,
        DataTableResolver dataTableResolver
    )
    {
        var parent =
            animal.Parent
            ?? throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Animal {animal.GlobalId} has no resolved habitat."
                )
            );
        if (!grids.TryGetValue(parent.GlobalId, out var grid))
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Animal {animal.GlobalId} references unknown habitat {parent.GlobalId}."
                )
            );

        var (idleMinimum, idleMaximum, movementSpeed, passable) = ResolveMovementData(
            animal,
            dataTableResolver
        );
        ValidateLoadedState(animal);
        grid.Occupy(animal.PositionX >> 9, animal.PositionY >> 9, animal, passable);
        return new AnimalState(animal, grid, idleMinimum, idleMaximum, movementSpeed, passable);
    }

    private static (
        int IdleMinimum,
        int IdleMaximum,
        int MovementSpeed,
        bool Passable
    ) ResolveMovementData(GameObjectState animal, DataTableResolver dataTableResolver)
    {
        if (
            !dataTableResolver.TryResolveInt(
                animal.Data.GlobalId,
                "StateIdleMinMS",
                out var idleMinimum
            )
            || !dataTableResolver.TryResolveInt(
                animal.Data.GlobalId,
                "StateIdleMaxMS",
                out var idleMaximum
            )
            || !dataTableResolver.TryResolveInt(
                animal.Data.GlobalId,
                "MovementSpeed",
                out var movementSpeed
            )
            || !dataTableResolver.TryResolveBoolean(
                animal.Data.GlobalId,
                "Passable",
                out var passable
            )
            || idleMaximum < idleMinimum
            || movementSpeed < 0
        )
            throw new InvalidDataException(
                $"Animal {animal.Data.Name} has incomplete native movement data."
            );
        return (idleMinimum, idleMaximum, movementSpeed, passable);
    }

    private static void ValidateLoadedState(GameObjectState animal)
    {
        if (
            !animal.Snapshot.Data.TryGetValue("Fed", out var fed)
            || fed.ValueKind is not JsonValueKind.True
            || animal.Snapshot.Timer.ValueKind
                is not JsonValueKind.Undefined
                    and not JsonValueKind.Null
        )
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Animal {animal.GlobalId} is not in the supported native fed, timer-free state."
                )
            );
    }

    public static void Update(AnimalState[] animals, GameRandom random)
    {
        foreach (var animal in animals)
            animal.Update(random);
    }

    public static void CompletePostLoadSetup(AnimalState[] animals, GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(animals);
        ArgumentNullException.ThrowIfNull(random);

        foreach (var animal in animals)
            animal.CompletePostLoadSetup(random);
    }

    private void CompletePostLoadSetup(GameRandom random)
    {
        if (_postLoadSetupCompleted)
            return;

        _postLoadSetupCompleted = true;
    }

    private void Update(GameRandom random)
    {
        if (!_postLoadSetupCompleted)
            throw new InvalidOperationException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Animal {GameObject.GlobalId} has not completed native post-load setup."
                )
            );

        if (
            !GameObject.Snapshot.Data.TryGetValue("Fed", out var fed)
            || fed.ValueKind is not JsonValueKind.True
            || GameObject.Snapshot.Timer.ValueKind
                is not JsonValueKind.Undefined
                    and not JsonValueKind.Null
        )
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Animal {GameObject.GlobalId} left the supported native fed, timer-free state."
                )
            );
        }

        if (_moving)
            MoveTowardsTarget(random);

        _stateMilliseconds -= GameTick.UpdateMilliseconds;
    }

    private void BeginMovement(GameRandom random)
    {
        _habitatGrid.Release(GameObject);

        AnimalHabitatGridCell? target = null;

        for (var attempt = 0; attempt < TargetAttemptCount; attempt++)
        {
            target =
                _habitatGrid.Select(random, requireUnoccupied: true)
                ?? _habitatGrid.Select(random, requireUnoccupied: false);

            if (target is null)
                break;

            if (
                attempt == TargetAttemptCount - 1
                || _habitatGrid.HasWalkablePath(
                    GameObject.PositionX >> 9,
                    GameObject.PositionY >> 9,
                    target.X,
                    target.Y
                )
            )
            {
                break;
            }
        }

        if (target is not null)
        {
            (_targetX, _targetY) = ResolveTargetPosition(target, random);
            _habitatGrid.Occupy(target.X, target.Y, GameObject, _passable);
            _moving = true;
        }

        _stateMilliseconds = _idleMinimum + random.NextInt(_idleMaximum - _idleMinimum);
    }

    private (int X, int Y) ResolveTargetPosition(AnimalHabitatGridCell target, GameRandom random)
    {
        var x = target.X * GameObjectState.TileSize | GameObjectState.TileCenter;
        var y = target.Y * GameObjectState.TileSize | GameObjectState.TileCenter;

        if (target.X is 0)
            x = 0x166;
        else if (target.X == _habitatGrid.Width - 1)
            x = (target.X << 9) | 0x99;

        if (target.Y is 0)
            y = 0x166;
        else if (target.Y == _habitatGrid.Height - 1)
            y = (target.Y << 9) | 0x99;

        if (
            target.X is not 0
            && target.Y is not 0
            && target.X != _habitatGrid.Width - 1
            && target.Y != _habitatGrid.Height - 1
        )
        {
            x =
                target.X * GameObjectState.TileSize
                + (random.NextInt(7) * GameObjectState.TileSize + 4 * GameObjectState.TileCenter)
                    / 10;
            y =
                target.Y * GameObjectState.TileSize
                + (random.NextInt(7) * GameObjectState.TileSize + 4 * GameObjectState.TileCenter)
                    / 10;
        }

        return (x, y);
    }

    private void MoveTowardsTarget(GameRandom random)
    {
        var currentX = GameObject.PositionX;
        var currentY = GameObject.PositionY;
        var deltaX = _targetX - currentX;
        var deltaY = _targetY - currentY;
        var squaredDistance = unchecked(deltaX * deltaX + deltaY * deltaY);
        var distance = IntegerMath.GetSquareRoot(squaredDistance);
        var scaledDistance = distance * 10;
        var scaledSpeed = _movementSpeed * 2;

        if (_movementSpeed < 1 || scaledDistance <= scaledSpeed)
        {
            GameObject.MoveTo(_targetX, _targetY);
            _moving = false;
            BeginMovement(random);
            return;
        }

        var stepX = scaledSpeed * deltaX / scaledDistance;
        var stepY = scaledSpeed * deltaY / scaledDistance;

        if (stepX != stepY)
            GameObject.SetMirrored(stepX < stepY);

        GameObject.MoveTo(currentX + stepX, currentY + stepY);
    }
}
