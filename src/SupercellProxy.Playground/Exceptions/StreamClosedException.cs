namespace SupercellProxy.Playground.Exceptions;

public class StreamClosedException(string? message = "end of stream", Exception? innerException = null) : Exception(message, innerException);
