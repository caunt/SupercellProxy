namespace SupercellProxy.Networking.Cryptography;

/// <summary>
/// Base for failures of the game's message encryption, so callers can catch every encryption problem in one place.
/// </summary>
public class NaClV3Exception : Exception
{
    /// <summary>
    /// Creates an encryption failure with no description.
    /// </summary>
    public NaClV3Exception() { }

    /// <summary>
    /// Creates an encryption failure described by text.
    /// </summary>
    /// <param name="message">What went wrong.</param>
    public NaClV3Exception(string? message)
        : base(message) { }

    /// <summary>
    /// Creates an encryption failure caused by another error.
    /// </summary>
    /// <param name="message">What went wrong.</param>
    /// <param name="innerException">The error that caused it.</param>
    public NaClV3Exception(string? message, Exception? innerException)
        : base(message, innerException) { }
}
