namespace SupercellProxy.Networking.Events;

/// <summary>
/// Represents <c language="csharp">EventTypeIndex</c>.
/// </summary>
public static class EventTypeIndex
{
    /// <summary>
    /// Gets the <c language="csharp">NextIndex</c> value.
    /// </summary>
    public static int NextIndex { get => Interlocked.Increment(ref field); } = -1;
}
