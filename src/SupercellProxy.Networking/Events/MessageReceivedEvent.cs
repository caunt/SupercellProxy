using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Events;

/// <summary>
/// <para>Describes a message received by a proxy stream and its cancellation state.</para>
/// </summary>
public sealed record MessageReceivedEvent(IMessage Message, MessageDirection Direction, MessageStream Source, MessageStream Destination) : IEvent
{
    /// <summary>
    /// Gets or sets the <c language="csharp">IsCancelled</c> value.
    /// </summary>
    public bool IsCancelled { get; set; }
}
