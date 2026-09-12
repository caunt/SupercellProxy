namespace SupercellProxy.Networking.Events;

/// <summary>
/// Defines the Event Type Cache&lt;TEvent&gt; contract.
/// </summary>
internal static class EventTypeCache<TEvent>
    where TEvent : IEvent
{
    /// <summary>
    /// Provides the Index value or operation.
    /// </summary>
    public static readonly int Index = EventTypeIndex.NextIndex;
}
