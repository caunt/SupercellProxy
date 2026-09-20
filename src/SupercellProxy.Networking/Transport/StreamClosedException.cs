using SupercellProxy.Networking.Protocol.ConnectionControl;

namespace SupercellProxy.Networking.Transport;

/// <summary>
/// Represents <c language="csharp">StreamClosedException</c>.
/// </summary>
public sealed class StreamClosedException : IOException
{
    /// <summary>
    /// Provides the Default Message value or operation.
    /// </summary>
    public const string DefaultMessage = "end of stream";

    /// <summary>
    /// Provides the Stream Closed Exception value or operation.
    /// </summary>
    public StreamClosedException()
        : this(DefaultMessage) { }

    /// <summary>
    /// Provides the Stream Closed Exception value or operation.
    /// </summary>
    public StreamClosedException(string? message)
        : base(message) { }

    /// <summary>
    /// Provides the Stream Closed Exception value or operation.
    /// </summary>
    public StreamClosedException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Provides the Stream Closed Exception value or operation.
    /// </summary>
    public StreamClosedException(string? message, DisconnectReason reason)
        : base(message)
    {
        Reason = reason;
    }

    /// <summary>Gets the server-reported disconnect reason, when the server sent one.</summary>
    public DisconnectReason? Reason { get; }
}
