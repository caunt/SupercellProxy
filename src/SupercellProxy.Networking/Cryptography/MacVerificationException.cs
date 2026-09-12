namespace SupercellProxy.Networking.Cryptography;

/// <summary>
/// Represents <c language="csharp">MacVerificationException</c>.
/// </summary>
public sealed class MacVerificationException : NaClV3Exception
{
    /// <summary>
    /// Provides the Default Message value or operation.
    /// </summary>
    public const string DefaultMessage = "MAC verification failed";

    /// <summary>
    /// Initializes a new <see cref="MacVerificationException"/> instance.
    /// </summary>
    public MacVerificationException() { }

    /// <summary>
    /// Provides the Mac Verification Exception value or operation.
    /// </summary>
    public MacVerificationException(string? message)
        : base(message) { }

    /// <summary>
    /// Provides the Mac Verification Exception value or operation.
    /// </summary>
    public MacVerificationException(string? message, Exception? innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Provides the Mac Verification Exception value or operation.
    /// </summary>
    public MacVerificationException(bool isPublicKeyBox, string? message = null)
        : base(message)
    {
        IsPublicKeyBox = isPublicKeyBox;
    }

    /// <summary>
    /// Gets or sets the <c language="csharp">IsPublicKeyBox</c> value.
    /// </summary>
    public bool IsPublicKeyBox { get; set; }
}
