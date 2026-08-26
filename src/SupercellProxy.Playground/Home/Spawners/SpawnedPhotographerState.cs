using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class SpawnedPhotographerState
{
    private const int NativeTimerUpdatesPerSecond = 15;
    private const string PhotographerFile = "data/photographer.csv";
    private readonly (
        int MovementSpeed,
        int IdleMinimum,
        int IdleMaximum,
        int PhotoMinimum,
        int PhotoMaximum
    )[] configurations;
    private bool exiting;
    private bool stationaryInitializationPending;

    private SpawnedPhotographerState(
        (
            int MovementSpeed,
            int IdleMinimum,
            int IdleMaximum,
            int PhotoMinimum,
            int PhotoMaximum
        )[] configurations
    )
    {
        this.configurations = configurations;
    }

    public bool Exists { get; private set; }
    public int DataRow { get; private set; } = -1;
    public int TransitUpdatesRemaining { get; private set; }
    public int State { get; private set; }
    public int StateUpdatesRemaining { get; private set; }

    public static SpawnedPhotographerState Create(
        DataTableResolver dataTableResolver,
        PhotographerState[] loadedPhotographers,
        IReadOnlyList<IntPair> entryRoute,
        IReadOnlyList<IntPair> exitRoute
    )
    {
        var resolvedConfigurations = ResolveConfigurations(dataTableResolver);
        var state = new SpawnedPhotographerState(resolvedConfigurations);
        ApplyLoadedPhotographer(
            state,
            loadedPhotographers,
            entryRoute,
            exitRoute,
            resolvedConfigurations
        );
        return state;
    }

    private static (
        int MovementSpeed,
        int IdleMinimum,
        int IdleMaximum,
        int PhotoMinimum,
        int PhotoMaximum
    )[] ResolveConfigurations(DataTableResolver dataTableResolver)
    {
        if (
            !dataTableResolver.TryResolvePhysicalRowCount(PhotographerFile, out var rowCount)
            || rowCount < 1
        )
            throw new InvalidDataException(
                $"{PhotographerFile} contains no photographer configurations."
            );

        var photographerConfigurations = new (
            int MovementSpeed,
            int IdleMinimum,
            int IdleMaximum,
            int PhotoMinimum,
            int PhotoMaximum
        )[rowCount];

        for (var row = 0; row < rowCount; row++)
            photographerConfigurations[row] = ResolveConfiguration(dataTableResolver, row);

        return photographerConfigurations;
    }

    private static (
        int MovementSpeed,
        int IdleMinimum,
        int IdleMaximum,
        int PhotoMinimum,
        int PhotoMaximum
    ) ResolveConfiguration(DataTableResolver dataTableResolver, int row)
    {
        if (
            !dataTableResolver.TryResolveInt(
                PhotographerFile,
                row,
                "WalkSpeed",
                out var movementSpeed
            )
            || !dataTableResolver.TryResolveInt(
                PhotographerFile,
                row,
                "IdleTimeMinMS",
                out var idleMinimum
            )
            || !dataTableResolver.TryResolveInt(
                PhotographerFile,
                row,
                "IdleTimeMaxMS",
                out var idleMaximum
            )
            || !dataTableResolver.TryResolveInt(
                PhotographerFile,
                row,
                "PhotoTimeMinMS",
                out var photoMinimum
            )
            || !dataTableResolver.TryResolveInt(
                PhotographerFile,
                row,
                "PhotoTimeMaxMS",
                out var photoMaximum
            )
            || movementSpeed < 1
            || idleMaximum < idleMinimum
            || photoMaximum < photoMinimum
        )
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Photographer row {row} has invalid native lifecycle data."
                )
            );

        return (movementSpeed, idleMinimum, idleMaximum, photoMinimum, photoMaximum);
    }

    private static void ApplyLoadedPhotographer(
        SpawnedPhotographerState state,
        IReadOnlyList<PhotographerState> loadedPhotographers,
        IReadOnlyList<IntPair> entryRoute,
        IReadOnlyList<IntPair> exitRoute,
        IReadOnlyList<(
            int MovementSpeed,
            int IdleMinimum,
            int IdleMaximum,
            int PhotoMinimum,
            int PhotoMaximum
        )> configurations
    )
    {
        if (loadedPhotographers.Count is 0)
            return;

        if (loadedPhotographers.Count is not 1)
            throw new InvalidDataException(
                "The native photographer spawner has multiple loaded photographers."
            );

        var loaded = loadedPhotographers[0];

        if (
            loaded.GameObject.Data.RowIndex < 0
            || loaded.GameObject.Data.RowIndex >= configurations.Count
        )
            throw new InvalidDataException(
                "The loaded photographer has an invalid native data row."
            );

        state.DataRow = loaded.GameObject.Data.RowIndex;
        state.State = loaded.State;
        state.Exists = true;

        if (loaded.State is 0)
        {
            state.TransitUpdatesRemaining = ResolveTransitUpdatesRemaining(
                loaded,
                entryRoute,
                configurations[state.DataRow].MovementSpeed
            );
            return;
        }

        if (loaded.State is 2)
        {
            state.stationaryInitializationPending = true;
            return;
        }

        if (loaded.State is not 6)
            throw new NotSupportedException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Loaded photographer state {loaded.State} is not implemented."
                )
            );

        state.TransitUpdatesRemaining = checked(
            ResolveTransitUpdatesRemaining(
                loaded,
                exitRoute,
                configurations[state.DataRow].MovementSpeed
            ) + 1
        );
        state.exiting = true;
    }

    private static int ResolveTransitUpdatesRemaining(
        PhotographerState loaded,
        IReadOnlyList<IntPair> route,
        int movementSpeed
    )
    {
        if (loaded.NextPoint < 0 || loaded.NextPoint > route.Count)
            throw new InvalidDataException("The loaded photographer has an invalid route point.");

        var current = new IntPair(loaded.GameObject.PositionX, loaded.GameObject.PositionY);
        var distance = 0;

        for (var pointIndex = loaded.NextPoint; pointIndex < route.Count; pointIndex++)
        {
            distance = checked(
                distance
                + IntegerMath.GetVectorLength(
                    checked(route[pointIndex].First - current.First),
                    checked(route[pointIndex].Second - current.Second)
                )
            );
            current = route[pointIndex];
        }

        return checked((distance + movementSpeed - 1) / movementSpeed);
    }

    public void Spawn(GameRandom random, IReadOnlyList<IntPair> route)
    {
        if (Exists)
            return;

        if (route.Count < 2)
            throw new InvalidDataException(
                "The native photographer spawn route contains fewer than two points."
            );

        DataRow = random.NextInt(configurations.Length);
        var distance = 0;

        for (var i = 1; i < route.Count; i++)
        {
            distance = checked(
                distance
                + IntegerMath.GetVectorLength(
                    checked(route[i].First - route[i - 1].First),
                    checked(route[i].Second - route[i - 1].Second)
                )
            );
        }

        TransitUpdatesRemaining = checked(
            (distance + configurations[DataRow].MovementSpeed - 1)
            / configurations[DataRow].MovementSpeed
        );
        Exists = true;
    }

    public void Update(GameRandom random)
    {
        if (!Exists)
            return;

        if (stationaryInitializationPending)
        {
            stationaryInitializationPending = false;
            BeginStationaryState(random);
            StateUpdatesRemaining--;
            return;
        }

        if (TransitUpdatesRemaining > 0)
        {
            TransitUpdatesRemaining--;

            if (TransitUpdatesRemaining is 0)
            {
                if (exiting)
                {
                    Exists = false;
                    return;
                }

                BeginStationaryState(random);
                StateUpdatesRemaining--;
            }

            return;
        }

        if (StateUpdatesRemaining <= 0)
        {
            throw new NotSupportedException(
                "The spawned photographer's next native state transition is not implemented."
            );
        }

        StateUpdatesRemaining--;
    }

    private void BeginStationaryState(GameRandom random)
    {
        var photograph = random.NextInt(2) is not 0;
        State = photograph ? 4 : 3;
        var configuration = configurations[DataRow];
        var minimum = photograph ? configuration.PhotoMinimum : configuration.IdleMinimum;
        var maximum = photograph ? configuration.PhotoMaximum : configuration.IdleMaximum;
        var milliseconds = checked(minimum + random.NextInt(maximum - minimum));
        StateUpdatesRemaining = checked(milliseconds * NativeTimerUpdatesPerSecond / 1000);

        if (StateUpdatesRemaining < 1)
            throw new InvalidDataException(
                "The photographer's native stationary duration is empty."
            );
    }
}
