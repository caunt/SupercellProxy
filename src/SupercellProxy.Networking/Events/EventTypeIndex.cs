namespace SupercellProxy.Networking.Events;

/// <summary>
/// Hands out event slot numbers, shared by all event types in the process.
/// </summary>
public static class EventTypeIndex
{
    /// <summary>
    /// Reserves and returns the next unused slot number, starting at 0; each read takes a new number, safely across threads.
    /// </summary>
    public static int NextIndex { get => Interlocked.Increment(ref field); } = -1;
}
