using SupercellProxy.Playground.Data.Assets;
using SupercellProxy.Playground.Data.Tables;

namespace SupercellProxy.Playground.Home;

internal sealed class PostmanState
{
    private PostmanState(GameObjectState gameObject)
    {
        GameObject = gameObject;
        State = gameObject.Snapshot.State;
        Timer = TimerSnapshot.Decode(gameObject.Snapshot.Timer);
    }

    public GameObjectState GameObject { get; }
    public int State { get; private set; }
    public TimerSnapshot Timer { get; }
    public int RouteIndex { get; private set; }

    public static PostmanState Resolve(
        GameObjectState[] gameObjects,
        DataTableResolver dataTableResolver
    )
    {
        if (!dataTableResolver.TryGetTableId(GameAssetFiles.Postman, out var postmanTableId))
            throw new InvalidOperationException(
                $"{GameAssetFiles.Postman} is not registered as a native data table."
            );

        var postman =
            gameObjects.SingleOrDefault(gameObject => gameObject.Data.TableId == postmanTableId)
            ?? throw new InvalidDataException("The authoritative home state contains no postman.");

        return new PostmanState(postman);
    }

    public void ApplyStateCommand()
    {
        RouteIndex = 0;
        State = 2;
    }
}
