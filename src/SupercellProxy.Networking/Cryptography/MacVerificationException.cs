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
    /// Initializes a new instance of the <see cref="MacVerificationException"/> class. Creates a failed-check error with no description.
    /// </summary>
    public MacVerificationException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MacVerificationException"/> class. Creates a failed-check error described by text.
    /// </summary>
    /// <param name="message">What went wrong.</param>
    public MacVerificationException(string? message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MacVerificationException"/> class. Creates a failed-check error caused by another error.
    /// </summary>
    /// <param name="message">What went wrong.</param>
    /// <param name="innerException">The error that caused it.</param>
    public MacVerificationException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MacVerificationException"/> class. Creates a failed-check error that records which kind of box failed.
    /// </summary>
    /// <param name="isPublicKeyBox">True for a handshake message opened with key pairs; <see langword="false"/> for a session message.</param>
    /// <param name="message">What went wrong; <see langword="null"/> leaves the default exception text.</param>
    public MacVerificationException(bool isPublicKeyBox, string? message = null)
        : base(message)
    {
        IsPublicKeyBox = isPublicKeyBox;
    }

    /// <summary>
    /// Gets or sets whether the failure happened while opening a handshake message sealed with key pairs rather than a message sealed with the
    /// shared session key.
    /// </summary>
    /// <value><see langword="true"/> if the exception was created for a failed handshake (public-key) box; otherwise, <see langword="false"/>.</value>
    public bool IsPublicKeyBox { get; set; }
}
