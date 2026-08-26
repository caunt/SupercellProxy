namespace SupercellProxy.Playground.Crypto.Exceptions;

/// <summary>
/// Represents <c>MacVerificationException</c>.
/// </summary>
public class MacVerificationException(string? message = null, Exception? innerException = null)
    : NaClV3Exception(message, innerException)
{
    /// <summary>
    /// Gets or sets the <c>IsPublicKeyBox</c> value.
    /// </summary>
    public bool IsPublicKeyBox { get; set; }

    /// <summary>
    /// Initializes a new <see cref="MacVerificationException"/> instance.
    /// </summary>
    public MacVerificationException(
        bool isPublicKeyBox,
        string? message = null,
        Exception? innerException = null
    )
        : this(message, innerException)
    {
        IsPublicKeyBox = isPublicKeyBox;
    }
}
