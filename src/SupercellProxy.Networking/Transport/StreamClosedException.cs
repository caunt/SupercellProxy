using SupercellProxy.Networking.Protocol.ConnectionControl;

namespace SupercellProxy.Networking.Transport;

/// <summary>
/// Thrown when a game connection ends while a message is being read or written: the socket closed, or the server sent a disconnect
/// notice. Callers usually reconnect; <see cref="Reason"/> tells a server-requested disconnect apart from a dropped socket.
/// </summary>
public sealed class StreamClosedException : IOException
{
    /// <summary>
    /// Text used when the socket simply reached its end without further explanation.
    /// </summary>
    public const string DefaultMessage = "end of stream";

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamClosedException"/> class. Creates a closed-connection error with the default text.
    /// </summary>
    public StreamClosedException()
        : this(DefaultMessage) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamClosedException"/> class. Creates a closed-connection error described by text.
    /// </summary>
    /// <param name="message">Which side closed and how.</param>
    public StreamClosedException(string? message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamClosedException"/> class. Creates a closed-connection error caused by a lower-level socket or stream error.
    /// </summary>
    /// <param name="message">Which side closed and how.</param>
    /// <param name="innerException">The socket or stream error that revealed the closure.</param>
    public StreamClosedException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamClosedException"/> class. Creates a closed-connection error for a disconnect the server announced.
    /// </summary>
    /// <param name="message">Which side closed and how.</param>
    /// <param name="reason">Why the server ended the session, as it reported.</param>
    public StreamClosedException(string? message, DisconnectReason reason)
        : base(message)
    {
        Reason = reason;
    }

    /// <summary>Gets the server-reported disconnect reason, when the server sent one.</summary>
    /// <value>The reason from the server's disconnect notice, or <see langword="null"/> when the socket simply closed or failed.</value>
    public DisconnectReason? Reason { get; }
}
