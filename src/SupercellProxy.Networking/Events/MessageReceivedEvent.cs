using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Events;

/// <summary>
/// Published when the proxy has read a message and before it forwards it, so handlers can inspect it or drop it.
/// </summary>
/// <param name="Message">The decoded message.</param>
/// <param name="Direction">Whether it travels from the game client to the server or back.</param>
/// <param name="Source">Channel the message was read from.</param>
/// <param name="Destination">Channel the message will be written to unless a handler cancels it.</param>
public sealed record MessageReceivedEvent(IMessage Message, MessageDirection Direction, MessageChannel Source, MessageChannel Destination)
{
    /// <summary>
    /// Gets or sets whether a handler dropped the message, in which case the proxy does not forward it to the other side.
    /// </summary>
    /// <value><see langword="true"/> if a handler set it to drop the message instead of forwarding it to <see cref="Destination"/>; otherwise, <see langword="false"/>.</value>
    public bool IsCancelled { get; set; }
}
