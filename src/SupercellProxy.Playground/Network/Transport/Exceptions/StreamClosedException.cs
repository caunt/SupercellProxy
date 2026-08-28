namespace SupercellProxy.Playground.Network.Transport.Exceptions;

/// <summary>
/// Represents <c language="csharp">StreamClosedException</c>.
/// </summary>
internal sealed class StreamClosedException : Exception
{
    internal const string DefaultMessage = "end of stream";

    public StreamClosedException()
        : this(DefaultMessage) { }

    public StreamClosedException(string? message)
        : base(message) { }

    public StreamClosedException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
