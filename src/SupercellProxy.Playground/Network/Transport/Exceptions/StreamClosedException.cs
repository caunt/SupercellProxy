namespace SupercellProxy.Playground.Network.Transport.Exceptions;

/// <summary>
/// Represents <c>StreamClosedException</c>.
/// </summary>
public class StreamClosedException(
    string? message = "end of stream",
    Exception? innerException = null
) : Exception(message, innerException);
