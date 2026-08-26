using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class MapGameManagerState
{
    private bool postLoadSetupPending;

    private MapGameManagerState(bool postLoadSetupPending)
    {
        this.postLoadSetupPending = postLoadSetupPending;
    }

    public static MapGameManagerState Create(MapGameSnapshot? snapshot)
    {
        if (snapshot is null || snapshot.Event <= 0 || snapshot.MapGlobalId <= 0)
            return new MapGameManagerState(postLoadSetupPending: false);

        if (
            snapshot.QuestrManager.CurrentQuests.ValueKind
            is not System.Text.Json.JsonValueKind.Object
        )
        {
            throw new InvalidDataException("Active map-game quest state is not an object.");
        }

        return new MapGameManagerState(
            snapshot.QuestrManager.CurrentQuests.EnumerateObject().Any()
        );
    }

    public void CompletePostLoadSetup(GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        if (!postLoadSetupPending)
            return;

        _ = random.NextInt(100);
        postLoadSetupPending = false;
    }
}
