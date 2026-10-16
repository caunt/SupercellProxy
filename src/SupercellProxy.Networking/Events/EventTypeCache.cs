namespace SupercellProxy.Networking.Events;

/// <summary>
/// Gives each event type a fixed slot number the first time it is used, so the event bus finds handlers by array position instead of
/// a dictionary lookup.
/// </summary>
internal static class EventTypeCache<TEvent>
    where TEvent : IEvent
{
    /// <summary>
    /// Slot of <typeparamref name="TEvent"/> in the event bus's handler array; stays the same for the life of the process.
    /// </summary>
    public static readonly int Index = EventTypeIndex.NextIndex;
}
