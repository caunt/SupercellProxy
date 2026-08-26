using SupercellProxy.Playground.Events.Bus;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Events;

/// <summary>
/// Represents <c>MessageSentEvent</c>.
/// </summary>
public record MessageSentEvent(
    IMessage Message,
    Direction Direction,
    MessageStream Source,
    MessageStream Destination
) : IEvent;
