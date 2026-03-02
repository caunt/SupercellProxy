using SupercellProxy.Playground.Events.Bus;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Streams;

namespace SupercellProxy.Playground.Events;

public record MessageSentEvent(IMessage Message, Direction Direction, SupercellStream Source, SupercellStream Destination) : IEvent;
