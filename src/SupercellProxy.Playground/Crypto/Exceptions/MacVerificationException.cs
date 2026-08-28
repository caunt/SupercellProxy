namespace SupercellProxy.Playground.Crypto.Exceptions;

/// <summary>
/// Represents <c language="csharp">MacVerificationException</c>.
/// </summary>
internal sealed class MacVerificationException : NaClV3Exception
{
    internal const string DefaultMessage = "MAC verification failed";

    /// <summary>
    /// Gets or sets the <c language="csharp">IsPublicKeyBox</c> value.
    /// </summary>
    public bool IsPublicKeyBox { get; set; }

    /// <summary>
    /// Initializes a new <see cref="MacVerificationException"/> instance.
    /// </summary>
    public MacVerificationException() { }

    public MacVerificationException(string? message)
        : base(message) { }

    public MacVerificationException(string? message, Exception? innerException)
        : base(message, innerException) { }

    public MacVerificationException(bool isPublicKeyBox, string? message = null)
        : base(message)
    {
        IsPublicKeyBox = isPublicKeyBox;
    }
}
