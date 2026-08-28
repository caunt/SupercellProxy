using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;
using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed record BalloonState(GameObjectState GameObject, int Heading)
{
    public static BalloonState[] Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver,
        GameRandom random
    )
    {
        if (!dataTableResolver.TryGetTableId(GameAssetFiles.Balloons, out var tableId))
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
