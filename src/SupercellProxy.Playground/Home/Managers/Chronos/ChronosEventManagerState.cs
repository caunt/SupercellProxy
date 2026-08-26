using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class ChronosEventManagerState
{
    private const int WheelConfigurationEventType = 14;
    private readonly bool reconcileInitialEventObject;
    private readonly int minimumX;
    private readonly int minimumY;
    private readonly int rangeX;
    private readonly int rangeY;
    private bool initialEventObjectReconciled;

    private ChronosEventManagerState(
        bool reconcileInitialEventObject,
        int minimumX,
        int minimumY,
        int rangeX,
        int rangeY
    )
    {
        this.reconcileInitialEventObject = reconcileInitialEventObject;
        this.minimumX = minimumX;
        this.minimumY = minimumY;
        this.rangeX = rangeX;
        this.rangeY = rangeY;
    }

    public IntPair? InitialEventObjectPosition { get; private set; }

    public static ChronosEventManagerState Create(
        ChronosEventsSnapshot? snapshot,
        DataTableResolver dataTableResolver,
        int serverTimestamp
    )
    {
        if (snapshot is null)
            throw new InvalidDataException("The saved state has no Chronos event manager.");

        var activeWheelEvents = snapshot.EventBoardState.Events.Count(eventState =>
            eventState.Type is WheelConfigurationEventType
            && eventState.StartTime <= serverTimestamp
            && serverTimestamp < eventState.EndTime
        );

        if (activeWheelEvents > 1)
            throw new NotSupportedException(
                "Multiple active wheel-configuration events are not implemented."
            );

        const string gameConfigFile = "data/game_config.csv";

        if (
            !dataTableResolver.TryResolveInt(
                gameConfigFile,
                "DailyCreatureSpawnMinX",
                "IntValue",
                out var minimumX
            )
            || !dataTableResolver.TryResolveInt(
                gameConfigFile,
                "DailyCreatureSpawnMinY",
                "IntValue",
                out var minimumY
            )
            || !dataTableResolver.TryResolveInt(
                gameConfigFile,
                "DailyCreatureSpawnMaxX",
                "IntValue",
                out var maximumX
            )
            || !dataTableResolver.TryResolveInt(
                gameConfigFile,
                "DailyCreatureSpawnMaxY",
                "IntValue",
                out var maximumY
            )
            || maximumX <= minimumX
            || maximumY <= minimumY
        )
        {
            throw new InvalidDataException("The retained event-object placement range is invalid.");
        }

        return new ChronosEventManagerState(
            activeWheelEvents is 1,
            minimumX,
            minimumY,
            maximumX - minimumX,
            maximumY - minimumY
        );
    }

    public void ReconcileInitialHomeObjects(GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        if (initialEventObjectReconciled)
            throw new InvalidOperationException(
                "Initial Chronos event objects have already been reconciled."
            );

        initialEventObjectReconciled = true;

        if (!reconcileInitialEventObject)
            return;

        InitialEventObjectPosition = new IntPair(
            minimumX + random.NextInt(rangeX),
            minimumY + random.NextInt(rangeY)
        );
    }
}
