namespace SupercellProxy.Playground.Crypto.Exceptions;

/// <summary>
/// Represents <c>NaClV3Exception</c>.
/// </summary>
public class NaClV3Exception(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);
