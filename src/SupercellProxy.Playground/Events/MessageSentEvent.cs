using SupercellProxy.Playground.Events.Bus;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Events;

/// <summary>
/// Represents <c language="csharp">MessageSentEvent</c>.
/// </summary>
internal sealed record MessageSentEvent(
    IMessage Message,
    Direction Direction,
    MessageStream Source,
    MessageStream Destination
) : IEvent;
