using SupercellProxy.Playground.Logic;

namespace SupercellProxy.Playground.Home;

internal sealed class MapGameManagerState
{
    private bool _postLoadSetupPending;

    private MapGameManagerState(bool postLoadSetupPending)
    {
        this._postLoadSetupPending = postLoadSetupPending;
    }

    public static MapGameManagerState Create(MapGameSnapshot? snapshot)
    {
        if (
            snapshot is null
            || snapshot.Manager.ValueKind is not System.Text.Json.JsonValueKind.Object
            || !snapshot.Manager.EnumerateObject().Any()
            || snapshot.MapGlobalId <= 0
        )
            return new MapGameManagerState(postLoadSetupPending: false);

        if (
            snapshot.QuestrManager.CurrentQuests.ValueKind
            is not System.Text.Json.JsonValueKind.Object
        )
        {
            throw new InvalidDataException("Active map-game quest state is not an object.");
        }

        if (snapshot.QuestrManager.LastChickenDayIndex is not -1)
            throw new NotSupportedException(
                "Map-game daily chicken selection outside the unset-day gate is not implemented."
            );

        return new MapGameManagerState(postLoadSetupPending: false);
    }

    public void CompletePostLoadSetup(GameRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        if (!_postLoadSetupPending)
            return;

        _ = random.NextInt(100);
        _postLoadSetupPending = false;
    }
}
