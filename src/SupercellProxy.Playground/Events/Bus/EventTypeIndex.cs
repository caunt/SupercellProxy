namespace SupercellProxy.Playground.Events.Bus;

public static class EventTypeIndex
{
    private static int _currentIndex = -1;

    public static int NextIndex => Interlocked.Increment(ref _currentIndex);
}
