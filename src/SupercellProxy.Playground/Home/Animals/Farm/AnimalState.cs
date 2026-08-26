using System.Globalization;
using System.Text.Json;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class AnimalState
{
    private const int DisabledRandomSentinel = -66666666;
    private const int UpdateMilliseconds = 33;
    private const int TargetAttemptCount = 15;

    private readonly AnimalHabitatGrid habitatGrid;
    private readonly int idleMinimum;
    private readonly int idleMaximum;
    private readonly int movementSpeed;
    private readonly bool passable;
    private bool moving;
    private bool postLoadSetupCompleted;
    private int targetX;
    private int targetY;
    private int stateMilliseconds;

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
        this.habitatGrid = habitatGrid;
        this.idleMinimum = idleMinimum;
        this.idleMaximum = idleMaximum;
        this.movementSpeed = movementSpeed;
        this.passable = passable;
        stateMilliseconds = unchecked(idleMinimum + DisabledRandomSentinel);
    }

    public GameObjectState GameObject { get; }

    public static AnimalState[] Resolve(
        GameObjectState[] gameObjects,
        AnimalHabitatState[] animalHabitats,
        AnimalHabitatPieceState[] animalHabitatPieces,
        DataTableResolver dataTableResolver
    )
    {
        const string animalsFile = "data/animals.csv";

        if (!dataTableResolver.TryGetTableId(animalsFile, out var animalTableId))
            throw new InvalidOperationException(
                $"{animalsFile} is not registered as a native data table."
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
        IReadOnlyDictionary<int, AnimalHabitatGrid> grids,
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
        if (postLoadSetupCompleted)
            return;

        postLoadSetupCompleted = true;
    }

    private void Update(GameRandom random)
    {
        if (!postLoadSetupCompleted)
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

        if (moving)
            MoveTowardsTarget(random);

        stateMilliseconds -= UpdateMilliseconds;
    }

    private void BeginMovement(GameRandom random)
    {
        habitatGrid.Release(GameObject);

        AnimalHabitatGridCell? target = null;

        for (var attempt = 0; attempt < TargetAttemptCount; attempt++)
        {
            target =
                habitatGrid.Select(random, requireUnoccupied: true)
                ?? habitatGrid.Select(random, requireUnoccupied: false);

            if (target is null)
                break;

            if (
                attempt == TargetAttemptCount - 1
                || habitatGrid.HasWalkablePath(
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
            (targetX, targetY) = ResolveTargetPosition(target, random);
            habitatGrid.Occupy(target.X, target.Y, GameObject, passable);
            moving = true;
        }

        stateMilliseconds = idleMinimum + random.NextInt(idleMaximum - idleMinimum);
    }

    private (int X, int Y) ResolveTargetPosition(AnimalHabitatGridCell target, GameRandom random)
    {
        var x = (target.X << 9) | 0x100;
        var y = (target.Y << 9) | 0x100;

        if (target.X is 0)
            x = 0x166;
        else if (target.X == habitatGrid.Width - 1)
            x = (target.X << 9) | 0x99;

        if (target.Y is 0)
            y = 0x166;
        else if (target.Y == habitatGrid.Height - 1)
            y = (target.Y << 9) | 0x99;

        if (
            target.X is not 0
            && target.Y is not 0
            && target.X != habitatGrid.Width - 1
            && target.Y != habitatGrid.Height - 1
        )
        {
            x = target.X * 0x200 + (random.NextInt(7) * 0x200 + 0x400) / 10;
            y = target.Y * 0x200 + (random.NextInt(7) * 0x200 + 0x400) / 10;
        }

        return (x, y);
    }

    private void MoveTowardsTarget(GameRandom random)
    {
        var currentX = GameObject.PositionX;
        var currentY = GameObject.PositionY;
        var deltaX = targetX - currentX;
        var deltaY = targetY - currentY;
        var squaredDistance = unchecked(deltaX * deltaX + deltaY * deltaY);
        var distance = IntegerMath.GetSquareRoot(squaredDistance);
        var scaledDistance = distance * 10;
        var scaledSpeed = movementSpeed * 2;

        if (movementSpeed < 1 || scaledDistance <= scaledSpeed)
        {
            GameObject.MoveTo(targetX, targetY);
            moving = false;
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
