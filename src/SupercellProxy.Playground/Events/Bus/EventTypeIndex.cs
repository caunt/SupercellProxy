namespace SupercellProxy.Playground.Events.Bus;

/// <summary>
/// Represents <c language="csharp">EventTypeIndex</c>.
/// </summary>
internal static class EventTypeIndex
{
    private static int s_currentIndex = -1;

    /// <summary>
    /// Gets the <c language="csharp">NextIndex</c> value.
    /// </summary>
    public static int NextIndex => Interlocked.Increment(ref s_currentIndex);
}
