using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Events;

/// <summary>
/// Represents <c language="csharp">MessageSentEvent</c>.
/// </summary>
public sealed record MessageSentEvent(IMessage Message, MessageDirection Direction, MessageStream Source, MessageStream Destination) : IEvent;
