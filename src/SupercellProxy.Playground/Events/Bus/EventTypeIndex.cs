namespace SupercellProxy.Playground.Events.Bus;

/// <summary>
/// Represents <c>EventTypeIndex</c>.
/// </summary>
public static class EventTypeIndex
{
    private static int _currentIndex = -1;

    /// <summary>
    /// Gets the <c>NextIndex</c> value.
    /// </summary>
    public static int NextIndex => Interlocked.Increment(ref _currentIndex);
}
