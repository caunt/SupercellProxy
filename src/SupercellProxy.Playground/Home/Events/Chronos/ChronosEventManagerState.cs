using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class ChronosEventManagerState
{
    private const int WheelConfigurationEventType = 14;
    private readonly bool _reconcileInitialEventObject;
    private readonly int _minimumX;
    private readonly int _minimumY;
    private readonly int _rangeX;
    private readonly int _rangeY;
    private readonly EventBoardEventState[] _eventBoardEvents;
    private readonly int _seenEventCashReward;
    private bool _initialEventObjectReconciled;

    private ChronosEventManagerState(
        bool reconcileInitialEventObject,
        int minimumX,
        int minimumY,
        int rangeX,
        int rangeY,
        EventBoardEventState[] eventBoardEvents,
        int seenEventCashReward
    )
    {
        this._reconcileInitialEventObject = reconcileInitialEventObject;
        this._minimumX = minimumX;
        this._minimumY = minimumY;
        this._rangeX = rangeX;
        this._rangeY = rangeY;
        this._eventBoardEvents = eventBoardEvents;
        this._seenEventCashReward = seenEventCashReward;
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

        var (minimumX, minimumY, maximumX, maximumY, seenEventCashReward) = ResolveConfiguration(
            dataTableResolver
        );

        return new ChronosEventManagerState(
            activeWheelEvents is 1,
            minimumX,
            minimumY,
            maximumX - minimumX,
            maximumY - minimumY,
            snapshot
                .EventBoardState.Events.Select(eventState => new EventBoardEventState(
                    eventState.EventId,
                    eventState.VariantId,
                    eventState.SeenInEventBoard,
                    eventState.StartTime <= serverTimestamp && serverTimestamp < eventState.EndTime
                ))
                .ToArray(),
            seenEventCashReward
        );
    }

    private static (
        int MinimumX,
        int MinimumY,
        int MaximumX,
        int MaximumY,
        int SeenEventCashReward
    ) ResolveConfiguration(DataTableResolver dataTableResolver)
    {
        if (
            !dataTableResolver.TryResolveInt(
                GameAssetFiles.GameConfig,
                "DailyCreatureSpawnMinX",
                "IntValue",
                out var minimumX
            )
            || !dataTableResolver.TryResolveInt(
                GameAssetFiles.GameConfig,
                "DailyCreatureSpawnMinY",
                "IntValue",
                out var minimumY
            )
            || !dataTableResolver.TryResolveInt(
                GameAssetFiles.GameConfig,
                "DailyCreatureSpawnMaxX",
                "IntValue",
                out var maximumX
            )
            || !dataTableResolver.TryResolveInt(
                GameAssetFiles.GameConfig,
                "DailyCreatureSpawnMaxY",
                "IntValue",
                out var maximumY
            )
            || maximumX <= minimumX
            || maximumY <= minimumY
            || !dataTableResolver.TryResolveInt(
                GameAssetFiles.GameConfig,
                "NewEventBoardEventSeenCashReward",
                "IntValue",
                out var seenEventCashReward
            )
        )
        {
            throw new InvalidDataException("The retained event configuration is invalid.");
        }

        return (minimumX, minimumY, maximumX, maximumY, seenEventCashReward);
    }

    public bool TryMarkEventSeen(int eventId, out int variantId, out int cashReward)
    {
        var eventState = _eventBoardEvents.FirstOrDefault(eventState =>
            eventState.EventId == eventId && eventState.HasLinkedEventState
        );
        if (eventState is null || !eventState.TryMarkSeen())
        {
            variantId = default;
            cashReward = default;
            return false;
        }

        variantId = eventState.VariantId;
        cashReward = _seenEventCashReward;
        return true;
    }

    public void ReconcileInitialHomeObjects(GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        if (_initialEventObjectReconciled)
            throw new InvalidOperationException(
                "Initial Chronos event objects have already been reconciled."
            );

        _initialEventObjectReconciled = true;

        if (!_reconcileInitialEventObject)
            return;

        InitialEventObjectPosition = new IntPair(
            _minimumX + random.NextInt(_rangeX),
            _minimumY + random.NextInt(_rangeY)
        );
    }
}
