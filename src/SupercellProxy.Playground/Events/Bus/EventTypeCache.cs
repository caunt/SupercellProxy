namespace SupercellProxy.Playground.Events.Bus;

internal static class EventTypeCache<TEvent>
    where TEvent : IEvent
{
    internal static readonly int Index = EventTypeIndex.NextIndex;
}
