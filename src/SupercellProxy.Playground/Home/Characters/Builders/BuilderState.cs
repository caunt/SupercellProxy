using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class BuilderState
{
    private const string BuildersFile = "data/builders.csv";
    private const int UpdateMilliseconds = 33;

    private readonly BuilderConfiguration[] configurations;

    private BuilderState(BuilderConfiguration[] configurations)
    {
        this.configurations = configurations;
    }

    public bool Exists { get; private set; }
    public bool Initialized { get; private set; }
    public int DataRow { get; private set; } = -1;
    public int StateMilliseconds { get; private set; }
    public int TransitUpdatesRemaining { get; private set; }

    public static BuilderState Create(DataTableResolver dataTableResolver)
    {
        if (
            !dataTableResolver.TryResolvePhysicalRowCount(BuildersFile, out var rowCount)
            || rowCount < 1
        )
            throw new InvalidDataException($"{BuildersFile} contains no builder configurations.");

        var resolvedConfigurations = new BuilderConfiguration[rowCount];

        for (var row = 0; row < rowCount; row++)
        {
            if (
                !dataTableResolver.TryResolveInt(
                    BuildersFile,
                    row,
                    "WalkSpeed",
                    out var movementSpeed
                )
                || !dataTableResolver.TryResolveInt(
                    BuildersFile,
                    row,
                    "IdleTimeMinMS",
                    out var idleMinimum
                )
                || !dataTableResolver.TryResolveInt(
                    BuildersFile,
                    row,
                    "IdleTimeMaxMS",
                    out var idleMaximum
                )
                || movementSpeed < 1
                || idleMaximum < idleMinimum
            )
            {
                throw new InvalidDataException(
                    string.Create(
                        CultureInfo.InvariantCulture,
                        $"Builder row {row} has an invalid native idle-time range."
                    )
                );
            }

            resolvedConfigurations[row] = new BuilderConfiguration(
                movementSpeed,
                idleMinimum,
                idleMaximum
            );
        }

        return new BuilderState(resolvedConfigurations);
    }

    public void Spawn(GameRandom random, IReadOnlyList<IntPair> route)
    {
        if (Exists)
            return;

        if (route.Count < 2)
            throw new InvalidDataException(
                "The native builder spawn route contains fewer than two points."
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

        if (!Initialized)
        {
            TransitUpdatesRemaining--;

            if (TransitUpdatesRemaining is 0)
            {
                var configuration = configurations[DataRow];
                StateMilliseconds = checked(
                    configuration.IdleMinimumMilliseconds
                    + random.NextInt(
                        configuration.IdleMaximumMilliseconds
                            - configuration.IdleMinimumMilliseconds
                    )
                );
                Initialized = true;
            }

            return;
        }

        StateMilliseconds = checked(StateMilliseconds - UpdateMilliseconds);

        if (StateMilliseconds <= 0)
            throw new NotSupportedException(
                "The spawned builder's next native state transition is not implemented."
            );
    }
}
