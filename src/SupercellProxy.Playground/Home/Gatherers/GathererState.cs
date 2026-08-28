using System.Globalization;
using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed record GathererState(
    GameObjectState GameObject,
    int GathererNestIndex,
    int GathererMineIndex,
    int AiState,
    int ChecksumState0,
    int TargetX,
    int TargetY,
    bool ChecksumFlag0,
    bool ChecksumFlag1,
    TimerSnapshot Timer,
    PathState Path
)
{
    public static GathererState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        if (!dataTableResolver.TryGetTableId(GameAssetFiles.Gatherers, out var gathererTableId))
            throw new InvalidOperationException(
                $"{GameAssetFiles.Gatherers} is not registered as a native data table."
            );

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == gathererTableId)
            .Select(CreateIdle)
            .ToArray();
    }

    private static GathererState CreateIdle(GameObjectState gameObject)
    {
        var snapshot = gameObject.Snapshot;
        var timer = TimerSnapshot.Decode(snapshot.Timer);

        if (
            snapshot.GathererAiState is not 1
            || snapshot.CarryingResources
            || snapshot.TravelTime is not 0
            || snapshot.TargetX is not 0
            || snapshot.TargetY is not 0
            || timer != default
        )
        {
            throw new InvalidDataException(
                string.Create(
                    CultureInfo.InvariantCulture,
                    $"Gatherer {gameObject.GlobalId} is active; only the native idle gatherer state is implemented."
                )
            );
        }

        return new GathererState(
            gameObject,
            snapshot.GathererNestIndex,
            snapshot.GathererMineIndex,
            snapshot.GathererAiState,
            0,
            snapshot.TargetX,
            snapshot.TargetY,
            ChecksumFlag0: false,
            ChecksumFlag1: false,
            timer,
            PathState.CreateIdle(128)
        );
    }
}
