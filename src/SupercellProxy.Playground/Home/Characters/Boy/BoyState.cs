using System.Globalization;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed record BoyState(
    GameObjectState GameObject,
    int State,
    IntPair ChecksumPair0,
    IntPair ChecksumPair1,
    TimerSnapshot HireTimer,
    TimerSnapshot CooldownTimer,
    TimerSnapshot OfferTimer,
    TimerSnapshot IntervalOfferTimer,
    bool ChecksumFlag0,
    bool FreeReEngagementAvailable,
    bool HireEnded,
    bool ChecksumFlag1,
    bool IntervalOfferActive,
    bool ChecksumFlag2,
    int ChecksumState0
)
{
    public static BoyState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        const string boyFile = "data/boy.csv";

        if (!dataTableResolver.TryGetTableId(boyFile, out var boyTableId))
            throw new InvalidOperationException(
                $"{boyFile} is not registered as a native data table."
            );

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == boyTableId)
            .Select(gameObject => Create(gameObject, dataTableResolver))
            .ToArray();
    }

    private static BoyState Create(GameObjectState gameObject, DataTableResolver dataTableResolver)
    {
        var snapshot = gameObject.Snapshot;
        var point = ResolveIdlePoint(gameObject, dataTableResolver);
        gameObject.MoveTo(point.First, point.Second);

        return new BoyState(
            gameObject,
            snapshot.State,
            point,
            point,
            TimerSnapshot.Decode(snapshot.HireTimer),
            TimerSnapshot.Decode(snapshot.CooldownTimer),
            TimerSnapshot.Decode(snapshot.OfferTimer),
            TimerSnapshot.Decode(snapshot.IntervalOfferTimer),
            ChecksumFlag0: false,
            snapshot.FreeReEngagementAvailable,
            snapshot.HireEnded,
            ChecksumFlag1: false,
            snapshot.IntervalOfferActive,
            ChecksumFlag2: false,
            0
        );
    }

    private static IntPair ResolveIdlePoint(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver
    )
    {
        var snapshot = gameObject.Snapshot;

        if (snapshot.State is not 3)
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Boy {gameObject.GlobalId} has state {snapshot.State}; only the native state 3 position mapping is implemented."
                )
            );
        }

        if (snapshot.X is not int x || snapshot.Y is not int y)
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Boy {gameObject.GlobalId} has no saved position."
                )
            );

        if (
            !dataTableResolver.TryResolveInt(gameObject.Data.GlobalId, "PosIdleX", out var idleX)
            || !dataTableResolver.TryResolveInt(gameObject.Data.GlobalId, "PosIdleY", out var idleY)
        )
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Boy {gameObject.GlobalId} has no native idle position."
                )
            );
        }

        if (x != idleX || y != idleY)
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Boy {gameObject.GlobalId} state 3 position ({x}, {y}) does not match its native idle position ({idleX}, {idleY})."
                )
            );
        }

        return new IntPair(ToPosition(idleX), ToPosition(idleY));
    }

    private static int ToPosition(int tilePosition)
    {
        return checked(tilePosition * 0x200 + 0x100);
    }
}
