namespace SupercellProxy.Playground.Home;

internal sealed class NeighborhoodObjectManagerState
{
    private NeighborhoodObjectManagerState(NeighborhoodObjectManagerSnapshot? snapshot)
    {
        Snapshot = snapshot;
    }

    public NeighborhoodObjectManagerSnapshot? Snapshot { get; }

    public static NeighborhoodObjectManagerState Create(NeighborhoodObjectManagerSnapshot? snapshot)
    {
        return new NeighborhoodObjectManagerState(snapshot);
    }
}
