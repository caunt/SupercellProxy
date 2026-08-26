using SupercellProxy.Playground.Events.Bus;
using SupercellProxy.Playground.Network.Messages;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Events;

/// <summary>
/// <para>Describes a message received by a proxy stream and its cancellation state.</para>
/// </summary>
public record MessageReceivedEvent(
    IMessage Message,
    Direction Direction,
    MessageStream Source,
    MessageStream Destination
) : IEvent
{
    /// <summary>
    /// Gets or sets the <c>IsCancelled</c> value.
    /// </summary>
    public bool IsCancelled { get; set; }
}
