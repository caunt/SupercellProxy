namespace SupercellProxy.Networking.Cryptography;

/// <summary>
/// Thrown when an encrypted message fails its Poly1305 check while being opened. It means the keys or nonce do not match the
/// sender's, usually because the two sides fell out of step, or that the bytes were changed in transit.
/// </summary>
public sealed class MacVerificationException : NaClV3Exception
{
    /// <summary>
    /// Text used when the encryption code reports a failed check.
    /// </summary>
    public const string DefaultMessage = "MAC verification failed";

    /// <summary>
    /// Creates a failed-check error with no description.
    /// </summary>
    public MacVerificationException() { }

    /// <summary>
    /// Creates a failed-check error described by text.
    /// </summary>
    /// <param name="message">What went wrong.</param>
    public MacVerificationException(string? message)
        : base(message) { }

    /// <summary>
    /// Creates a failed-check error caused by another error.
    /// </summary>
    /// <param name="message">What went wrong.</param>
    /// <param name="innerException">The error that caused it.</param>
    public MacVerificationException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Creates a failed-check error that records which kind of box failed.
    /// </summary>
    /// <param name="isPublicKeyBox">True for a handshake message opened with key pairs; false for a session message.</param>
    /// <param name="message">What went wrong; null leaves the default exception text.</param>
    public MacVerificationException(bool isPublicKeyBox, string? message = null)
        : base(message)
    {
        IsPublicKeyBox = isPublicKeyBox;
    }

    /// <summary>
    /// True when the failure happened while opening a handshake message sealed with key pairs; false for a message sealed with the
    /// shared session key.
    /// </summary>
    public bool IsPublicKeyBox { get; set; }
}
