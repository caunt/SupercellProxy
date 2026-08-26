using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed record BalloonState(GameObjectState GameObject, int Heading)
{
    private const string BalloonsFile = "data/balloons.csv";

    public static BalloonState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver,
        GameRandom random
    )
    {
        if (!dataTableResolver.TryGetTableId(BalloonsFile, out var tableId))
            return [];

        return gameObjects
            .Where(gameObject => gameObject.Data.TableId == tableId)
            .Select(gameObject => new BalloonState(
                gameObject,
                unchecked(random.NextInt(180) * 8 + 0x438)
            ))
            .ToArray();
    }
}
