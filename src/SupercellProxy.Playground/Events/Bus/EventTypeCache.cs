namespace SupercellProxy.Playground.Events.Bus;

public static class EventTypeCache<TEvent> where TEvent : IEvent
{
    public static readonly int Index = EventTypeIndex.NextIndex;
}
