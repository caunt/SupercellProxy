using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Events;

/// <summary>
/// Published after the proxy has forwarded a message to the other side; too late to change or drop it.
/// </summary>
/// <param name="Message">The decoded message that was forwarded.</param>
/// <param name="Direction">Whether it travelled from the game client to the server or back.</param>
/// <param name="Source">Channel the message was read from.</param>
/// <param name="Destination">Channel the message was written to.</param>
public sealed record MessageSentEvent(IMessage Message, MessageDirection Direction, MessageChannel Source, MessageChannel Destination);
