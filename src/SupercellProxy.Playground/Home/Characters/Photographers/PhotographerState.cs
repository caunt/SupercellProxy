using System.Globalization;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed class PhotographerState
{
    private PhotographerState(
        GameObjectState gameObject,
        PhotographerStateCode state,
        int nextPoint,
        int pathPointCapacity
    )
    {
        GameObject = gameObject;
        State = state;
        NextPoint = nextPoint;
        Path = new PhotographerPathState(pathPointCapacity);
    }

    public GameObjectState GameObject { get; }
    public PhotographerStateCode State { get; internal set; }
    public int StateTimer { get; internal set; }
    public int NextPoint { get; internal set; }
    public int RuntimeStateA { get; internal set; } = -1;
    public int RuntimeStateB { get; internal set; } = -1;
    public bool PathComplete { get; internal set; }
    public bool LifecycleEnabled { get; internal set; }
    public IntPair MovementVector { get; internal set; }
    public IntPair CandidateMovementVector { get; internal set; }
    public IntPair PersistentMovementVector { get; internal set; }
    public static bool HasManager => true;
    public IntPair[]? EntryRoute { get; internal set; }
    public IntPair[]? ExitRoute { get; internal set; }
    public PhotographerPathState Path { get; }
    public int FacingRefreshTimer { get; internal set; }
    public int LifecycleTimer { get; internal set; }
    public int PendingTargetX { get; internal set; }
    public int PendingTargetY { get; internal set; }

    public static PhotographerState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        if (
            !dataTableResolver.TryGetTableId(
                GameAssetFiles.Photographer,
                out var photographerTableId
            )
        )
            throw new InvalidOperationException(
                $"{GameAssetFiles.Photographer} is not registered as a native data table."
            );

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == photographerTableId)
            .Select(gameObject => CreateInitial(gameObject, dataTableResolver))
            .ToArray();
    }

    private static PhotographerState CreateInitial(
        GameObjectState gameObject,
        DataTableResolver dataTableResolver
    )
    {
        var snapshot = gameObject.Snapshot;
        var stateDefined = Enum.IsDefined(typeof(PhotographerStateCode), snapshot.State);

        if (!stateDefined || snapshot.NextPoint is < 0 or > 1 || snapshot.LinkedGlobalId is not 0)
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Photographer {gameObject.GlobalId} has unsupported initial state: State={snapshot.State}, NextPoint={snapshot.NextPoint}, LinkedGlobalId={snapshot.LinkedGlobalId}."
                )
            );
        }

        if (
            !dataTableResolver.TryResolveInt(
                gameObject.Data.GlobalId,
                "MaxPathLength",
                out var pathPointCapacity
            )
            || pathPointCapacity < 1
        )
        {
            throw new InvalidDataException(
                $"Photographer {gameObject.Data.Name} has no valid MaxPathLength value."
            );
        }

        return new PhotographerState(
            gameObject,
            Enum.GetValues<PhotographerStateCode>()
                .Single(state =>
                    Convert.ToInt32(state, CultureInfo.InvariantCulture) == snapshot.State
                ),
            snapshot.NextPoint,
            pathPointCapacity
        );
    }
}
